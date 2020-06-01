using System;
using System.Drawing;
using System.Threading.Tasks;

namespace DeviceApi
{
    public abstract class ACommunicationProtocol
    {
        private Action<Image> SendBitmapDelegate;

        internal void SetBitmapDelegate(Action<Image> action)
        {
            SendBitmapDelegate = action;
        }

        internal ItemAddResult AddItem(Item item, int quantity)
        {
            return OnAddItem(item, quantity);
        }

        internal Task RequestVideo(string cameraName)
             => Task.Run(() => OnVideoRequested(cameraName));

        protected abstract void OnVideoRequested(string cameraName);

        protected abstract ItemAddResult OnAddItem(Item item, int quantity);

        public void SendBitmap(Image bitmap)
        {
            SendBitmapDelegate?.Invoke(bitmap);
        }
    }
}
