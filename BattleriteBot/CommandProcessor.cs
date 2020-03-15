using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Int32;

namespace BattleriteBot
{
    public class CommandProcessor
    {
        private readonly IMainController _mMainController;
        private readonly IBotWrapper _mBotWrapper;

        private const string Help = "/help";
        private const string Status = "/status";
        private const string Reset = "/reset";

        private const string Sign = "/sign";
        private const string Go = "/go";
        private const string Play = "/play";

        private const string Add = "/add";

        private const string AnyTeam = "/signmeup";

        private const string Twos = "2v2";
        private const string Threes = "3v3";
        private const string Any = "any";

        private const string Roll = "/roll";

        private const string Echo = "/say";
        private const string Backlog = "/backlog";

        private const char Separator = ' ';

        public CommandProcessor(IBotWrapper botWrapper, IMainController mainController)
        {
            _mBotWrapper = botWrapper;
            _mMainController = mainController;
        }

        public void ProcessCommand(MessageData data)
        {
            _mMainController.AddMessage(data.Chat);

            if (!data.Text.Any()) return;
            if (data.Text.First() != '/'  || data.Text.Length > 50) return;

            var lower = data.Text.ToLower();
            var splits = lower.Split(Separator);
            
            var signing = new Signing(data.Username);

            switch (splits.First())
            {
                case Add:
                    if (splits.Length > 1)
                    {
                        var newdata = new MessageData
                        {
                            Username = splits[1],
                            Text = splits.Skip(2).Aggregate(Go, (current, next) => current + " " + next),
                            Chat = data.Chat
                        };
                        ProcessCommand(newdata);
                    }
                    break;
                case Go:
                case Play:
                case Sign:
                    if (splits.Length < 2)
                    {
                        InvalidSign(data);
                        return;
                    }

                    List<DateTime?> times;
                    times = splits.Length > 2 ? ExtractTimeTag(splits) : new List<DateTime?> { null, null };

                    if (times==null) { 
                        InvalidSign(data);
                        return;
                    }

                    switch (splits[1])
                    {
                        case "2":
                        case Twos:
                            signing = signing.SetTwos(true, times);
                            break;
                        case "3":
                        case Threes:
                            signing = signing.SetThrees(true, times);
                            break;
                        case AnyTeam:
                        case Any:
                            signing = new Signing(data.Username, true, true, times[0], times[1]);
                            break;
                        default:
                            InvalidSign(data);
                            return;
                    }
                    var result = _mMainController.AddSigning(data.Chat, signing);
                    _mBotWrapper.PingParty(result, data.Chat);
                    break;
                case Status:
                    _mBotWrapper.PingParty(_mMainController.GetState(data.Chat), data.Chat);
                    break;
                case Reset:
                    _mMainController.Reset(data.Chat);
                    _mBotWrapper.SendMessage(data.Chat, "Party statuses reset.");
                    break;
                case Echo:
                    _mBotWrapper.SendMessage(data.Chat, GetSay(data.Text));
                    break;
                case Backlog:
                    _mBotWrapper.SendMessage(data.Chat, GetBacklog());
                    break;
                case Roll:
                    var amount = 1;
                    if (splits.Length == 2)
                    {
                        var success = TryParse(splits[1], out amount);
                        if (!success) amount = 1;
                    }
                    _mBotWrapper.SendMessage(
                        data.Chat, data.Username + RollTimes(amount));
                    break;
                case "/petallbunnies":
                case "/petbunny":
                    _mBotWrapper.SendMessage(data.Chat,
                        "\uD83D\uDC30 Pet the bunnies! \uD83D\uDC30");
                    break;
                case "/lolapua":
                case "/h":
                case Help:
                    _mBotWrapper.SendMessage(data.Chat,GetCommands());
                    return;
            }
        }

        private List<DateTime?> ExtractTimeTag(string[] splits)
        {
            DateTime? start = null, end = null;

            if (splits.Length > 2)
            {
                var thingA = splits[2].Split("-");
                var thingB = splits[2].Split(" ");
                var thing = thingA.Length > thingB.Length ? thingA : thingB;
                try { 
                    start = DateTime.ParseExact(thing[0], "H:mm", null, System.Globalization.DateTimeStyles.None);
                } catch { start = null; }

                try {
                    var endStr = "";
                    if (splits.Length > 3)
                        endStr = splits[3];
                    else
                        endStr = thing[1];

                    end = DateTime.ParseExact(endStr, "H:mm", null, System.Globalization.DateTimeStyles.None);
                } catch { end = null; }
            }
            return new List<DateTime?> { start, end };
        }

        private void InvalidSign(MessageData data)
        {
            _mBotWrapper.SendMessage(data.Chat,
                "/sign, /go, /play parameters: [Mode] [Start]-[End]\n"
                + "Where\n" 
                    + "  Valid Modes are: \n    " 
                        + Twos + ", " + Threes + ", " + Any + ", " + AnyTeam + "\n"
                    + "  (Optional) Start is: HH:MM\n"
                    + "  (Optional) End is: HH:MM\n"
                    + "  (Separate timestamps by space or '-')\n"
                + "\nOptional time tag is used to show when you're available to play."
                + "\n\nExample: /go any 18:00-21:30");
        }

        private static string GetSay(string cmd)
        {
            if (cmd.Length > 5)
                return cmd.Substring(5, cmd.Length - 5);
            return "Empty string provided.";
        }

        private static string RollTimes(int amount = 1)
        {
            var collector = " rolled:\n";
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < Math.Min(amount, 8); i++)
            {
                collector = collector + ( amount!=1 ? (i+1) + ": " : "" )+ random.Next(100) + "\n";
            }
            return collector;
        }

        private static string GetCommands()
        {
            return "List of available commands:\n"
                   + Help + ": Display all commands\n"
                   + Play + " & " + Go + " & " + Sign + ": Start a party\n"
                   + Reset + ": Reset all parties for this channel\n"
                   + Status + ": Display current party status\n"
                   + Roll + ": Roll 1-100. You can specify /roll N to roll N times.\n"
                   + Echo + ": Have me say things\n"
                   + Backlog + ": Display current backlog";
        }

        private static string GetBacklog()
        {
            return ""
                + "* Ping party at specified time\n"
                + "* Edit previous message instead of repost on every reply\n"
                + "* Discord support\n"
                + "* Purge party status if X time or messages have passed\n"
                + "* No players if all empty\n"
                + "* Easter eggs & backdoors :)\n";
        }
    }
}