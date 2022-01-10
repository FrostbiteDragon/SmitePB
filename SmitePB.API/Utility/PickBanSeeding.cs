using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using SmitePB.API.Models;
using SmitePB.API.Services;
using SmitePB.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmitePB.API
{
    public static class PickBanSeeding
    {
        private static readonly GameResult[] gameResults = new GameResult[]
        {
            new GameResult(
                true, 
                "Janitors", 
                "Disaster", 
                new string[]
                {
                    "Achilles",
                    "Athena",
                    "Cliodhna",
                    "Agni",
                    "Cupid",
                    "Scylla",
                    "Janus",
                    "Camazots",
                    "Bachus",
                    "Hera"
                },
                new string[]
                {
                    "Ah Puch",
                    "Amaterasu",
                    "Anhur",
                    "Anubis",
                    "Ao Kuang",
                    "Aphrodite",
                    "Apollo",
                    "Arachne",
                    "Ares",
                    "Artemis"
                }
            )
        };

        public static async void SeedGames(this IApplicationBuilder builder)
        {
            var services = builder.ApplicationServices.CreateScope().ServiceProvider;
            var session = services.GetService<IAsyncDocumentSession>();

            var games = await session.Query<Game>().ToListAsync();

            foreach (var game in games)
            {
                foreach (var id in game.PickIds.Concat(game.BanIds))
                    session.Delete(id);

                session.Delete(game);
            }
            await session.SaveChangesAsync();

            foreach (var gameResult in gameResults)
                await PickBanService.SaveGamePBs(services, gameResult);

            await session.SaveChangesAsync();
        }
    }
}
