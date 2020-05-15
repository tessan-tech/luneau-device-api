namespace DeviceApi.Services
{
    public class Sdp
    {
        public Sdp() { }
        public Sdp(string type, string sdp)
        {
            this.type = type;
            this.sdp = sdp;
        }

        public string type { get; set; }
        public string sdp { get; set; }
    }
}
