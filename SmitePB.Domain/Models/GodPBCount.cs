using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmitePB.Domain
{
    public record GodPBCount
    {
        public string God { get; init; }
        public int Count { get; init; }

        public GodPBCount(string god, int count)
        {
            God = god;
            Count = count;
        }
    }
}
