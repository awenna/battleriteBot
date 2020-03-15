using System;
using System.Collections.Immutable;
using System.Linq;
using BattleriteBot;
using Shouldly;
using Xunit;

namespace BattleriteBotTests
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

            var signings = new[]
            {
                new Signing("First", false, true),
                new Signing("Second", false, true),
                new Signing("Third", false, true)
            };

            foreach (var signing in signings)
            {
                state = state.AddSigning(signing);
            }
            state.ThreesFull().ShouldBe(true);
            state.TwosFull().ShouldBe(false);
        }

        [Fact]
        public void GetSuggestion_Threes()
        {
            var state = new ChatState();

            var signings = new []
            {
                new Signing("2", true, false),
                new Signing("3", false, true),
                new Signing("Any", true, true),
                new Signing("Any", true, true),
            };

            foreach (var signing in signings)
            {
                state = state.AddSigning(signing);
            }

            state.GetSuggestion().Count().ShouldBe(3);
        }

        [Fact]
        public void GetSuggestion_Twos()
        {
            var state = new ChatState();

            var signings = new[]
            {
                new Signing("2", true, false),
                new Signing("3", false, true),
                new Signing("Any", true, true),
            };

            foreach (var signing in signings)
            {
                state = state.AddSigning(signing);
            }

            state.GetSuggestion().Count().ShouldBe(2);
        }

        [Fact]
        public void TimeStamping_NoFittingTimes_DoesNotReturnFullTeam()
        {
            var state = new ChatState();

            var now = DateTime.UtcNow;

            var aStart = now;
            var aEnd = now.AddHours(1);

            var bStart = now.AddHours(1);
            var bEnd = now.AddHours(3);

            var cStart = now.AddHours(3);

            // ---A--
            // BBB---
            // -CCCCC

            var signings = new[]
            {
                new Signing("A", true, false, aStart, aEnd),
                new Signing("B", true, false, bStart, bEnd),
                new Signing("C", true, false, cStart),
            };

            foreach (var signing in signings)
            {
                state = state.AddSigning(signing);
            }

            state.TwosFull().ShouldBe(false);
            state.GetSuggestion().Count().ShouldBe(0);
        }

        [Fact]
        public void TimeStamping()
        {
            var state = new ChatState();

            var now = DateTime.UtcNow;

            var aStart = now.AddHours(3);
            var aEnd = now.AddHours(4);

            var bStart = now;
            var bEnd = now.AddHours(3);

            var cStart = now.AddHours(1);

            // ---A--
            // BBB---
            // -CCCCC

            var signings = new[]
            {
                new Signing("A", true, false, aStart, aEnd),
                new Signing("B", true, false, bStart, bEnd),
                new Signing("C", true, false, cStart),
            };

            foreach (var signing in signings)
            {
                state = state.AddSigning(signing);
            }

            state.GetTwos().SequenceEqual(
                ImmutableList.Create(signings[1], signings[2]));
        }

        [Fact]
        public void SameName_UpdatesPrevious()
        {
            throw new NotImplementedException();
        }
    }
}
