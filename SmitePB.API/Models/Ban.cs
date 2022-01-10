using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record Ban
    {
        public string Id { get; }
        public string FriendlyTeamName { get; }
        public string EnemyTeamName { get; }
        public string God { get; }

        public Ban(string friendlyTeamName, string enemyTeamName, string god, string id = "")
        {
            Id = id;
            FriendlyTeamName = friendlyTeamName;
            EnemyTeamName = enemyTeamName;
            God = god;
        }
    }
}
