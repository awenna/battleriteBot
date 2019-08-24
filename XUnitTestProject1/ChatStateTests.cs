using System;
using System.Linq;
using BattleriteBot;
using Shouldly;
using Xunit;

namespace XUnitTestProject1
{
    public class ChatStateTests
    {
        [Fact]
        public void TwosFull()
        {
            var state = new ChatState();
            var signing1 = new Signing("First", true);
            var signing2 = new Signing("Second", true);

            state = state.AddSigning(signing1).AddSigning(signing2);

            state.TwosFull().ShouldBe(true);
            state.ThreesFull().ShouldBe(false);
        }

        [Fact]
        public void ThreesFull()
        {
            var state = new ChatState();
            var signing1 = new Signing("First", false, true);
            var signing2 = new Signing("Second", false, true);
            var signing3 = new Signing("Third", false, true);

            state = state.AddSigning(signing1).AddSigning(signing2).AddSigning(signing3);

            state.ThreesFull().ShouldBe(true);
            state.TwosFull().ShouldBe(false);
        }

        [Fact]
        public void GetSuggestion()
        {
            var state = new ChatState();

            var signings = new []
            {
                new Signing("2", false, true),
                new Signing("3", false, true),
                new Signing("Any", false, true),
                new Signing("Any", false, true),
            };

            signings.Select(_ => state.AddSigning(_));

            state.GetSuggestion().Count().ShouldBe(3);
        }

        [Fact]
        public void SameName_UpdatesPrevious()
        {
            throw new NotImplementedException();
        }
    }
}
