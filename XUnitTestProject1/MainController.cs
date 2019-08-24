using System.Collections.Immutable;
using BattleriteBot;
using NSubstitute.Exceptions;
using Shouldly;
using Telegram.Bot.Types;
using Xunit;

namespace XUnitTestProject1
{
    public class MainController
    {
        private readonly BattleriteBot.MainController target = 
            new BattleriteBot.MainController();

        [Fact]
        public void AddOne_NoCrash()
        {
            var chat = new Chat();
            var signing1 = new Signing("Me :D", true, false);
            var signing2 = new Signing("Me :D", false, true);

            target.AddSigning(chat, signing1);
            target.AddSigning(chat, signing2);
        }

        [Fact]
        public void FillParty_Twos()
        {
            var chat = new Chat();
            var signing1 = new Signing("First", true, false);
            var signing2 = new Signing("Second", true, false);

            var expected = new ChatState(
                ImmutableList.Create(signing1, signing2));
            
            target.AddSigning(chat, signing1);
            target.AddSigning(chat, signing2).ShouldBe(expected);
        }
        /*
        [Fact]
        public void FillParty_Both()
        {
            var chat = new Chat();
            var signing1 = new Signing("First");
            var signing2 = new Signing("Second");
            var signing3 = new Signing("Third");
            var signing4 = new Signing("Fourth");

            var expected1 = new ChatState(
                ImmutableList.Create(signing1, signing2),
                ImmutableList.Create<Signing>());

            var expected2 = new ChatState(
                ImmutableList.Create<Signing>(),
                ImmutableList.Create<Signing>(signing2,signing3, signing4));

            target.AddSigning(chat, signing1, true, false);
            target.AddSigning(chat, signing2, true, true).ShouldBe(expected1);
            target.AddSigning(chat, signing3, false, true);
            target.AddSigning(chat, signing4, false, true).ShouldBe(expected2);
        }*/
    }
}
