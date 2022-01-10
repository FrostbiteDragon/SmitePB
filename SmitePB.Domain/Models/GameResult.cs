using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record GameResult
    {
        public bool OrderWon { get; }
        public string OrderTeamName { get; }
        public string ChaosTeamName { get; }
        public string[] Picks { get; }
        public string[] Bans { get; }

        public GameResult(bool orderWon, string orderTeamName, string chaosTeamName, string[] picks, string[] bans)
        {
            OrderWon = orderWon;
            OrderTeamName = orderTeamName;
            ChaosTeamName = chaosTeamName;
            Picks = picks;
            Bans = bans;
        }
    }
}
