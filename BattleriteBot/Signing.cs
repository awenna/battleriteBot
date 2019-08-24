using System;

namespace BattleriteBot
{
    public class Signing
    {
        public string Name { get; }
        public bool Twos { get;  }
        public bool Threes { get; }
        public DateTime? Start { get; }
        public DateTime? End { get; }

        public Signing (string name,
            bool twos = false,
            bool threes = false,
            DateTime? start = null,
            DateTime? end = null)
        {
            Name = name;
            Twos = twos;
            Threes = threes;
            Start = start;
            End = end;
        }

        public Signing SetTwos(bool newval)
        {
            return new Signing(this.Name, newval, Threes, Start, End);
        }

        public Signing SetThrees(bool newval)
        {
            return new Signing(this.Name, Twos, newval, Start, End);
        }
    }
}
