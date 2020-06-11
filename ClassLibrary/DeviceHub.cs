using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DeviceApi
{
    internal class DeviceHub : Hub
    {
        private readonly HubService HubService;
        public DeviceHub(HubService hubService)
        {
            HubService = hubService;
        }

        public override Task OnConnectedAsync()
        {
            HubService.NotifyUserConnection(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            HubService.NotifyUserDisconnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
