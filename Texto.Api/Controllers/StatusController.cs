using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StatusController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("I'm doing okay");
        }

        [AllowAnonymous]
        [HttpGet, Route("say1")]
        public IActionResult Say()
        {
            return Ok($"{_configuration["Say1"]}");
        }

        [AllowAnonymous]
        [HttpGet, Route("say2")]
        public IActionResult Say2()
        {
            return Ok($"{_configuration["Words:Say2"]}");
        }
    }
}