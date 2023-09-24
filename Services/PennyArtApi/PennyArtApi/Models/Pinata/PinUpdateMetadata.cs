namespace PennyArtApi.Models.Pinata
{
    public class PinUpdateMetadata
    {
        public string ipfsPinHash { get; set; }
        public Dictionary<string, string> keyvalues { get; set; } = new();
    }
}
