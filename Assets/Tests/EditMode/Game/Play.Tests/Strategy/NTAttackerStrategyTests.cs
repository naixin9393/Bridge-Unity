
using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using Moq;
using NUnit.Framework;

namespace BridgeEdu.Engines.Play {
    [TestFixture]
    public class NTAttackerStrategyTests {
        private IPlayingStrategy _strategy;

        [SetUp]
        public void SetUp() {
            _strategy = new NTAttackerStrategy();
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenNot1NT() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.Clubs));
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenNotAttackerTurn() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.IsAttackerTurn).Returns(false);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenOnlyOnePossibleCard() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.IsAttackerTurn).Returns(true);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(new List<Card> { new(Rank.Ace, Suit.Spades) });
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test, TestCaseSource(nameof(LowestEquivalentCardNTAttackerTestCases))]
        public void GetSuggestions_ReturnsLowestCard_WhenPossibleCardsAreEquivalent(List<Card> possibleCards, Card expectedCard) {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.IsAttackerTurn).Returns(true);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);

            var suggestions = _strategy.GetSuggestions(mockContext.Object);
            Assert.IsNotEmpty(suggestions);
            Assert.AreEqual(1, suggestions.Count);
            Assert.AreEqual(expectedCard, suggestions[0].Card);
        }

        private static IEnumerable<TestCaseData> LowestEquivalentCardNTAttackerTestCases() {
            yield return new TestCaseData(
                new List<Card> { new(Rank.Ace, Suit.Spades), new(Rank.King, Suit.Spades) },
                new Card(Rank.King, Suit.Spades)
            ).SetName("Returns K when A K they are equivalent");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Two, Suit.Hearts), new(Rank.Three, Suit.Hearts), new(Rank.Four, Suit.Hearts) },
                new Card(Rank.Two, Suit.Hearts)
            ).SetName("Returns 2 when 2, 3, 4 they are equivalent");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Five, Suit.Diamonds), new(Rank.Six, Suit.Diamonds) },
                new Card(Rank.Five, Suit.Diamonds)
            ).SetName("Returns 5 when 5, 6 they are equivalent");
        }
    }
}