using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmitePB.API.Models
{
    public record PickBan
    {
        public string FriendlyTeamName { get; }
        public string EnemyTeamName { get; }
        public string God { get; }
        public bool Win { get; }

        public PickBan(string friendlyTeamName, string enemyTeamName, string god, bool win)
        {
            FriendlyTeamName = friendlyTeamName;
            EnemyTeamName = enemyTeamName;
            God = god;
            Win = win;
        }
    }
}
