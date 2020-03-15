using BattleriteBot;
using Xunit;
using System.Collections.Immutable;
using Shouldly;
using System.Linq;

namespace BattleriteBotTests
{
    public class IntervalsTests
    {
        [Fact]
        public void TwoNullable_Works()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("A", false, false,
                    null, null),
                new Signing("B", false, false,
                    now.AddHours(1),
                    null)
            );

            var expected = ImmutableList.Create(signings[0], signings[1]);

            var result = Intervals.FindOverlap(signings, 2);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(now.AddHours(1));
            result.Item3.ShouldBe(null);
        }

        [Fact]
        public void TwoOverlapping_ShouldReturnOverlap()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("A", false, false, 
                    now,
                    now.AddHours(2)),
                new Signing("B", false, false,
                    now.AddHours(1),
                    now.AddHours(4))
            );

            var expected = ImmutableList.Create(signings[0], signings[1]);

            var result = Intervals.FindOverlap(signings, 2);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(now.AddHours(1));
            result.Item3.ShouldBe(now.AddHours(2));
        }

        [Fact]
        public void NoOverlap_ShouldReturnEmpty()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("A", false, false,
                    now,
                    now.AddHours(2)),
                new Signing("B", false, false,
                    now.AddHours(3),
                    now.AddHours(4))
            );

            var expected = ImmutableList.Create<Signing>();

            var result = Intervals.FindOverlap(signings, 2);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(null);
            result.Item3.ShouldBe(null);
        }

        [Fact]
        public void TooSmallOverlap_ShouldReturnEmpty()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("A", false, false,
                    now,
                    now.AddHours(1)),
                new Signing("B", false, false,
                    now.AddMinutes(15),
                    now.AddMinutes(30))
            );

            var expected = ImmutableList.Create<Signing>();

            var result = Intervals.FindOverlap(signings, 2);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(null);
            result.Item3.ShouldBe(null);
        }

        [Fact]
        public void ComplexCase_TwoOverlapping_ReturnsCorrectInterval()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("1", false, false,
                    now,
                    now.AddHours(1)),
                new Signing("A", false, false,
                    now.AddHours(1),
                    now.AddHours(6)),
                new Signing("2", false, false,
                    now,
                    now.AddMinutes(25)),
                new Signing("B", false, false,
                    now.AddHours(2),
                    now.AddHours(4)),
                new Signing("3", false, false,
                    now.AddMinutes(45),
                    now.AddMinutes(60 + 25)),
                new Signing("4", false, false,
                    now.AddHours(4),
                    now.AddHours(7)),
                new Signing("C", false, false,
                    now.AddHours(3),
                    now.AddHours(5)),
                new Signing("5", false, false,
                    now.AddHours(5),
                    now.AddHours(7))
            );

            var expected = ImmutableList.Create(signings[1], signings[3]);

            var result = Intervals.FindOverlap(signings, 2);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(now.AddHours(2));
            result.Item3.ShouldBe(now.AddHours(4));
        }

        [Fact]
        public void ComplexCase_ThreeOverlapping_ReturnsCorrectInterval()
        {
            var now = Time.Now();

            var signings = ImmutableList.Create(
                new Signing("1", false, false,
                    now,
                    now.AddHours(1)),
                new Signing("A", false, false,
                    now.AddHours(1),
                    now.AddHours(6)),
                new Signing("2", false, false,
                    now,
                    now.AddMinutes(25)),
                new Signing("B", false, false,
                    now.AddHours(2),
                    now.AddHours(4)),
                new Signing("3", false, false,
                    now.AddMinutes(45),
                    now.AddMinutes(60 + 25)),
                new Signing("4", false, false,
                    now.AddHours(4),
                    now.AddHours(7)),
                new Signing("C", false, false,
                    now.AddHours(3),
                    now.AddHours(5)),
                new Signing("5", false, false,
                    now.AddHours(5),
                    now.AddHours(7))
            );

            var expected = ImmutableList.Create(signings[1], signings[3], signings[6]);

            var result = Intervals.FindOverlap(signings, 3);
            result.Item1.SequenceEqual(expected).ShouldBe(true);
            result.Item2.ShouldBe(now.AddHours(3));
            result.Item3.ShouldBe(now.AddHours(4));
        }
    }
}
