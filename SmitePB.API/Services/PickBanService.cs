using SmitePB.Domain.Models;
using System;
using Raven.Client.Documents.Linq;
using static SmitePB.API.Services.RavenService;
using Raven.Client.Documents;
using System.Threading.Tasks;

namespace SmitePB.API.Services
{
    public static class PickBanService
    {
        public static async Task<GodStats> GetGodStats(IServiceProvider services, string god)
        {
            return await AccessRaven(services, async session =>
            {
                var globalGamesPlayed = 
                    await session
                    .Query<Pick>()
                    .CountAsync() / 10;
                var gamesPlayed = 
                    await session
                    .Query<Pick>()
                    .Where(x => x.God == god)
                    .CountAsync();
                var gamesBaned = 
                    await session
                    .Query<Ban>()
                    .Where(x => x.God == god)
                    .CountAsync();
                var wins =
                    await session
                    .Query<Pick>()
                    .Where(x => x.God == god)
                    .Where(x => x.Win == true)
                    .CountAsync();

                return new GodStats(
                    pickBanRate: (gamesPlayed + gamesBaned) * 100 / globalGamesPlayed,
                    winRate: wins * 100 / gamesPlayed,
                    gamesPlayed: gamesPlayed
                );
            });
        }
    }
}
