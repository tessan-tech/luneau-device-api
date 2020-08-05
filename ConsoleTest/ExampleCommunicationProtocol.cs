using Automaton;
using DeviceApi;
using Newtonsoft.Json.Linq;
using System;

namespace ConsoleTest
{
    class ExampleCommunicationProtocol : ACommunicationProtocol
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
}
