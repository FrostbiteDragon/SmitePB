using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record Team
    {
        public string DisplayName { get; }
        public string Colour { get; }
        public string Logo { get; }
        public string[] Players { get; }

        public Team(string displayName, string colour, string logo, string[] players)
        {
            DisplayName = displayName;
            Colour = colour;
            Logo = logo;
            Players = players;
        }
    }

    public record TeamDTO
    {
        public readonly string displayName;
        public readonly string colour;
        public readonly string[] players;

        public TeamDTO(string displayName, string colour, string[] players)
        {
            this.displayName = displayName;
            this.colour = colour;
            this.players = players;
        }
    }
}
