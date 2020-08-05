using Automaton;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DeviceApi
{
    internal class DeviceHub : Hub
    {
        private readonly HubService HubService;
        private readonly ACommunicationProtocol CommunicationProtocol;

        public DeviceHub(HubService hubService, ACommunicationProtocol communicationProtocol)
        {
            HubService = hubService;
            CommunicationProtocol = communicationProtocol;
        }

        public void Command(string name, JToken payload)
        {
            CommunicationProtocol.SubmitCommand(name, payload);
        }

        public override Task OnConnectedAsync()
        {
            CommunicationProtocol.UserConnected(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
