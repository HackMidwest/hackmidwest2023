using Microsoft.Extensions.Options;
using PennyArtApi.Models;
using RestSharp;

namespace PennyArtApi.ExternalServices
{
    public class CrossmintClient : ICrossmintClient
    {
        private readonly ILogger<CrossmintClient> _logger;
        private readonly CrossmintOptions _options;

        public CrossmintClient(ILogger<CrossmintClient> logger, 
            IOptions<CrossmintOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task MintNft(string url, string wallet)
        {
            // 0x78394Bed766e66be178220924A734F2E13237B54
            var client = new RestClient();
            var request = new RestRequest($"{_options.BaseUrl}/api/2022-06-09/collections/default/nfts", Method.Post);
            request.AddHeader("x-client-secret", _options.ApiKey);
            request.AddHeader("x-project-id", _options.ProjectId);
            request.AddJsonBody("{ \"recipient\": \"polygon:" + wallet + "\", \"metadata\": \"" + url + "\" }");

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {

            }
        }
    }
}
