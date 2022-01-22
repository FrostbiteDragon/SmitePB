using System;
using System.Collections.Generic;
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
            god = god.ToUpper();

            var globalGamesPlayed =
                await session
                .Query<Game>()
                .CountAsync();
            var gamesBaned =
                await session
                .Query<Game>()
                .Where(x => x.Bans.Any(x => x.God == god))
                .CountAsync();

            var picks =
                await session
                .Query<Game>()
                .Select(x => new { x.Picks })
                .ToArrayAsync();

            var gamesPlayed =
                picks
                .Where(x => x.Picks.Any(x => x.God == god))
                .Count();

            var wins =
                picks
                .Where(x => x.Picks.Any(x => x.Win && x.God == god))
                .Count();

            return new GodStats(
                pickBanRate: globalGamesPlayed == 0 
                ? 0
                : (gamesPlayed + gamesBaned) * 100 / globalGamesPlayed,
                winRate: 
                    gamesPlayed == 0 
                    ? 0 
                    : wins * 100 / gamesPlayed,
                gamesPlayed: gamesPlayed
            );
        });

        public static Task<GodPBCount[]> GetTeamTopPBs(IServiceProvider services, string team) => AccessRaven(services, async session =>
        {
            var totalGamesTeamPlayed =
                await session
                .Query<Game>()
                .Where(x => x.OrderTeamName == team || x.ChaosTeamName == team)
                .CountAsync();

            var pickBans =
                await session
                .Query<Game>()
                .Where(x => x.OrderTeamName == team || x.ChaosTeamName == team)
                .Select(x => new { x.Picks, x.Bans })
                .ToArrayAsync();

            var picks =
                pickBans
                .SelectMany(x => x.Picks.Where(x => x.FriendlyTeamName.ToUpper() == team.ToUpper()))
                .Select(x => x.God)
                .ToArray();

            var bansAgainst =
                 pickBans
                .SelectMany(x => x.Bans.Where(x => x.EnemyTeamName.ToUpper() == team.ToUpper()))
                .Select(x => x.God)
                .ToArray();

            return 
                picks
                .Concat(bansAgainst)
                .GroupBy(x => x)
                .Select(x => new GodPBCount(x.Key, x.Count() * 100 / totalGamesTeamPlayed))
                .OrderByDescending(x => x.Count)
                .ToArray();
        });

        public static Task<GodPBCount[]> GetLeagueTopPBs(IServiceProvider services) => AccessRaven(services, async session =>
        {
            var totalGames =
                 await session
                 .Query<Game>()
                 .CountAsync();

            return
                (
                    await session
                    .Query<Game>()
                    .Select(x => new { x.Picks, x.Bans })
                    .ToArrayAsync()
                )
                .SelectMany(x => x.Bans.Concat(x.Picks).ToArray().Select(x => x.God))
                .GroupBy(x => x)
                .Select(x => new GodPBCount(x.Key, x.Count() * 100 / totalGames))
                .OrderByDescending(x => x.Count)
                .ToArray();

        });

        public static Task SaveGamePBs(IServiceProvider services, GameResult gameResult) => AccessRaven(services, async session =>
        {
            var picks =
                gameResult.Picks
                .Take(5)
                .Select(x => new PickBan(gameResult.OrderTeamName, gameResult.ChaosTeamName, x, gameResult.OrderWon))
                .Concat(
                    gameResult.Picks
                    .TakeLast(5)
                    .Select(x => new PickBan(gameResult.ChaosTeamName, gameResult.OrderTeamName, x, !gameResult.OrderWon))
                    .ToArray()
                ).ToArray();

            var bans =
                gameResult.Bans
                .Take(5)
                .Select(x => new PickBan(gameResult.OrderTeamName, gameResult.ChaosTeamName, x, gameResult.OrderWon))
                .Concat(
                    gameResult.Bans
                    .TakeLast(5)
                    .Select(x => new PickBan(gameResult.ChaosTeamName, gameResult.OrderTeamName, x, !gameResult.OrderWon))
                    .ToArray()
                ).ToArray();

            var game = new Game(gameResult.OrderWon, gameResult.OrderTeamName, gameResult.ChaosTeamName, picks, bans);
            await session.StoreAsync(game);
            await session.SaveChangesAsync();
        });
    }

}
