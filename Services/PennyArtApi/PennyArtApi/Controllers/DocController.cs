using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PennyArtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocController : ControllerBase
    {
        private readonly ILogger<DocController> _logger;

        public DocController(ILogger<DocController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IEnumerable<string> Get(string id)
        {
            return new List<string>
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };
        }
    }
}
