using System;
using System.Drawing;
using System.Threading.Tasks;

namespace DeviceApi
{
    public abstract class ACommunicationProtocol
    {
        private Action<string> SendVideoUrlDelegate;

        internal void SetSendUrlDelegate(Action<string> action)
        {
            SendVideoUrlDelegate = action;
        }

        internal void NotifyUserConnection(string Id)
            => OnUserConnected(Id);

        protected abstract void OnUserConnected(string Id);

        internal void NotifyUserDisconnection(string Id)
            => OnUserDiconnected(Id);

        protected abstract void OnUserDiconnected(string Id);

        public void SendVideoUrl(string url)
        {
            SendVideoUrlDelegate?.Invoke(url);
        }
    }
}
