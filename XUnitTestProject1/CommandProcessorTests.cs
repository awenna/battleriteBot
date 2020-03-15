using System.Diagnostics;
using BattleriteBot;
using NSubstitute;
using Telegram.Bot.Types;
using Xunit;

namespace BattleriteBotTests
{
    public class CommandProcessorTests
    {
        private readonly CommandProcessor target;

        private readonly IMainController _mainController;
        private readonly IBotWrapper _botWrapper;
        
        public CommandProcessorTests()
        {
            _mainController = Substitute.For<IMainController>();
            _botWrapper = Substitute.For<IBotWrapper>();
            target = new CommandProcessor(_botWrapper, _mainController);

            _mainController.AddSigning(
                Arg.Any<Chat>(),
                Arg.Any<Signing>())
                    .Returns(new ChatState());

        }

        [Fact]
        public void Twos_Proper_Processed()
        {
            var data = BuildMessageData("/sign 2");

            var expected = new Signing(data.Username, true, false);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected);
        }

        [Fact]
        public void Threes_Proper_Processed()
        {
            var data = BuildMessageData("/sign 3");

            var expected = new Signing(data.Username, false, true);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected);
        }

        [Fact]
        public void Any_Proper_Processed()
        {
            var data = BuildMessageData("/sign any");
            var data2 = BuildMessageData("/signmeup");

            var expected1 = new Signing(data.Username, true, true);
            var expected2 = new Signing(data.Username, true, true);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected1);

            target.ProcessCommand(data2);
            _mainController.Received().AddSigning(data2.Chat, expected2);
        }

        [Fact]
        public void Sign_TooShort_Error()
        {
            var data = BuildMessageData("/sign");
            var data2 = BuildMessageData("/play ");
            var data3 = BuildMessageData("/go\n");

            target.ProcessCommand(data);
            target.ProcessCommand(data2);
            target.ProcessCommand(data3);

            _mainController.DidNotReceiveWithAnyArgs();
        }

        [Fact]
        public void FaultyInputs_DoesNotCrash()
        {
            var data = new[]
            {
                BuildMessageData(""),
                BuildMessageData("/"),
                BuildMessageData("/say"),
                BuildMessageData("/say /asd"),
                BuildMessageData("/status"),
                BuildMessageData("/status "),
                BuildMessageData("/echo :D"),
                BuildMessageData("/say "),
                BuildMessageData("/say \n"),
                BuildMessageData("/reset asd"),
                BuildMessageData("/add add add asd"),
                BuildMessageData("/add ad aölsdkfjöadkljfökl löölllllllasdfasdfasdfasdf"
                + "asdssaaaafdlkgflögfjksdhgökdjshfgösjdöfkljgsdölkfgjösdkfjg"),
            };

            foreach (var messageData in data)
            {
                target.ProcessCommand(messageData);
            }
        }

        [Fact]
        public void AddOther_NoCrash()
        {
            var data = BuildMessageData("/add other 2");

            target.ProcessCommand(data);

            _mainController.Received()
                .AddSigning(Arg.Any<Chat>(), new Signing("other", true));
        } 

        [Fact]
        public void StartTag()
        {
            var now = Time.Now();
            var then = now.AddHours(2);
            
            var data = BuildMessageData("/sign 3 "+then.ToShortTimeString());

            var expected = new Signing(data.Username, false, true, then);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected);
        }

        [Fact]
        public void BothTags_Dash()
        {
            var now = Time.Now();
            var start = now.AddHours(2);
            var end = now.AddHours(4);

            var data = BuildMessageData("/sign 3 " 
                + start.ToShortTimeString()
                + "-"
                + end.ToShortTimeString());

            var expected = new Signing(
                data.Username, false, true, start, end);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected);
        }

        [Fact]
        public void BothTags_Space()
        {
            var now = Time.Now();
            var then = now.AddHours(2);
            var end = now.AddHours(4);

            var data = BuildMessageData("/sign 3 "
                + then.ToShortTimeString()
                + " "
                + end.ToShortTimeString());

            var expected = new Signing(data.Username, false, true, then);

            target.ProcessCommand(data);
            _mainController.Received().AddSigning(data.Chat, expected);
        }

        [Fact]
        public void BothTags_EndBeforeStart_Errors()
        {
            var now = Time.Now();
            var start = now.AddHours(2);
            var end = now.AddHours(4);

            var data = BuildMessageData("/sign 3 "
                + end.ToShortTimeString()
                + "-"
                + start.ToShortTimeString());

            var expected = new Signing(data.Username, false, true, end);

            target.ProcessCommand(data);
            _mainController.DidNotReceiveWithAnyArgs();
        }

        private static MessageData BuildMessageData(string text, string user="user")
        {
            return new MessageData
            {
                Username = user,
                Text = text,
                Chat = new Chat(),
            };
        }
    }
}
