using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using Moq;
using NUnit.Framework;

namespace BridgeEdu.Engines.Play {
    public class NTDefenderStrategyTests {
        [Test]
        public void IsApplicable_ReturnsFalse_WhenNot1NT() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.Clubs));
            mockContext.Setup(c => c.IsAttackerTurn).Returns(true);
            var strategy = new NTDefenderStrategy();
            Assert.IsFalse(strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenNotDefenderTurn() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.IsAttackerTurn).Returns(true);
            var strategy = new NTDefenderStrategy();
            Assert.IsFalse(strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenPossibleCardsNotSameSuitAsLead() {
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.IsAttackerTurn).Returns(false);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.CurrentTrick.LeadSuit).Returns(Suit.Hearts);
            mockContext.Setup(c => c.PossibleCards).Returns(new List<Card> { new(Rank.Ace, Suit.Spades), new(Rank.King, Suit.Spades), new(Rank.Two, Suit.Diamonds) });

            var strategy = new NTDefenderStrategy();
            Assert.IsFalse(strategy.IsApplicable(mockContext.Object));
        }

        [Test, TestCaseSource(nameof(DefenderSecondTurnLowestCardTestCases))]
        public void GetSuggestions_ReturnsLowestCard_WhenFirstCardNotHonor(List<Card> possibleCards, Card firstCard, Card expectedCard) {
            var mockContext = new Mock<IPlayingContext>();
            var mockTrick = new Mock<ITrick>();
            mockTrick.Setup(t => t.Plays).Returns(new List<(Card, IPlayer)> { (firstCard, null) });
            mockContext.Setup(c => c.CurrentTrick).Returns(mockTrick.Object);
            mockContext.Setup(c => c.IsAttackerTurn).Returns(false);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);

            var strategy = new NTDefenderStrategy();
            var suggestions = strategy.GetSuggestions(mockContext.Object);

            Assert.IsNotEmpty(suggestions);
            Assert.AreEqual(expectedCard, suggestions[0].Card);
        }

        [Test, TestCaseSource(nameof(DefenderSecondTurnHonorOverHonorTestCases))]
        public void GetSuggestions_ReturnsBiggerHonor_WhenFirstCardIsHonor(List<Card> possibleCards, Card firstCard, Card expectedCard) {
            var mockContext = new Mock<IPlayingContext>();
            var mockTrick = new Mock<ITrick>();
            mockTrick.Setup(t => t.Plays).Returns(new List<(Card, IPlayer)> { (firstCard, null) });
            mockContext.Setup(c => c.CurrentTrick).Returns(mockTrick.Object);
            mockContext.Setup(c => c.IsAttackerTurn).Returns(false);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);

            var strategy = new NTDefenderStrategy();
            var suggestions = strategy.GetSuggestions(mockContext.Object);

            Assert.IsNotEmpty(suggestions);
            Assert.AreEqual(expectedCard, suggestions[0].Card);
        }

        [Test, TestCaseSource(nameof(DefenderFourthTurnLowestCardTestCases))]
        public void GetSuggestions_ReturnsLowestCard_WhenPartnerCardHighest(List<Card> possibleCards, Card firstCard, Card secondCard, Card thirdCard, Card expectedCard) {
            var mockContext = new Mock<IPlayingContext>();
            var mockTrick = new Mock<ITrick>();
            mockTrick.Setup(t => t.Plays).Returns(new List<(Card, IPlayer)> { (firstCard, null), (secondCard, null), (thirdCard, null) });
            mockTrick.Setup(t => t.LeadSuit).Returns(firstCard.Suit);
            mockContext.Setup(c => c.CurrentTrick).Returns(mockTrick.Object);
            mockContext.Setup(c => c.IsAttackerTurn).Returns(false);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(possibleCards);

            var strategy = new NTDefenderStrategy();
            var suggestions = strategy.GetSuggestions(mockContext.Object);

            Assert.IsNotEmpty(suggestions);
            Assert.AreEqual(expectedCard, suggestions[0].Card);
        }

        private static IEnumerable<TestCaseData> DefenderFourthTurnLowestCardTestCases() {
            yield return new TestCaseData(
                new List<Card> { new(Rank.Two, Suit.Diamonds), new(Rank.Four, Suit.Diamonds) },
                new Card(Rank.Nine, Suit.Diamonds),
                new Card(Rank.Jack, Suit.Diamonds),
                new Card(Rank.Five, Suit.Diamonds),
                new Card(Rank.Two, Suit.Diamonds)
            ).SetName("Returns 2 when partner card is highest and possible cards = 2, 4");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Seven, Suit.Clubs), new(Rank.Five, Suit.Clubs), new(Rank.Eight, Suit.Clubs) },
                new Card(Rank.Ten, Suit.Clubs),
                new Card(Rank.Queen, Suit.Clubs),
                new Card(Rank.Three, Suit.Clubs),
                new Card(Rank.Five, Suit.Clubs)
            ).SetName("Returns 5 when partner card is highest and possible cards = 5, 7, 8");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Ten, Suit.Spades), new(Rank.Three, Suit.Spades), new(Rank.Jack, Suit.Spades) },
                new Card(Rank.Ten, Suit.Spades),
                new Card(Rank.Four, Suit.Spades),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Three, Suit.Spades)
            ).SetName("Returns 3 when cant win and possible cards = 10, J, 3");

            yield return new TestCaseData(
                new List<Card> { new(Rank.King, Suit.Hearts), new(Rank.Two, Suit.Hearts), new(Rank.Three, Suit.Hearts) },
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Six, Suit.Hearts),
                new Card(Rank.Two, Suit.Hearts)
            ).SetName("Returns 2 when cant win and possible cards = K, 3, 2");
        }

        private static IEnumerable<TestCaseData> DefenderSecondTurnLowestCardTestCases() {
            yield return new TestCaseData(
                new List<Card> { new(Rank.Two, Suit.Diamonds), new(Rank.Four, Suit.Diamonds) },
                new Card(Rank.Nine, Suit.Diamonds),
                new Card(Rank.Two, Suit.Diamonds)
            ).SetName("Returns 2 when first card is 9 and possible cards = 2, 4");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Seven, Suit.Clubs), new(Rank.Five, Suit.Clubs), new(Rank.Eight, Suit.Clubs) },
                new Card(Rank.Ten, Suit.Clubs),
                new Card(Rank.Five, Suit.Clubs)
            ).SetName("Returns 5 when first card is 10 and possible cards = 5, 7, 8");
        }

        private static IEnumerable<TestCaseData> DefenderSecondTurnHonorOverHonorTestCases() {
            yield return new TestCaseData(
                new List<Card> { new(Rank.Ace, Suit.Spades), new(Rank.Queen, Suit.Spades), new(Rank.Jack, Suit.Spades) },
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Ace, Suit.Spades)
            ).SetName("Returns A when first card is K and possible cards = A, Q, J");

            yield return new TestCaseData(
                new List<Card> { new(Rank.Queen, Suit.Hearts), new(Rank.King, Suit.Hearts) },
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts)
            ).SetName("Returns Q when first card is J and possible cards = Q, K");
        }
    }
}