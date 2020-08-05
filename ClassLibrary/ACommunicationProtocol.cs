using Automaton;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace DeviceApi
{
    public abstract class ACommunicationProtocol
    {
        private Action<Status> SendStatusDelegate;
        private Action<Image> SendImageDelegate;
        private Action<FileStream> SendPdfDelegate;
        private Action<Command[]> SendAvailableCommandsDelegate;

        internal void SetSendStatusDelegate(Action<Status> action)
        {
            SendStatusDelegate = action;
        }

        internal void SetSendImageDelegate(Action<Image> action)
        {
            SendImageDelegate = action;
        }

        internal void SetSendAvailableCommandsDelegate(Action<Command[]> action)
        {
            SendAvailableCommandsDelegate = action;
        }

        internal void SetSendPdfDelegate(Action<FileStream> action)
        {
            SendPdfDelegate = action;
        }

        internal void SubmitCommand(string command, JToken payload) => OnCommand(command, payload);

        internal void UserConnected(string userId) => OnUserConnected(userId);

        protected abstract void OnUserConnected(string userId);

        protected abstract void OnCommand(string command, JToken payload);

        public void SendStatus(Status status)
        {
            SendStatusDelegate?.Invoke(status);
        }

        public void SendImage(Image image)
        {
            SendImageDelegate?.Invoke(image);
        }

        public void SendPdf(FileStream pdf)
        {
            SendPdfDelegate?.Invoke(pdf);
        }

        public void SendAvailableCommands(Command[] commands)
        {
            SendAvailableCommandsDelegate?.Invoke(commands);
        }
    }
}
