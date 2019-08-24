using System;
using Awesome;
using BattleriteBot;
using NSubstitute;
using NSubstitute.Core.Arguments;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace XUnitTestProject1
{
    public class CommandProcessorTests
    {
        private readonly CommandProcessor target;

        private readonly ITelegramBotClient botClient;
        private readonly IMainController _mainController;
        
        public CommandProcessorTests()
        {
            _mainController = Substitute.For<IMainController>();
            target = new CommandProcessor(Substitute.For<BotWrapper>(), _mainController);
            botClient = Substitute.For<ITelegramBotClient>();
        }

        [Fact]
        public void Twos_Proper_Processed()
        {
            var data = new MessageData
            {
                Text = "/sign 2"
            };
            var r = target.ProcessCommand(botClient, data);
            _mainController.Received().AddSigning(null, Arg.Any<Signing>());
        }

        [Fact]
        public void Threes_Proper_Processed()
        {
            var data = new MessageData
            {
                Text = "/sign 3"
            };
            var r = target.ProcessCommand(botClient, data);
            r.Wait();
            _mainController.Received().AddSigning(null, Arg.Any<Signing>());
        }

        [Fact]
        public void Any_Proper_Processed()
        {
            var data = new MessageData { Text = "/sign any" };
            var data2 = new MessageData { Text = "/signmeup" };
            var r = target.ProcessCommand(botClient, data);
            r.Wait();
            _mainController.Received().AddSigning(null, Arg.Any<Signing>());
            r = target.ProcessCommand(botClient, data2);
            r.Wait();
            _mainController.Received().AddSigning(null, Arg.Any<Signing>());
        }

        [Fact]
        public void Sign_TooShort_Error()
        {
            var data = new MessageData { Text = "/sign", Chat = new Chat()};
            var data2 = new MessageData { Text = "/sign ", Chat = new Chat() };
            var data3 = new MessageData { Text = "/sign\n", Chat = new Chat() };

            var r = target.ProcessCommand(botClient, data);
            r.Wait();
            _mainController.DidNotReceiveWithAnyArgs();

            r = target.ProcessCommand(botClient, data2);
            r.Wait();
            _mainController.DidNotReceiveWithAnyArgs();

            r = target.ProcessCommand(botClient, data3);
            r.Wait();
            _mainController.DidNotReceiveWithAnyArgs();
        }
    }
}
