namespace PennyArtApi.Models.Pinata
{
    public class PinFileResponse
    {
        public string IpfsHash { get; set; }
        public int PinSize {  get; set; }
        public string Timestamp { get; set; }
    }
}
