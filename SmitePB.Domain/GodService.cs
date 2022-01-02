using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public class GodService
    {
        public static God[] GetGods()
        {
            static IEnumerable<God> CreateGodList()
            {
                var godDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Gods"));

                foreach (var godDirectory in godDirectories)
                {
                    var name =
                        godDirectory
                        .Split(Path.DirectorySeparatorChar)
                        .Last()
                        .ToUpper();

                    yield return new God(
                        name: name,
                        pick: Path.Combine(godDirectory, "Pick.png"),
                        ban: Path.Combine(godDirectory, "Ban.png"),
                        lockInSound: Path.Combine(godDirectory, "LockIn.mp3")
                    );
                }
            }

            return CreateGodList().ToArray();
        }

        public static async Task<GodStats> GetStatsForGod(string godName)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new("https://localhost:5001/"),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));

            var response = await client.GetAsync($"stats/{godName}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<GodStats>();
            else throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public static async Task SaveGameResult(GameResult gameResult)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new("https://localhost:5001/"),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));

            await client.PostAsJsonAsync("result", gameResult);
        }
    }
}
