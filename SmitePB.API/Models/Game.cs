using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.API.Models
{
    public record Game
    {
        public string Id { get; }
        public bool OrderWon { get; }
        public string OrderTeamName { get; }
        public string ChaosTeamName { get; }
        public PickBan[] Picks { get; }
        public PickBan[] Bans { get; }

        public Game(bool orderWon, string orderTeamName, string chaosTeamName, PickBan[] picks, PickBan[] bans, string id = "")
        {
            Id = id;
            OrderWon = orderWon;
            OrderTeamName = orderTeamName;
            ChaosTeamName = chaosTeamName;
            Picks = picks;
            Bans = bans;
        }
    }
}
