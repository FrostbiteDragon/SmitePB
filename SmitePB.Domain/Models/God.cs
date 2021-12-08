using System;
using System.Collections.Generic;
using System.Text;

namespace SmitePB.Domain
{
    public record God
    {
        public string name;
        public string Pick { get; }
        public string Ban { get; }

        public God(string name, string pick, string ban)
        {
            this.name = name;
            Pick = pick;
            Ban = ban;
        }
    }
}
