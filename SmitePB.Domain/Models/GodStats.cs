using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record GodStats
    {
        public int PickBanRate { get; }
        public int WinRate { get; }
        public int GamesPlayed { get; }

        public GodStats(int pickBanRate, int winRate, int gamesPlayed)
        {
            PickBanRate = pickBanRate;
            WinRate = winRate;
            GamesPlayed = gamesPlayed;
        }
    }
}
