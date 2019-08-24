using System;
using System.Linq;
using System.Threading;
using Awesome;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace BattleriteBot
{
    public class BotWrapper
    {
        static ITelegramBotClient botClient;
        private CommandProcessor processor;

        public void Start(CommandProcessor pros, string key)
        {
            processor = pros;
            botClient = new TelegramBotClient(key);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        public async void PingParty(ChatState state, Chat chat)
        {
            var message = "";
            var status = "";

            var sugg = state.GetSuggestion();

            if (sugg.Any())
            {
                
            }
            if (state.ThreesFull())
                status = "3v3 Team ready!\n";
            else if (state.TwosFull())
                status = "2v2 Team ready!\n";
            else status = "Current status:\n";

            if (state.GetTwos().Any())
                message = status + "\n2v2:\n"
                    + state.GetTwos().Select(x => x.Name + "\n")
                        .Aggregate((x, y) => x + y);

            if(state.GetThrees().Any())
                message = message + "\n3v3:\n"
                        + state.GetThrees().Select(x => x.Name + "\n")
                            .Aggregate((x, y) => x + y);
            
            await botClient.SendTextMessageAsync(
                chatId: chat,
                text: message);
        }

        public async void Ack(Chat chat)
        {
            await botClient.SendTextMessageAsync(
                chatId: chat,
                text: "Signing added!");
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var data = new MessageData
                {
                    Text = e.Message.Text,
                    Chat = e.Message.Chat,
                    Username = e.Message.From.Username,
                };
                await processor.ProcessCommand(botClient, data);
            }
        }
    }
}
