using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmitePB.API.Models;
using SmitePB.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using static SmitePB.API.Services.PickBanService;
using static SmitePB.API.Services.RavenService;

namespace SmitePB.API.Controllers
{
    [ApiController]
    public class PickBanController : ControllerBase
    {
        private IServiceProvider Services => HttpContext.RequestServices;

        [HttpPost("result")]
        public async Task<IActionResult> PostGameResult([FromBody] GameResult gameResult)
        {
            await SaveGamePBs(Services, gameResult);
            return Ok("Game saved stored.");
        }

        [HttpGet("stats/{god}")]
        public async Task<IActionResult> HttpGetGodStats(string god)
        {
            return Ok(await GetGodStats(Services, god));
        }

        [HttpGet("topPB/{team}")]
        public async Task<IActionResult> HttpGetTeamTopPB(string team)
        {
            var x = await GetTeamTopPBs(Services, team);
            return Ok(await GetTeamTopPBs(Services, team));
        }

        [HttpGet("topPB")]
        public async Task<IActionResult> HttpGetLeagueTopPB()
        {
            return Ok(await GetLeagueTopPBs(Services));
        }
    }
}
