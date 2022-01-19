using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmitePB.Domain;

namespace SmitePB.Manager.Services
{
    public class FileService
    {
        public static string AssetsFolder => Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public static string GetFile(string directory, string fileName) => Directory.EnumerateFiles(directory, $"{fileName}.*").FirstOrDefault();

        public static IEnumerable<Team> GetTeams()
        {
            var teamDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Teams"));

            foreach (var teamDirectory in teamDirectories)
            {
                var teamDTO = JsonConvert.DeserializeObject<TeamDTO>(File.ReadAllText(Path.Combine(teamDirectory, "Team.json")));

                yield return new Team(
                    teamDTO.displayName, 
                    teamDTO.colour, 
                    GetFile(teamDirectory, "Logo"),
                    teamDTO.players is not null ? teamDTO.players : new string[] {"Player", "Player", "Player", "Player", "Player"});
            }
        }

        public static God[] GetGods()
        {
            static IEnumerable<God> CreateGodList()
            {
                var godDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Gods"));

                foreach (var godDirectory in godDirectories)
                {
                    var godFile = Directory.EnumerateFiles(godDirectory, "*.json").FirstOrDefault();

                    var name = (godFile is null) switch
                    {
                        true => godDirectory
                            .Split(Path.DirectorySeparatorChar)
                            .Last()
                            .ToUpper(),
                        false => JsonConvert.DeserializeObject<GodDTO>(File.ReadAllText(godFile)).name
                    };
                        

                    yield return new God(
                        name: name,
                        pick: GetFile(godDirectory, "Pick"),
                        ban: GetFile(godDirectory, "Ban"),
                        lockInSound: GetFile(godDirectory, "LockIn")
                    );
                }
            }

            return CreateGodList().ToArray();
        }
    }
}
