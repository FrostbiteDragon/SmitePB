using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Manager
{
    public record Team
    {
        public readonly string name;
        public readonly string colour;

        public Team(string name, string colour)
        {
            this.name = name;
            this.colour = colour;
        }
    }
}
