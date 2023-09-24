using PennyArtApi.Models.Pinata;

namespace PennyArtApi.Models.Responses
{
    public class DocResponse
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        public static DocResponse FromPinListResponseRow(PinListResponseRow row, string gatewayUrl)
        {
            return new DocResponse
            {
                Name = row.metadata.name,
                Url = $"{gatewayUrl}/ipfs/{row.ipfs_pin_hash}",
                Tags = row.metadata.keyvalues.ContainsKey("tags") ? row.metadata.keyvalues["tags"] : string.Empty
            };
        }
    }
}
