using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmitePB.Domain;
using System;
using System.Threading.Tasks;
using static SmitePB.API.Services.PickBanService;
using static SmitePB.API.Services.RavenService;

namespace SmitePB.API.Controllers
{
    [ApiController]
    public class PickBanController : ControllerBase
    {
        private IServiceProvider Services => HttpContext.RequestServices;

        [HttpPost("pick")]
        public async Task<IActionResult> PostBan([FromBody] Pick pick)
        {
            await Store(Services, pick);
            return Ok("Pick stored.");
        }

        [HttpPost("ban")]
        public async Task<IActionResult> PostPick([FromBody] Ban ban)
        {
            await Store(Services, ban);
            return Ok("Ban stored.");
        }

        [HttpGet("stats/{god}")]
        public async Task<IActionResult> HttpGetGodStats(string god)
        {
            return Ok(await GetGodStats(Services, god));
        }
    }
}
