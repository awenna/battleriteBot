using System.Linq;
using BattleriteBot;
using Telegram.Bot;

namespace Awesome
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any()) return;
            var com = new BotWrapper();
            var handler = new MainController();
            var comPros = new CommandProcessor(com, handler);
            com.Start(comPros, args[0]);
        }
    }
}