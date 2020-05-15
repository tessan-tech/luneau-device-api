using DeviceApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.MixedReality.WebRTC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicrosoftPeerConnection = Microsoft.MixedReality.WebRTC.PeerConnection;

namespace DeviceApi.Services
{
    public class PeerConnection: IDisposable
    {
        private MicrosoftPeerConnection Peer;
        private Action<Sdp> OnSdp;
        private Action<IceCandidate> OnIceCandidate;
        private TaskCompletionSource<PeerConnection> Initialization;

        public PeerConnection(Action<Sdp> onSdp, Action<IceCandidate> onIceCandidate)
        {
            OnSdp = onSdp;
            OnIceCandidate = onIceCandidate;
            Initialization = new TaskCompletionSource<PeerConnection>();
        }

        public void Dispose()
        {
            Peer.Dispose();
        }

        public async Task Init()
        {
            Peer = new MicrosoftPeerConnection();

            Peer.IceCandidateReadytoSend += (string candidate, int sdpMlineindex, string sdpMid) =>
            {
                OnIceCandidate.Invoke(new IceCandidate(candidate, sdpMlineindex, sdpMid));
            };

            Peer.LocalSdpReadytoSend += (string type, string sdp) =>
            {
                OnSdp.Invoke(new Sdp(type, sdp));
            };

            var config = new PeerConnectionConfiguration
            {
                IceServers = new List<IceServer> {
                    new IceServer{ Urls = { "stun:stun.l.google.com:19302" } }
                },
            };
            await Peer.InitializeAsync(config);
            await Peer.AddLocalVideoTrackAsync();
            Initialization.SetResult(this);
            Console.WriteLine("Peer connection initialized.");
        }

        public async void SetRemoteIceCandidate(IceCandidate iceCandidate)
        {
            await Initialization.Task;
            Peer.AddIceCandidate(iceCandidate.sdpMid, iceCandidate.sdpMlineindex, iceCandidate.candidate);
        }

        public async void SetRemoteSdp(Sdp sdp)
        {
            await Initialization.Task;
            Peer.SetRemoteDescription(sdp.type, sdp.sdp);
            Peer.CreateAnswer();
        }
    }
}
