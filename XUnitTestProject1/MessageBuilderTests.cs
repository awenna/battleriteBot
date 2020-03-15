using BattleriteBot;
using Xunit;

namespace BattleriteBotTests
{
    public class MessageBuilderTests
    {
        public MessageBuilderTests()
        {
        }

        [Fact]
        public void Somethign()
        {
            var state = new ChatState();
            MessageBuilder.BuildMessage(state);


        }
    }
}
