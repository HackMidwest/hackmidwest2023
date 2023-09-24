using PennyArtApi.Models.Pinata;
using PennyArtApi.Models.Responses;

namespace PennyArtApi.ExternalServices
{
    public interface IPinataClient
    {
        Task<PinFileResponse?> PinFileToIpfsAsync(Stream file, string filename, string userId);
        Task<IEnumerable<DocResponse>> SearchByTags(string tagset);
        Task<IEnumerable<DocResponse>> SearchByUserId(string userId);
    }
}