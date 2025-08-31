using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartController : ControllerBase
    {
        private readonly MlSearchService _ml;
        private readonly ILogger<SmartController> _logger;

        public SmartController(MlSearchService ml, ILogger<SmartController> logger)
        {
            _ml = ml;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            _logger.LogInformation("Received search query: {Query}", q);

            var result = await _ml.GetSmartSearchAsync(q);
            return Ok(result);
        }

        [HttpGet("similar/{id}")]
        public async Task<IActionResult> Similar(string id)
        {
            _logger.LogInformation("Received request for similar tasks, ID={TaskId}", id);

            var result = await _ml.GetSimilarAsync(id);
            return Ok(result);
        }
    }
}
