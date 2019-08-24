using Telegram.Bot.Types;

namespace BattleriteBot
{
    public class MessageData
    {
        public string Text { get; set; }
        public Chat Chat { get; set;  }
        public string Username { get; set; }
    }
}
