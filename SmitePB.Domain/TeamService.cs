using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmitePB.Domain
{
    public static class TeamService
    {
        public static IEnumerable<Team> GetTeams()
        {
            var teamDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Teams"));

            foreach (var teamDirectory in teamDirectories)
            {
                var teamDTO = JsonConvert.DeserializeObject<TeamDTO>(File.ReadAllText(Path.Combine(teamDirectory, "Team.json")));
                yield return new Team(teamDTO.displayName, teamDTO.colour, Path.Combine(teamDirectory, "Logo.png"));
            }
        }
    }
}
