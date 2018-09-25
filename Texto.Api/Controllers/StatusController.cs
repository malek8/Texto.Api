using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("I'm doing okay");
        }
    }
}