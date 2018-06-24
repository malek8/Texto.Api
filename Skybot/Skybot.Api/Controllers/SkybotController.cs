using Microsoft.AspNetCore.Mvc;
using Skybot.Api.Services;

namespace Skybot.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SkybotController : ControllerBase
    {
        private readonly IRecognitionService recognitionService;

        public SkybotController(IRecognitionService recognitionService)
        {
            this.recognitionService = recognitionService;
        }

        [Route("process")]
        [HttpPost]
        public IActionResult Process([FromBody]dynamic message)
        {
            return Ok(recognitionService.Process(message.query.ToString()));
        }
    }
}
