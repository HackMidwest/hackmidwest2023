namespace PennyArtApi.Models.Pinata
{
    public class PinListResponse
    {
        public int count { get; set; }
        public List<PinListResponseRow> rows { get; set; }
    }

    public class PinListResponseRow
    {
        public string id { get; set; }
        public string ipfs_pin_hash { get; set; }
        public string date_pinned { get; set; }
        public PinMetadata metadata { get; set; }
    }
}
