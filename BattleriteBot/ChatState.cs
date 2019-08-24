using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BattleriteBot
{
    public class ChatState
    {
        public int MessagesSinceLast { get; }
        public ImmutableList<Signing> Signings { get; }

        public ChatState()
        {
            MessagesSinceLast = 0;
            Signings = ImmutableList.Create<Signing>();
        }

        public ChatState(
            ImmutableList<Signing> signings,
            int msg = 0)
        {
            MessagesSinceLast = msg;
            Signings = signings;
        }

        public bool TwosFull()
        {
            return Signings.FindAll(_ => _.Twos).Count >= 2;
        }

        public bool ThreesFull()
        {
            return Signings.FindAll(_ => _.Threes).Count >= 3;
        }

        public IEnumerable<Signing> GetTwos()
        {
            return Signings.FindAll(_ => _.Twos);
        }

        public IEnumerable<Signing> GetThrees()
        {
            return Signings.FindAll(_ => _.Threes);
        }

        public ChatState AddSigning(Signing signing)
        {
            return new ChatState(Signings.Add(signing));
        }

        public IEnumerable<Signing> GetSuggestion()
        {
            if (ThreesFull()) return GetThrees();
            if (TwosFull()) return GetTwos();
            return new List<Signing>();
        }

        public override bool Equals(object obj)
        {
            try
            {
                var other = (ChatState) obj;
                return MessagesSinceLast == other.MessagesSinceLast
                       && Signings.SequenceEqual(other.Signings);
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
