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
        private Action<Image, string> SendImageDelegate;
        private Action<Stream> SendExamResultDelegate;
        private Action<Command[]> SendAvailableCommandsDelegate;

        internal void SetSendStatusDelegate(Action<Status> action)
        {
            SendStatusDelegate = action;
        }

        internal void SetSendImageDelegate(Action<Image, string> action)
        {
            SendImageDelegate = action;
        }

        internal void SetSendAvailableCommandsDelegate(Action<Command[]> action)
        {
            SendAvailableCommandsDelegate = action;
        }

        internal void SetSendExamResult(Action<Stream> action)
        {
            SendExamResultDelegate = action;
        }

        internal void SubmitCommand(string command, JToken payload) => OnCommand(command, payload);

        internal void UserConnected(string userId) => OnUserConnected(userId);

        protected abstract void OnUserConnected(string userId);

        protected abstract void OnCommand(string command, JToken payload);

        public void SendStatus(Status status)
        {
            SendStatusDelegate?.Invoke(status);
        }

        public void SendImage(Image image, string cameraName)
        {
            SendImageDelegate?.Invoke(image, cameraName);
        }

        public void SendExamResult(Stream result)
        {
            SendExamResultDelegate?.Invoke(result);
        }

        public void SendAvailableCommands(Command[] commands)
        {
            SendAvailableCommandsDelegate?.Invoke(commands);
        }
    }
}
