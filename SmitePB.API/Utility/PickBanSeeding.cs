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
                    "AGNI",
                    "Cupid",
                    "Scylla",
                    "Janus",
                    "Camazotz",
                    "Bacchus",
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
            ),
            new GameResult(
                false,
                "Janitors",
                "Disaster",
                new string[]
                {
                    "Achilles",
                    "Athena",
                    "Cliodhna",
                    "AGNI",
                    "Cupid",
                    "Scylla",
                    "Janus",
                    "Camazotz",
                    "Bacchus",
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
            ),
             new GameResult(
                false,
                "ppp",
                "pingdom",
                new string[]
                {
                    "ZHONG KUI",
                    "ZEUS",
                    "YMIR",
                    "YEMOJA",
                    "XING TIAN",
                    "XBALANQUE",
                    "VULCAN",
                    "VAMANA",
                    "ULLR",
                    "TYR"
                },
                new string[]
                {
                    "TSUKUYOMI",
                    "TIAMAT",
                    "THOTH",
                    "THOR",
                    "THE MORRIGAN",
                    "THANATOS",
                    "TERRA",
                    "SYLVANUS",
                    "SUSANO",
                    "SUN WUKONG"
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
                session.Delete(game);
            }
            await session.SaveChangesAsync();

            foreach (var gameResult in gameResults)
                await PickBanService.SaveGamePBs(services, gameResult);

            await session.SaveChangesAsync();
        }
    }
}
