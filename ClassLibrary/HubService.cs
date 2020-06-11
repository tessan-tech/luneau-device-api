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
            CommunicationProtocol.SetSendUrlDelegate(url => HubContext.Clients.All.SendCoreAsync("videoUrl", new object[] { url }));
        }

        public void NotifyUserConnection(string userId)
        {
            CommunicationProtocol.NotifyUserConnection(userId);
        }

        public void NotifyUserDisconnection(string userId)
        {
            CommunicationProtocol.NotifyUserDisconnection(userId);
        }
    }
}
