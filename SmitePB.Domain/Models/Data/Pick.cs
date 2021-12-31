using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record Pick
    {
        public string Id { get; } = "";
        public string Team { get; }
        public string God { get; }
        public bool Win { get; }

        public Pick(string team, string god, bool win)
        {
            Team = team;
            God = god;
            Win = win;
        }
    }
}
