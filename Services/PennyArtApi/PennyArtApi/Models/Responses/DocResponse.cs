using PennyArtApi.Models.Pinata;

namespace PennyArtApi.Models.Responses
{
    public class DocResponse
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public static DocResponse FromPinListResponseRow(PinListResponseRow row, string gatewayUrl)
        {
            return new DocResponse
            {
                Name = row.metadata.name,
                Url = $"{gatewayUrl}/ipfs/{row.ipfs_pin_hash}"
            };
        }
    }
}
