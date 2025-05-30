using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using Moq;
using NUnit.Framework;

namespace BridgeEdu.Engines.Play {
    public class NTDiscardStrategyTests {
        private NTDiscardStrategy _strategy;

        [SetUp]
        public void Setup() {
            _strategy = new NTDiscardStrategy();
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenNotNTContract() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.Spades));
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnFalse_WhenHasCardOfLeadingSuit() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.CurrentTrick.LeadSuit).Returns(Suit.Hearts);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(new List<Card> { new(Rank.Ace, Suit.Hearts) });
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnFalse_WhenFirstCardOfTrick() {
            var mockContext = new Mock<IPlayingContext>();
            var mockTrick = new Mock<ITrick>();
            Suit? mockSuit = null;
            mockTrick.Setup(t => t.LeadSuit).Returns(mockSuit);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.CurrentTrick).Returns(mockTrick.Object);
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsTrue_WhenNTContractWithoutLeadingSuitCards() {
            var mockContext = new Mock<IPlayingContext>();
            var possibleCards = new List<Card> { new(Rank.Ace, Suit.Diamonds), new(Rank.Four, Suit.Spades) };
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.CurrentTrick.LeadSuit).Returns(Suit.Clubs);
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);
            Assert.IsTrue(_strategy.IsApplicable(mockContext.Object));
        }

        [Test, TestCaseSource(nameof(DiscardLowestCardTestCases))]
        public void GetSuggestions_ReturnsLowestCard_WhenApplicable(List<Card> possibleCards, Card expectedCard) {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);
            mockContext.Setup(c => c.CurrentTrick.LeadSuit).Returns(Suit.Clubs);

            var suggestions = _strategy.GetSuggestions(mockContext.Object);

            Assert.AreEqual(1, suggestions.Count);
            Assert.AreEqual(expectedCard, suggestions[0].Card);
        }

        private static IEnumerable<TestCaseData> DiscardLowestCardTestCases() {
            yield return new TestCaseData(
                new List<Card> { new(Rank.Two, Suit.Diamonds), new(Rank.Four, Suit.Spades) },
                new Card(Rank.Two, Suit.Diamonds)
            ).SetName("Discard 2D when multiple hand is 2D 4S");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Five, Suit.Diamonds), new(Rank.Four, Suit.Hearts), new(Rank.Three, Suit.Spades) },
                new Card(Rank.Three, Suit.Spades)
            ).SetName("Discard 3C when multiple hand is 5D 4H 3S");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Ace, Suit.Diamonds), new(Rank.King, Suit.Spades) },
                new Card(Rank.King, Suit.Spades)
            ).SetName("Discard AD when multiple hand is AD KS");
        }
    }
}
