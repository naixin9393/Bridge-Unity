using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Game.Play;
using BridgeEdu.Game.Play.Exceptions;
using BridgeEdu.Game.Players;

namespace BridgeEdu.Game.Bidding.Tests {
    public class TrickTests {
        IPlayer northPlayer;
        IPlayer eastPlayer;
        IPlayer southPlayer;
        IPlayer westPlayer;

        [SetUp]
        public void SetUp() {
            northPlayer = CreateMockPlayer(Position.North).Object;
            eastPlayer = CreateMockPlayer(Position.East).Object;
            southPlayer = CreateMockPlayer(Position.South).Object;
            westPlayer = CreateMockPlayer(Position.West).Object;
        }

        [Test]
        public void Trick_ShouldBeEmpty_IfNoCardsWerePlayed() {
            var trick = new Trick(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, Strain.NoTrump, northPlayer);
            Assert.AreEqual(0, trick.Plays.Count);
        }

        [TestCase(Rank.Ace, Suit.Spades, Rank.Four, Suit.Spades, Rank.King, Suit.Hearts, Rank.Queen, Suit.Diamonds)]
        [TestCase(Rank.Five, Suit.Spades, Rank.Ten, Suit.Spades, Rank.King, Suit.Hearts, Rank.Queen, Suit.Diamonds)]
        public void Trick_ShouldContainCardsPlayed(Rank firstCardRank, Suit firstCardSuit, Rank secondCardRank, Suit secondCardSuit, Rank thirdCardRank, Suit thirdCardSuit, Rank fourthCardRank, Suit fourthCardSuit) {
            var trick = new Trick(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, Strain.NoTrump, northPlayer);
            trick.PlayCard(new Card(firstCardRank, firstCardSuit), northPlayer);
            trick.PlayCard(new Card(secondCardRank, secondCardSuit), eastPlayer);
            trick.PlayCard(new Card(thirdCardRank, thirdCardSuit), southPlayer);
            trick.PlayCard(new Card(fourthCardRank, fourthCardSuit), westPlayer);
            Assert.AreEqual(4, trick.Plays.Count);

            Assert.AreEqual(new Card(firstCardRank, firstCardSuit), trick.Plays[0].Card);
            Assert.AreEqual(new Card(secondCardRank, secondCardSuit), trick.Plays[1].Card);
            Assert.AreEqual(new Card(thirdCardRank, thirdCardSuit), trick.Plays[2].Card);
            Assert.AreEqual(new Card(fourthCardRank, fourthCardSuit), trick.Plays[3].Card);
        }

        [Test]
        public void Trick_Play_ShouldUpdateTurn() {
            Trick trick = new(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, Strain.NoTrump, northPlayer);
            trick.PlayCard(new Card(Rank.Two, Suit.Clubs), northPlayer);
            Assert.AreEqual(Position.East, trick.CurrentPlayer.Position);
            trick.PlayCard(new Card(Rank.Ace, Suit.Diamonds), eastPlayer);
            Assert.AreEqual(Position.South, trick.CurrentPlayer.Position);
            trick.PlayCard(new Card(Rank.Six, Suit.Hearts), southPlayer);
            Assert.AreEqual(Position.West, trick.CurrentPlayer.Position);
            trick.PlayCard(new Card(Rank.Four, Suit.Spades), westPlayer);
            Assert.AreEqual(Position.North, trick.CurrentPlayer.Position);
        }

        [Test]
        public void Trick_ShouldEnd_AfterFourCardsPlayed() {
            Trick trick = new(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, Strain.NoTrump, northPlayer);
            trick.PlayCard(new Card(Rank.Two, Suit.Clubs), northPlayer);
            trick.PlayCard(new Card(Rank.Ace, Suit.Diamonds), eastPlayer);
            trick.PlayCard(new Card(Rank.Six, Suit.Hearts), southPlayer);
            trick.PlayCard(new Card(Rank.Four, Suit.Spades), westPlayer);
            Assert.AreEqual(true, trick.IsOver);
        }

        [TestCase(Rank.Ace, Suit.Spades, Rank.Four, Suit.Spades, Rank.King, Suit.Hearts, Rank.Queen, Suit.Diamonds, Strain.NoTrump, Position.North)]
        [TestCase(Rank.Five, Suit.Spades, Rank.Ten, Suit.Spades, Rank.King, Suit.Hearts, Rank.Queen, Suit.Diamonds, Strain.NoTrump, Position.East)]
        [TestCase(Rank.Ace, Suit.Spades, Rank.Four, Suit.Hearts, Rank.King, Suit.Hearts, Rank.Queen, Suit.Diamonds, Strain.Hearts, Position.South)] // Trump is Hearts
        [TestCase(Rank.Five, Suit.Spades, Rank.Ten, Suit.Spades, Rank.King, Suit.Diamonds, Rank.Queen, Suit.Diamonds, Strain.Spades, Position.East)]  // Trump is Spades
        [TestCase(Rank.Ace, Suit.Spades, Rank.Ace, Suit.Hearts, Rank.Ace, Suit.Diamonds, Rank.Ace, Suit.Clubs, Strain.NoTrump, Position.North)] // All equal rank
        [TestCase(Rank.Two, Suit.Spades, Rank.Three, Suit.Spades, Rank.Four, Suit.Spades, Rank.Ace, Suit.Spades, Strain.NoTrump, Position.West)] // Lowest to Highest
        public void Trick_Winner_ShouldBeExpectedPlayer(Rank firstCardRank, Suit firstCardSuit, Rank secondCardRank, Suit secondCardSuit, Rank thirdCardRank, Suit thirdCardSuit, Rank fourthCardRank, Suit fourthCardSuit, Strain strain, Position expectedPosition) {
            var trick = new Trick(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, strain, northPlayer);
            trick.PlayCard(new Card(firstCardRank, firstCardSuit), northPlayer);
            trick.PlayCard(new Card(secondCardRank, secondCardSuit), eastPlayer);
            trick.PlayCard(new Card(thirdCardRank, thirdCardSuit), southPlayer);
            trick.PlayCard(new Card(fourthCardRank, fourthCardSuit), westPlayer);
            Assert.AreEqual(expectedPosition, trick.Winner.Position);
        }

        [Test]
        public void Trick_Play_ShouldThrowException_IfNotPlayersTurn() {
            var trick = new Trick(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer }, Strain.NoTrump, northPlayer);
            Assert.Throws<NotPlayersTurnException>(() => trick.PlayCard(new Card(Rank.Two, Suit.Clubs), eastPlayer));
        }

        private Mock<IPlayer> CreateMockPlayer(Position position) {
            var mockPlayer = new Mock<IPlayer>();
            mockPlayer.Setup(p => p.Position).Returns(position);
            //mockPlayer.Setup(p => p.PlayCard(It.IsAny<Card>()));
            return mockPlayer;
        }
    }
}