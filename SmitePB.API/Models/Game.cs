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
        public string[] PickIds { get; }
        public string[] BanIds { get; }

        public Game(bool orderWon, string orderTeamName, string chaosTeamName, string[] pickIds, string[] banIds, string id = "")
        {
            Id = id;
            OrderWon = orderWon;
            OrderTeamName = orderTeamName;
            ChaosTeamName = chaosTeamName;
            PickIds = pickIds;
            BanIds = banIds;
        }
    }
}
