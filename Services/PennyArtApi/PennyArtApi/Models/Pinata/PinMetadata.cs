namespace PennyArtApi.Models.Pinata
{
    public class PinMetadata
    {
        public string name { get; set; }
        public Dictionary<string, string> keyvalues { get; set; } = new();
    }
}
