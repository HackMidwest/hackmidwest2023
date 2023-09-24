using Microsoft.Extensions.Options;
using PennyArtApi.Models;
using PennyArtApi.Models.Pinata;
using PennyArtApi.Models.Responses;
using RestSharp;
using System.Text.Json;
using System.Linq;

namespace PennyArtApi.ExternalServices
{
    public class PinataClient : IPinataClient
    {
        private readonly ILogger<PinataClient> _logger;
        private readonly PinataOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ICrossmintClient _crossmintClient;

        public PinataClient(ILogger<PinataClient> logger, IOptions<PinataOptions> options, HttpClient httpClient, ICrossmintClient crossmintClient)
        {
            _logger = logger;
            _options = options.Value;
            _httpClient = httpClient;
            _crossmintClient = crossmintClient;
        }

        public async Task<PinFileResponse?> PinFileToIpfsAsync(Stream file, string filename, string userId)
        {
            try
            {
                PinMetadata pinMetadata = new()
                {
                    name = filename
                };
                pinMetadata.keyvalues.Add("userId", userId);
                var sMeta = JsonSerializer.Serialize(pinMetadata);

                var client = new RestClient();
                var request = new RestRequest($"{_options.BaseUrl}/pinning/pinFileToIPFS", Method.Post);
                request.AddHeader("Authorization", $"Bearer {_options.ApiJwt}");
                request.AlwaysMultipartFormData = true;
                request.AddFile("file", ((MemoryStream)file).ToArray(), filename);
                request.AddParameter("pinataMetadata", sMeta);
                request.AddParameter("pinataOptions", "{\"cidVersion\":0}");
                RestResponse response = await client.ExecuteAsync(request);
                var pResponse = JsonSerializer.Deserialize<PinFileResponse>(response.Content);

                await _crossmintClient.MintNft($"{_options.GatewayUrl}/ipfs/{pResponse.IpfsHash}", "0x78394Bed766e66be178220924A734F2E13237B54");
                await UpdateWithTags(pResponse, userId);

                return pResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PinataClient.PinFileToIpfsAsync Error");
            }
            
            return null;
        }

        public async Task<IEnumerable<DocResponse>> SearchByUserId(string userId)
        {
            var client = new RestClient();
            var request = new RestRequest($"{_options.BaseUrl}/data/pinList", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_options.ApiJwt}");
            request.AddHeader("Accept", "application/json");
            request.AddQueryParameter("status", "pinned");
            request.AddQueryParameter("pageLimit", "1000");
            request.AddQueryParameter("metadata[keyvalues][userId]", "{\"value\":\"" + userId + "\",\"op\":\"eq\"}");
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var pResponse = JsonSerializer.Deserialize<PinListResponse>(response.Content);

                List<DocResponse> dResponse = new();
                foreach (var item in pResponse.rows)
                {
                    dResponse.Add(DocResponse.FromPinListResponseRow(item, _options.GatewayUrl));
                }

                return dResponse;
            }

            return new List<DocResponse>();
        }

        private async Task UpdateWithTags(PinFileResponse pin, string userId)
        {
            ImageAnalyzer ia = new();
            var tags = await ia.AnalyzeAsync($"{_options.GatewayUrl}/ipfs/{pin.IpfsHash}");
            var sTags = string.Join(",", tags);
            PinUpdateMetadata metadata = new();
            metadata.ipfsPinHash = pin.IpfsHash;
            
            metadata.keyvalues.Add("tags", sTags);
            metadata.keyvalues.Add("userId", userId);

            var client = new RestClient();
            var request = new RestRequest($"{_options.BaseUrl}/pinning/hashMetadata", Method.Put);
            request.AddHeader("Authorization", $"Bearer {_options.ApiJwt}");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(JsonSerializer.Serialize(metadata));

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {

            }
        }
    }
}
