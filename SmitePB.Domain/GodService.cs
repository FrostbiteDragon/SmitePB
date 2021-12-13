using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public class GodService
    {
        public static IEnumerable<God> GetGods()
        {
            var godDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Gods"));

            foreach(var godDirectory in godDirectories)
            {
                yield return new God(
                    name: godDirectory.Split(Path.DirectorySeparatorChar).Last(),
                    pick: Path.Combine(godDirectory, "Pick.png"),
                    ban: Path.Combine(godDirectory, "Ban.png"),
                    lockInSound: Path.Combine(godDirectory, "LockIn.mp3"));
            }
        }
    }
}
