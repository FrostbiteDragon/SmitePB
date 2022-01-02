using System;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using SmitePB.API.Models;
using SmitePB.Domain;
using static SmitePB.API.Services.RavenService;

namespace SmitePB.API.Services
{
    public static class PickBanService
    {
        public static Task<GodStats> GetGodStats(IServiceProvider services, string god) => AccessRaven(services, async session =>
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
                winRate: gamesPlayed == 0
                    ? 0
                    : wins * 100 / gamesPlayed,
                gamesPlayed: gamesPlayed
            );
        });

        public static Task SaveGamePBs(IServiceProvider services, GameResult gameResult) => AccessRaven(services, async session =>
        {
            var picks = 
                gameResult.Picks
                .Take(5)
                .Select(x => new Pick(gameResult.OrderTeamName, x, gameResult.OrderWon, Guid.NewGuid().ToString()))
                .Concat(
                    gameResult.Picks
                    .TakeLast(5)
                    .Select(x => new Pick(gameResult.ChaosTeamName, x, !gameResult.OrderWon, Guid.NewGuid().ToString()))
                ).ToArray();

            var bans =
               gameResult.Bans
               .Take(5)
               .Select(x => new Ban(gameResult.OrderTeamName, gameResult.ChaosTeamName, x, Guid.NewGuid().ToString()))
               .Concat(
                   gameResult.Bans
                   .TakeLast(5)
                   .Select(x => new Ban(gameResult.ChaosTeamName, gameResult.OrderTeamName, x, Guid.NewGuid().ToString()))
               ).ToArray();

            foreach (var pick in picks)
                await session.StoreAsync(pick, pick.Id);

            foreach (var ban in bans)
                await session.StoreAsync(ban, ban.Id);

            await session.StoreAsync(
                new Game(
                    gameResult.OrderWon,
                    gameResult.OrderTeamName,
                    gameResult.ChaosTeamName,
                    picks.Select(x => x.Id).ToArray(),
                    bans.Select(x => x.Id).ToArray()
                )
            );

            await session.SaveChangesAsync();
        });
    }
}
