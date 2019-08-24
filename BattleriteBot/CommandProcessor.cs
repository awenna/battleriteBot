using System;
using System.Linq;
using System.Threading.Tasks;
using BattleriteBot;
using Telegram.Bot;

namespace Awesome
{
    public class CommandProcessor
    {
        private readonly IMainController _mMainController;

        private const string Help = "/help";
        private const string Status = "/status";
        private const string Reset = "/reset";

        private const string Sign = "/sign";
        private const string AnyTeam = "/signmeup";
        private const string Echo = "/say";

        private const string Twos = "2";
        private const string Threes = "3";
        private const string Any = "any";

        private const char Separator = ' ';

        //private Hashtable Chats = new Hashtable();

        private readonly BotWrapper _mBotWrapper;

        public CommandProcessor(BotWrapper botWrapper, IMainController mainController)
        {
            _mBotWrapper = botWrapper;
            _mMainController = mainController;
        }

        public async Task ProcessCommand(ITelegramBotClient botClient, MessageData data)
        {
            if (data.Text.First() != '/' ) return;

            var lower = data.Text.ToLower();
            var splits = lower.Split(Separator);
            
            var signing = new Signing(data.Username);

            switch (splits.First())
            {
                case Sign:
                    if (splits.Length < 2)
                    {
                        await InvalidSign(botClient, data);
                        return;
                    }
                    switch (splits[1])
                    {
                        case Twos:
                            signing = signing.SetTwos(true);
                            break;
                        case Threes:
                            signing = signing.SetThrees(true);
                            break;
                        case Any:
                            signing = new Signing(data.Username, true, true);
                            break;
                        default:
                            await InvalidSign(botClient, data);
                            return;
                    }
                    break;
                case AnyTeam:
                    signing = new Signing(data.Username, true, true);
                    break;
                case Status:
                    _mBotWrapper.PingParty(_mMainController.GetState(data.Chat), data.Chat);
                    break;
                case Reset:
                    _mMainController.Reset(data.Chat);
                    await botClient.SendTextMessageAsync(
                        chatId: data.Chat,
                        text: "Party statuses reset.");
                    break;
                case Echo:
                    await botClient.SendTextMessageAsync(
                        chatId: data.Chat,
                        text: GetSay(data.Text));
                    break;
                default:
                    await botClient.SendTextMessageAsync(
                        chatId: data.Chat,
                        text: GetCommands());
                    return;
            }

            var result = _mMainController.AddSigning(data.Chat, signing);
            _mBotWrapper.PingParty(result, data.Chat);
        }

        private Task InvalidSign(ITelegramBotClient botClient, MessageData data)
        {
            return botClient.SendTextMessageAsync(
                chatId: data.Chat,
                text: "Invalid parameters for /sign. Valid ones are:\n" + Twos + "\n" + Threes + "\n" + Any + "\n" + AnyTeam);
        }

        private static string GetSay(string cmd)
        {
            return cmd.Substring(5, cmd.Length-5);
        }

        private static string GetCommands()
        {
            return "List of available commands:\n"
                + Help + ": Display this\n"
                + Sign + ": Start a party\n"
                + Reset + ": Reset all parties for this channel\n"
                + Echo + ": Have me say things";
        }
    }
}