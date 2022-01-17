using System;
using System.Collections.Generic;
using System.Text;

namespace SmitePB.Domain
{
    public record God
    {
        public string Name { get; }
        public string Pick { get; }
        public string Ban { get; }
        public string LockInSound { get; }

        public God(string name, string pick, string ban, string lockInSound)
        {
            Name = name;
            Pick = pick;
            Ban = ban;
            LockInSound = lockInSound;
        }
    }

    public record GodDTO
    {
        public readonly string name;

        public GodDTO(string name)
        {
            this.name = name;
        }
    }
}
