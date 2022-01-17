using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.API.Models
{
    public class Pick
    {
        public string Id { get; set; }
        public string Team { get; }
        public string God { get; }
        public bool Win { get; }

        public Pick(string team, string god, bool win, string id = "")
        {
            Id = id;
            Team = team;
            God = god;
            Win = win;
        }
    }
}
