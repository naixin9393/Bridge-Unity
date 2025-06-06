using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Players;
using BridgeEdu.Game.Play.Exceptions;

namespace BridgeEdu.Game.Play.Tests {
    public class PlayTests {
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
        public void Play_ShouldContainOneTricks_WhenCreated() {
            IPlay play = new PlayPhase(CreateMockAuction().Object);
            Assert.AreEqual(1, play.Tricks.Count);
        }

        [Test]
        public void Play_ShouldSetPlayersCorrectly_WhenCreated() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            IPlay play = new PlayPhase(mockAuction.Object);
            Assert.AreEqual(mockAuction.Object.Players, play.Players);
        }

        [Test]
        public void Play_ShouldDetermineLeadPlayer_WhenCreated() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);

            IPlay play = new PlayPhase(mockAuction.Object);
            Assert.AreEqual(westPlayer, play.LeadPlayer);
        }

        [Test]
        public void Play_CurrentPlayer_ShouldBeLeadPlayer_WhenCreated() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            IPlay play = new PlayPhase(mockAuction.Object);
            Assert.AreEqual(play.CurrentPlayer, play.LeadPlayer);
        }

        [Test]
        public void Play_ShouldDetermineContract_WhenCreated() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            IPlay play = new PlayPhase(mockAuction.Object);
            Assert.AreEqual(new Bid(1, Strain.NoTrump), play.Contract);
        }

        [Test]
        public void Play_PlayCard_ShouldChangeCurrentPlayer() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            IPlay play = new PlayPhase(mockAuction.Object);
            play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
            Assert.AreEqual(northPlayer, play.CurrentPlayer);
        }

        [Test]
        public void Play_PlayCard_ShowThrowException_IfNotPlayersTurn() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            IPlay play = new PlayPhase(mockAuction.Object);
            Assert.Throws<NotPlayersTurnException>(() => play.PlayCard(new Card(Rank.Ace, Suit.Clubs), northPlayer));
        }

        [Test]
        public void Play_PlayCard_ShouldAddCardToCurrentTrick() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            IPlay play = new PlayPhase(mockAuction.Object);
            play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
            Assert.AreEqual(1, play.CurrentTrick.Plays.Count);
        }

        [Test]
        public void Play_CurrentPlayer_ShouldBeTrickWinner_WhenFourCardsPlayed() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            IPlay play = new PlayPhase(mockAuction.Object);
            play.PlayCard(new Card(Rank.Two, Suit.Clubs), westPlayer);
            play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
            play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
            play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
            Assert.AreEqual(northPlayer, play.CurrentPlayer);
        }

        [Test]
        public void Play_PlayCard_ShouldThrowException_WhenHasCardsOfSameSuit_AndDoesNotFollowLead() {
            var mockAuction = new Mock<IBidding>();
            var playerWithSameSuit = new Mock<IPlayer>();
            var cards = new List<Card> { new(Rank.Three, Suit.Clubs), new(Rank.Four, Suit.Hearts) };
            var hand = new Hand();
            hand.AddCards(cards);
            playerWithSameSuit.Setup(p => p.Hand).Returns(hand);

            mockAuction.Setup(a => a.Players).Returns(new List<IPlayer> { playerWithSameSuit.Object, eastPlayer, southPlayer, westPlayer });
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            mockAuction.Setup(a => a.FinalContract).Returns(new Bid(1, Strain.NoTrump));

            IPlay play = new PlayPhase(mockAuction.Object);
            play.PlayCard(new Card(Rank.Two, Suit.Clubs), westPlayer);
            Assert.Throws<NotFollowingLeadException>(() => play.PlayCard(new Card(Rank.Four, Suit.Hearts), playerWithSameSuit.Object));
        }

        [Test]
        public void Play_TricksWonByAttackers_ShouldBeIncreased_WhenTrickWinnerIsAttacker() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer); // Declarer and his partner are attackers (south and north)
            IPlay play = new PlayPhase(mockAuction.Object);
            play.PlayCard(new Card(Rank.Two, Suit.Clubs), westPlayer);
            play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
            play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
            play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
            Assert.AreEqual(1, play.TricksWonByAttackers);
        }

        [Test]
        public void Play_PlayShouldEnd_WhenLastTrickIsOver() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);

            var lastTrick = new Mock<ITrick>();
            lastTrick.SetupSequence(t => t.IsOver)
                .Returns(false)
                .Returns(false)
                .Returns(false)
                .Returns(true);
            lastTrick.Setup(t => t.Plays).Returns(new List<(Card, IPlayer)> { });
            var tricks = new List<ITrick>(new Trick[13]) {
                [12] = lastTrick.Object
            };
            var play = new PlayPhase(mockAuction.Object) {
                _tricks = tricks,
                _currentTrickIndex = 12
            };

            Assert.IsFalse(play.IsOver);
            play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
            play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
            play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
            play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
            Assert.IsTrue(play.IsOver);
        }

        [Test]
        public void Play_ContractMade_ShouldBeTrue_WhenContractIsMade() {
            Mock<IBidding> mockAuction = CreateMockAuction();
            var play = new PlayPhase(mockAuction.Object) {
                _tricksWonByAttackers = 7
            };
            Assert.IsTrue(play.ContractIsMade);
        }


        private Mock<IPlayer> CreateMockPlayer(Position position) {
            var mockPlayer = new Mock<IPlayer>();
            mockPlayer.Setup(p => p.Position).Returns(position);
            return mockPlayer;
        }

        private Mock<IBidding> CreateMockAuction() {
            var mockAuction = new Mock<IBidding>();
            mockAuction.Setup(a => a.Players).Returns(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer });
            mockAuction.Setup(a => a.FinalContract).Returns(new Bid(1, Strain.NoTrump));
            mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
            return mockAuction;
        }
    }
}