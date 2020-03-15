using System;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace BattleriteBot
{
    public interface IBotWrapper
    {
        //void Ack(Chat chat);
        void PingParty(ChatState state, Chat chat);
        void SendMessage(Chat chat, string message);
        void Start(CommandProcessor pros, string key);
    }

    public class BotWrapper : IBotWrapper
    {
        static ITelegramBotClient _botClient;
        private CommandProcessor _processor;

        public void Start(CommandProcessor pros, string key)
        {
            _processor = pros;
            _botClient = new TelegramBotClient(key);

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        public async void SendMessage(Chat chat, string message)
        {
            if (message.Any())
            {
                var r = await _botClient.SendTextMessageAsync(chat, message);
                //r.
            }
        }

        public void UpdateMessage(Chat chat, string newMessage, int messageID)
        {
            throw new NotImplementedException();
        }

        public void PingParty(ChatState state, Chat chat)
        {
            var message = MessageBuilder.BuildMessage(state);

            SendMessage(chat, message);
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var data = new MessageData
                {
                    Text = e.Message.Text,
                    Chat = e.Message.Chat,
                    Username = e.Message.From.Username,
                };
                _processor.ProcessCommand(data);
            }
        }
    }
}
