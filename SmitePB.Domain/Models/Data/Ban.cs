using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain.Models
{
    public record Ban
    {
        public string Id { get; } = "";
        public string Team { get; }
        public string Against { get; }
        public string God { get; }

        public Ban(string team, string against, string god)
        {
            Team = team;
            Against = against;
            God = god;
        }
    }
}
