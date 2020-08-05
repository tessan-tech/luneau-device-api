using Accord.Video.FFMPEG;
using Automaton;
using DeviceApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class VTX600
    {
        private CancellationTokenSource[] ExamCancellationSources;
        private readonly ACommunicationProtocol Protocol;
        private readonly StateMachine StateMachine;

        public VTX600(ACommunicationProtocol protocol, StateMachine stateMachine)
        {
            Protocol = protocol;
            StateMachine = stateMachine;
            ExamCancellationSources = new CancellationTokenSource[0];
        }

        public async Task StartExams(ExamInput input)
        {
            AbortExams();
            StateMachine.ResetAvailableCommands(KeyStates.ExamsInProgress);
            var cancellationTokens = input.Exams.Select(e => CreateExamCancellation()).ToArray();
            foreach (Task exam in input.Exams.Select((e, i) => StartExam(e, cancellationTokens[i])))
                await exam;
            Protocol.SendPdf(File.OpenRead("./sample.pdf"));
            StateMachine.ResetAvailableCommands(KeyStates.Home);
        }

        public void AbortExams()
        {
            StateMachine.ResetAvailableCommands(KeyStates.Home);
            foreach(var source in ExamCancellationSources)
                source.Cancel();
            ExamCancellationSources = new CancellationTokenSource[] { };
        }

        public void RegisterPatient(RegisterPatientInput input)
        {
            Protocol.SendStatus(new Status("PATIENT_REGISTRATION_RESULT", input));
        }

        public void Up(MoveCameraInput input)
        {
            Protocol.SendStatus(new Status("AIMING_UP", input));
        }

        public void Down(MoveCameraInput input)
        {
            Protocol.SendStatus(new Status("AIMING_DOWN", input));
        }

        private Task StartExam(string examName, CancellationToken token)
        {
            switch (examName)
            {
                case "WF":
                    return WF(token);
                case "TOPO":
                    return Topo(token);
                case "FONDUS":
                    return Fondus(token);
                default:
                    Protocol.SendStatus(new Status("EXAM_UNAVAIBLABLE", new { Name = examName }));
                    return Task.CompletedTask;
            }
        }

        private IEnumerable<Image> GetVideoImages(string fileName)
        {
            using (var vFReader = new VideoFileReader())
            {
                vFReader.Open(fileName);
                for (int i = 0; i < vFReader.FrameCount; i++)
                {
                    Bitmap bmpBaseOriginal = vFReader.ReadVideoFrame(i);
                    yield return bmpBaseOriginal;
                }
                vFReader.Close();
            }
        }

        private async Task MockExam(string examName, CancellationToken token)
        {
            var random = new Random();
            var failureReason = new string[] { "cannot find the eye", "calibration error", "image acquisition failed" };
            Protocol.SendStatus(new Status($"STARTING_EXAM", new { Name = examName.ToUpper() }));
            await Task.Delay(2000, token).ContinueWith(task => { });
            if (token.IsCancellationRequested)
            {
                SendExamAborted(examName);
                return;
            }
            bool isSuccessfull = random.Next(10) > 3;
            if (!isSuccessfull)
            {
                Protocol.SendStatus(new Status($"FAILED_EXAM", new { Name = examName.ToUpper(), Reason = failureReason[random.Next(3)] }));
                return;
            }
            await Task.Delay(4000, token).ContinueWith(task => {});
            if (token.IsCancellationRequested)
            {
                SendExamAborted(examName);
                return;
            }
            Protocol.SendStatus(new Status($"RESULT_EXAM", new { Name = examName.ToUpper(), Result = "some result" }));
        }

        private void SendExamAborted(string examName)
        {
            Protocol.SendStatus(new Status($"FAILED_EXAM", new { Name = examName.ToUpper(), Reason = "exam was aborted" }));
        }

        public void StartCamera(CancellationToken token)
        {
            Task.Run(async () =>
            {
                foreach (var image in GetVideoImages("./sample1.mp4"))
                {
                    if (token.IsCancellationRequested) break;
                    Protocol.SendImage(image);
                    await Task.Delay(50);
                }
            }, token);
        }

        private CancellationToken CreateExamCancellation()
        {
            var tokenSource = new CancellationTokenSource();
            ExamCancellationSources = ExamCancellationSources.Append(tokenSource).ToArray();
            return tokenSource.Token;
        }

        private Task WF(CancellationToken token)
        {
            StartCamera(token);
            return MockExam("WF", token);
        }

        private Task Topo(CancellationToken token)
        {
            return MockExam("Topo", token);
        }

        private Task Fondus(CancellationToken token)
        {
            return MockExam("Fondus", token);
        }
    }

    public class ExamInput
    {
        public string[] Exams;
    }

    public class MoveCameraInput
    {
        public int Delta;
    }

    public class RegisterPatientInput
    {
        public string Firstname;
        public string Lastname;
    }
}
