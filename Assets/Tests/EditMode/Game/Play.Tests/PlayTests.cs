using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

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
        IPlay play = new Play(CreateMockAuction().Object);
        Assert.AreEqual(1, play.Tricks.Count);
    }

    [Test]
    public void Play_ShouldSetPlayersCorrectly_WhenCreated() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        IPlay play = new Play(mockAuction.Object);
        Assert.AreEqual(mockAuction.Object.Players, play.Players);
    }

    [Test]
    public void Play_ShouldDetermineLeadPlayer_WhenCreated() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);

        IPlay play = new Play(mockAuction.Object);
        Assert.AreEqual(westPlayer, play.LeadPlayer);
    }

    [Test]
    public void Play_CurrentPlayer_ShouldBeLeadPlayer_WhenCreated() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        Assert.AreEqual(play.CurrentPlayer, play.LeadPlayer);
    }

    [Test]
    public void Play_ShouldDetermineContract_WhenCreated() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        IPlay play = new Play(mockAuction.Object);
        Assert.AreEqual(new Bid(1, Strain.NoTrump), play.Contract);
    }

    [Test]
    public void Play_PlayCard_ShouldChangeCurrentPlayer() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
        Assert.AreEqual(northPlayer, play.CurrentPlayer);
    }

    [Test]
    public void Play_PlayCard_ShowThrowException_IfNotPlayersTurn() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        Assert.Throws<NotPlayersTurnException>(() => play.PlayCard(new Card(Rank.Ace, Suit.Clubs), northPlayer));
    }

    [Test]
    public void Play_PlayCard_ShouldAddCardToCurrentTrick() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
        Assert.AreEqual(1, play.CurrentTrick.Plays.Count);
    }

    [Test]
    public void Play_TrickCount_ShouldIncrease_WhenFourCardsPlayed() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        play.PlayCard(new Card(Rank.Ace, Suit.Clubs), westPlayer);
        play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
        play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
        play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
        Assert.AreEqual(2, play.Tricks.Count);
    }

    [Test]
    public void Play_CurrentPlayer_ShouldBeTrickWinner_WhenFourCardsPlayed() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        IPlay play = new Play(mockAuction.Object);
        play.PlayCard(new Card(Rank.Two, Suit.Clubs), westPlayer);
        play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
        play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
        play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
        Assert.AreEqual(northPlayer, play.CurrentPlayer);
    }

    [Test]
    public void Play_TricksWonByAttackers_ShouldBeIncreased_WhenTrickWinnerIsAttacker() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer); // Declarer and his partner are attackers (south and north)
        IPlay play = new Play(mockAuction.Object);
        play.PlayCard(new Card(Rank.Two, Suit.Clubs), westPlayer);
        play.PlayCard(new Card(Rank.King, Suit.Clubs), northPlayer);
        play.PlayCard(new Card(Rank.Queen, Suit.Clubs), eastPlayer);
        play.PlayCard(new Card(Rank.Jack, Suit.Clubs), southPlayer);
        Assert.AreEqual(1, play.TricksWonByAttackers);
    }

    [Test]
    public void Play_PlayShouldEnd_WhenLastTrickIsOver() {
        Mock<IAuction> mockAuction = CreateMockAuction();
        mockAuction.Setup(a => a.Declarer).Returns(southPlayer);
        
        var lastTrick = new Mock<ITrick>();
        lastTrick.SetupSequence(t => t.IsOver)
            .Returns(false)
            .Returns(false)
            .Returns(false)
            .Returns(true);
        var tricks = new List<ITrick>(new Trick[13]) {
            [12] = lastTrick.Object
        };
        var play = new Play(mockAuction.Object) {
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
        Mock<IAuction> mockAuction = CreateMockAuction();
        var play = new Play(mockAuction.Object) {
            _tricksWonByAttackers = 7
        };
        Assert.IsTrue(play.ContractIsMade);
    }

    private Mock<IPlayer> CreateMockPlayer(Position position) {
        var mockPlayer = new Mock<IPlayer>();
        mockPlayer.Setup(p => p.Position).Returns(position);
        mockPlayer.Setup(p => p.PlayCard(It.IsAny<Card>()));
        return mockPlayer;
    }

    private Mock<IAuction> CreateMockAuction() {
        var mockAuction = new Mock<IAuction>();
        mockAuction.Setup(a => a.Players).Returns(new List<IPlayer> { northPlayer, eastPlayer, southPlayer, westPlayer });
        mockAuction.Setup(a => a.FinalContract).Returns(new Bid(1, Strain.NoTrump));
        return mockAuction;
    }
}