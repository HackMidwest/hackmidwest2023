namespace PennyArtApi.ExternalServices
{
    public interface ICrossmintClient
    {
        Task MintNft(string url, string wallet);
    }
}