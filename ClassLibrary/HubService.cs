using Microsoft.AspNetCore.SignalR;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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
            communicationProtocol.SetBitmapDelegate((bitmap) =>
            {

                var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);

                HubContext.Clients.All.SendCoreAsync("image", new object[] { ms.ToArray() });

                Console.WriteLine($"bitmap {bitmap} sent on bridge");
            });

            PeriodicMessage();
        }

        public async void PeriodicMessage()
        {
            while (true)
            {
                await Task.Delay(5000);
                SendMessage("hello !");
            }
        }

        public void RequestVideo()
        {
            CommunicationProtocol.RequestVideo("camera1");
        }

        public void SendMessage(string message)
        {
            HubContext.Clients.All.SendCoreAsync("message", new object[] { message });
        }

        public ItemAddResult AddItem(Item item, int quantity)
        {
            return CommunicationProtocol.AddItem(item, quantity);
        }

        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
