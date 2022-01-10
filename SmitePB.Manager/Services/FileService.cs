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
                yield return new Team(teamDTO.displayName, teamDTO.colour, Directory.EnumerateFiles(teamDirectory,  $"Logo.*").FirstOrDefault());
            }
        }

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
