using System;
using System.Collections.Generic;

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

        public Signing SetTwos(bool newval, List<DateTime?> times = null)
        {
            return new Signing(Name, newval, Threes, 
                times == null ? Start : times[0], 
                times == null ? End : times[1]);
        }

        public Signing SetThrees(bool newval, List<DateTime?> times = null)
        {
            return new Signing(Name, Twos, newval, 
                times == null ? Start : times[0],
                times == null ? End : times[1]);
        }

        public override bool Equals(object obj)
        {
            try
            {
                var other = (Signing)obj;
                return Name == other.Name &&
                       Twos == other.Twos &&
                       Threes == other.Threes &&
                       Start == other.Start &&
                       End == other.End;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
