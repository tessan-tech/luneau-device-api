using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DeviceApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;

namespace DeviceApi.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RTCHub: Hub
    {
        private static Dictionary<string, PeerConnection> PeerConnections;
        private IHubContext<RTCHub> HubContext;
        static RTCHub()
        {
            PeerConnections = new Dictionary<string, PeerConnection>();
        }

        public RTCHub(IHubContext<RTCHub> hubContext)
        {
            HubContext = hubContext;
        }

        private async Task<PeerConnection> InitPeerConnection(string connectionId)
        {
            PeerConnection peerConnection = new PeerConnection(
                sdp => SendSdp(connectionId, sdp),
                candidate => SendIceCandidate(connectionId, candidate));
            PeerConnections.Add(connectionId, peerConnection);
            await peerConnection.Init();
            return peerConnection;
        }

        public override Task OnConnectedAsync()
        {
            if (PeerConnections.Count > 0)
                throw new HubException("WebRTC does not currently support multiple connections.");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            if (PeerConnections.TryGetValue(connectionId, out var peer))
            {
                peer.Dispose();
                PeerConnections.Remove(connectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async void AddIceCandidate(IceCandidate iceCandidate)
        {
            string connectionId = Context.ConnectionId;
            PeerConnection peer = PeerConnections.GetValueOrDefault(connectionId)
                ?? await InitPeerConnection(connectionId);
            peer.SetRemoteIceCandidate(iceCandidate);
        }

        public async void AddSdp(Sdp sdp)
        {
            string connectionId = Context.ConnectionId;
            PeerConnection peer = PeerConnections.GetValueOrDefault(connectionId)
                ?? await InitPeerConnection(connectionId);
            peer.SetRemoteSdp(sdp);
        }

        private Task SendIceCandidate(string connectionId, IceCandidate iceCandidate)
            => HubContext.Clients.Client(connectionId).SendCoreAsync("iceCandidate", new object[] { iceCandidate });

        private Task SendSdp(string connectionId, Sdp sdp)
        => HubContext.Clients.Client(connectionId).SendCoreAsync("sdp", new object[] { sdp });
    }
}
