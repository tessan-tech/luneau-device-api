using Accord;
using Accord.Diagnostics;
using Accord.Video.FFMPEG;
using Automaton;
using DeviceApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class MyCommunicationProtocol : ACommunicationProtocol
    {
        protected override void OnCommand(string command, JToken payload)
        {
            if (!Enum.TryParse(command, out Command commandEnum))
            {
                SendStatus(new Status("COMMAND_UNAVAILABLE", new { reason = $"{command} does not exist" }));
                return;
            }
            if (!Program.SM.Can(commandEnum))
            {
                SendStatus(new Status("COMMAND_UNAVAILABLE", new { reason = "device state does not permit " + command.ToString() }));
                return;
            }
            switch (commandEnum)
            {
                case Command.POWER_OFF:
                    Program.SM.ResetAvailableCommands();
                    break;
                case Command.REGISTER_PATIENT:
                    Program.vTX.RegisterPatient(payload.ToObject<RegisterPatientInput>());
                    break;
                case Command.ABORT_EXAMS:
                    Program.vTX.AbortExams();
                    break;
                case Command.START_EXAMS:
                    Program.vTX.StartExams(payload.ToObject<ExamInput>());
                    break;
                case Command.UP:
                    Program.vTX.Up(payload.ToObject<MoveCameraInput>());
                    break;
                case Command.DOWN:
                    Program.vTX.Down(payload.ToObject<MoveCameraInput>());
                    break;
            }
        }

        protected override void OnUserConnected(string userId)
        {
            SendAvailableCommands(Program.SM.GetAvailableCommands());
        }
    }

    class Program
    {
        public static StateMachine SM = new StateMachine();
        public static VTX600 vTX;

        static void Main(string[] args)
        {
            SM.AddAvailableCommands(KeyStates.Home);
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var communicationProtocol = new MyCommunicationProtocol();
            SM.CommandsChangedEvent += (e, arguments) => communicationProtocol.SendAvailableCommands(arguments.Commands);
            vTX = new VTX600(communicationProtocol, SM);
            var xmlPublicKey = "<RSAKeyValue><Modulus>l37+bbDzBGYFxEKRfBcJENV4xPP0i81LRYfJH62RrOCadt8L3kf6ANtQd1ZipzpFyWvQCt9g0VjMNsELZVAlxReWr+NeS1KruBwT2yiE6umW2AkmXv9uE/JeoZaOsgodAPtBx/WgfgHn1YLwZQOqU7O4gajpTq1GwTTYq1F96k3KNkBUdFsWRM6BonvOayPNkZIYQMySS7Sn0kkvWNqONxzEN/PGZvkGITDEoEtuMR+90XCDQJM7HiFFYXVyrihOu7uURKlA74K4eA/I6bCHkD5NDtK4SU7Aem1GzsXzM/1gUvFJ5wY9etyPSIuMIz/micOho71duPRSNB6rfyMRpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            await DeviceApiWebHost.RunWebHostBuilder(args, xmlPublicKey, communicationProtocol);
        }
    }
}
