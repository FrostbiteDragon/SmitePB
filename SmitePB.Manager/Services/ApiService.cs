using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmitePB.Domain;

namespace SmitePB.Manager.Services
{
    public class ApiService
    {
        private readonly string apiUrl;

        public ApiService(IConfiguration configuration)
        {
            apiUrl = configuration["ApiUrl"];
        }

        private async Task<TResult> AccessClient<TResult>(Func<HttpClient, Task<TResult>> func)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new(apiUrl),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));

            //must be awaited so that resources are dispossed properly
            var result = await func(client);
            return result;
        }
        private async Task AccessClient(Func<HttpClient, Task> func)
        {
            using var client = new HttpClient()
            {
                BaseAddress = new(apiUrl),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));

            //must be awaited so that resources are dispossed properly
            await func(client);
        }

        public Task<GodStats> GetStatsForGod(string godName) => AccessClient(async client =>
        {
            var response = await client.GetAsync($"stats/{godName}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<GodStats>();
            else throw new Exception(await response.Content.ReadAsStringAsync());
        });

        public Task<GodPBCount[]> GetTopPBforTeam(string team) => AccessClient(async client =>
        {
            var response = await client.GetAsync($"topPB/{team}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<GodPBCount[]>();
            else throw new Exception(await response.Content.ReadAsStringAsync());
        });

        public Task SaveGameResult(GameResult gameResult) => AccessClient(async client =>
        {
            await client.PostAsJsonAsync("result", gameResult);
        });
    }
}
