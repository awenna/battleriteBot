using System.Linq;
using System;
using System.Collections.Generic;

namespace BattleriteBot
{
    public interface IMessageBuilder
    {
        
    }

    public static class MessageBuilder
    {
        private const string TwosReady = "2v2 Team ready!\n";
        private const string ThreesReady = "3v3 Team ready!\n";
        private const string CurrentStatus = "Current status:\n";

        public static string BuildMessage(ChatState state)
        {
            var message = "";
            string status;

            var forTwos = state.Signings.Where(_ => _.Twos);
            var forThrees = state.Signings.Where(_ => _.Threes);

            var threes = Intervals.FindOverlap(forThrees, 3);
            var twos = Intervals.FindOverlap(forTwos, 2);

            if (threes.Item1.Count > 2)
            {
                status = ThreesReady;
                message = message + BuildTimeString(threes.Item2, threes.Item3);
                message = message + BuildTeam(threes.Item1, true);
            }
            else if (twos.Item1.Count > 1)
            {
                status = TwosReady;
                message = message + BuildTimeString(twos.Item2, twos.Item3);
                message = message + BuildTeam(twos.Item1, true);
            }
            else
            {
                status = CurrentStatus;
                if (state.GetTwos().Any())
                    message = message + "2v2:\n" + BuildTeam(forTwos, false);
                if (state.GetThrees().Any())
                    message = message + "3v3:\n" + BuildTeam(forThrees, false);
            }

            return status + message;
        }

        private static string BuildTeam
            (IEnumerable<Signing> signings, bool ping)
        {
            if (signings.Any())
            {
                return signings.Select(
                    x => (
                        ping ? "  @" : "  ")
                        + x.Name
                        + (x.Start == null ?
                            " " + Time.Now().ToShortTimeString() :
                            " " + x.Start.Value.ToShortTimeString())
                        + (x.End == null ? " -->" : " - " + x.End.Value.ToShortTimeString())
                        + "\n")
                    .Aggregate((x, y) => x + y);
            }
            return "";
        }

        private static string BuildTimeString(DateTime? start, DateTime? end)
        {
            return "Party from: " + (start ?? Time.Now()).ToShortTimeString()
                + (end == null ? "-->" : " - " + end.Value.ToShortTimeString()) + "\n";
        }
    }
}
