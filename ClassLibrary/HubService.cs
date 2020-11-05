using Automaton;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceApi
{
    class HubService
    {
        private readonly IHubContext<DeviceHub> HubContext;
        private readonly ACommunicationProtocol CommunicationProtocol;

        public HubService(IHubContext<DeviceHub> hubContext, ACommunicationProtocol communicationProtocol)
        {
            HubContext = hubContext;
            CommunicationProtocol = communicationProtocol;
            CommunicationProtocol.SetSendStatusDelegate(SendStatus);
            CommunicationProtocol.SetSendAvailableCommandsDelegate(SendAvailableCommands);
            CommunicationProtocol.SetSendImageDelegate(SendImage);
            CommunicationProtocol.SetSendExamResult(SendExamResult);
        }

        private void SendStatus(Status status)
        {
            Send("status", status);
        }

        private async void SendImage(Image image, string cameraName)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);

            await Send("image", (object)ms.ToArray(), cameraName);
        }

        private async void SendExamResult(Stream result)
        {
            var ms = new MemoryStream();
            await result.CopyToAsync(ms);
            await Send("examResult", (object)ms.ToArray());
        }

        private void SendAvailableCommands(Command[] commands)
        {
            Send("availableCommands", commands.Select(c => c.ToString()));
        }

        private Task Send(string verb, params object[] objects)
            => HubContext.Clients.All.SendCoreAsync(verb, objects);
    }
}
