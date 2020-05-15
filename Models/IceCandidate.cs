namespace DeviceApi.Services
{
    public class IceCandidate
    {
        public IceCandidate() { }
        public IceCandidate(string candidate, int sdpMlineindex, string sdpMid)
        {
            this.candidate = candidate;
            this.sdpMlineindex = sdpMlineindex;
            this.sdpMid = sdpMid;
        }

        public string candidate { get; set; }
        public int sdpMlineindex { get; set; }
        public string sdpMid { get; set; }
    }
}
