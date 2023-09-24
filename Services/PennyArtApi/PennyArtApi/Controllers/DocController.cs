using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PennyArtApi.ExternalServices;
using PennyArtApi.Models.Pinata;
using PennyArtApi.Models.Responses;

namespace PennyArtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocController : ControllerBase
    {
        private readonly ILogger<DocController> _logger;
        private readonly IPinataClient _pinataClient;

        public DocController(ILogger<DocController> logger, IPinataClient pinataClient)
        {
            _logger = logger;
            _pinataClient = pinataClient;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<DocResponse>> Get(string userId)
        {
            var pResponse = await _pinataClient.SearchByUserId(userId);

            return pResponse;
        }

        [HttpPost("{userId}")]
        public async Task PostAsync([FromRoute] string userId, IFormFile doc)
        {
            MemoryStream ms = new();
            doc.CopyTo(ms);
            var pResponse = await _pinataClient.PinFileToIpfsAsync(ms, doc.FileName, userId);

            Ok(pResponse);
        }
    }
}
