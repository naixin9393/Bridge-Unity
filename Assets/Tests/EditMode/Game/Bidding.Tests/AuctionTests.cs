using System.Collections.Generic;
using NUnit.Framework;

public class AuctionTests {
    private List<IPlayer> CreateTestPlayers() => new() {
        new Player(Position.North),
        new Player(Position.East),
        new Player(Position.South),
        new Player(Position.West)
    };

    private List<List<ICall>> CreateValidCalls(List<IPlayer> players, int dealerIndex) {
        return new List<List<ICall>>() {
            new() {
                new BidCall(new Bid(1, Strain.Clubs), players[dealerIndex++ % 4]),
                new BidCall(new Bid(2, Strain.Clubs), players[dealerIndex++ % 4]),
                new BidCall(new Bid(3, Strain.Clubs), players[dealerIndex++ % 4]),
                new BidCall(new Bid(4, Strain.Clubs), players[dealerIndex++ % 4]),
            },
            new() {
                new Pass(players[dealerIndex++ % 4]),
                new BidCall(new Bid(1, Strain.NoTrump), players[dealerIndex++ % 4]),
                new BidCall(new Bid(2, Strain.Clubs), players[dealerIndex++ % 4]),
                new BidCall(new Bid(4, Strain.Clubs), players[dealerIndex++ % 4]),
            },
            new() {
                new BidCall(new Bid(1, Strain.NoTrump), players[dealerIndex++ % 4]),
                new Pass(players[dealerIndex++ % 4]),
                new BidCall(new Bid(3, Strain.Clubs), players[dealerIndex++ % 4]),
                new Double(players[dealerIndex++ % 4])
            },
            new() {
                new BidCall(new Bid(1, Strain.NoTrump), players[dealerIndex++ % 4]),
                new Double(players[dealerIndex++ % 4]),
                new Redouble(players[dealerIndex++ % 4]),
                new Pass(players[dealerIndex++ % 4])
            },
            new() {
                new BidCall(new Bid(1, Strain.NoTrump), players[dealerIndex++ % 4]),
                new Pass(players[dealerIndex++ % 4]),
                new Pass(players[dealerIndex++ % 4]),
                new Pass(players[dealerIndex++ % 4])
            }
        };
    }


    [TestCase(0, TestName = "Dealer_ShouldBe_NorthPlayer")]
    [TestCase(1, TestName = "Dealer_ShouldBe_EastPlayer")]
    [TestCase(2, TestName = "Dealer_ShouldBe_SouthPlayer")]
    [TestCase(3, TestName = "Dealer_ShouldBe_WestPlayer")]
    public void Dealer_ShouldBe_ExpectedPlayer(int playerIndex) {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[playerIndex]);
        Assert.AreEqual(players[playerIndex], auction.Dealer);
    }

    [TestCase(0, CallType.Bid, Position.East, TestName = "CurrentPlayer_ShouldBe_EastPlayer_WhenDealerIsNorth_And_Bids")]
    [TestCase(1, CallType.Bid, Position.South, TestName = "CurrentPlayer_ShouldBe_SouthPlayer_WhenDealerIsEast_And_Bids")]
    [TestCase(2, CallType.Bid, Position.West, TestName = "CurrentPlayer_ShouldBe_WestPlayer_WhenDealerIsSouth_And_Bids")]
    [TestCase(3, CallType.Bid, Position.North, TestName = "CurrentPlayer_ShouldBe_NorthPlayer_WhenDealerIsWest_And_Bids")]
    [TestCase(0, CallType.Pass, Position.East, TestName = "CurrentPlayer_ShouldBe_EastPlayer_WhenDealerIsNorth_And_Bids")]
    [TestCase(1, CallType.Pass, Position.South, TestName = "CurrentPlayer_ShouldBe_SouthPlayer_WhenDealerIsEast_And_Bids")]
    [TestCase(2, CallType.Pass, Position.West, TestName = "CurrentPlayer_ShouldBe_WestPlayer_WhenDealerIsSouth_And_Passes")]
    [TestCase(3, CallType.Pass, Position.North, TestName = "CurrentPlayer_ShouldBe_NorthPlayer_WhenDealerIsWest_And_Passes")]
    public void CurrentPlayer_ShouldBe_ExpectedPlayer_WhenDealerIs(int playerIndex, CallType callType, Position expectedPosition) {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[playerIndex]);

        switch (callType) {
            case CallType.Bid:
                auction.MakeCall(new BidCall(new Bid(1, Strain.Clubs), players[playerIndex]));
                break;
            case CallType.Pass:
                auction.MakeCall(new Pass(players[playerIndex]));
                break;
        }
        Assert.AreEqual(expectedPosition, auction.CurrentPlayer.Position);
    }

    [TestCase(0, 0, TestName = "ShouldUpdateAuctionState_Bid-Bid-Bid-Bid")]
    [TestCase(2, 1, TestName = "ShouldUpdateAuctionState_Pass-Bid-Bid-Bid")]
    [TestCase(1, 2, TestName = "ShouldUpdateAuctionState_Bid-Pass-Bid-Double")]
    [TestCase(3, 3, TestName = "ShouldUpdateAuctionState_Bid-Double-Redouble-Pass")]
    [TestCase(3, 4, TestName = "ShouldUpdateAuctionState_Bid-Pass-Pass-Pass")]
    public void Auction_MakeCall_ShouldUpdateAuctionState_WhenCallerMakesValidCall(int playerIndex, int callsIndex) {
        var players = CreateTestPlayers();
        var validCalls = CreateValidCalls(players, playerIndex);
        var auction = new Auction(players, players[playerIndex]);

        ICall validCall = validCalls[callsIndex][0];
        auction.MakeCall(validCall);
        playerIndex = (playerIndex + 1) % 4;

        Assert.AreEqual(validCall, auction.LastCall);
        Assert.AreEqual(players[playerIndex++ % 4], auction.CurrentPlayer);

        validCall = validCalls[callsIndex][1];
        auction.MakeCall(validCall);

        Assert.AreEqual(validCall, auction.LastCall);
        Assert.AreEqual(players[playerIndex++ % 4], auction.CurrentPlayer);

        validCall = validCalls[callsIndex][2];
        auction.MakeCall(validCall);

        Assert.AreEqual(validCall, auction.LastCall);
        Assert.AreEqual(players[playerIndex++ % 4], auction.CurrentPlayer);

        validCall = validCalls[callsIndex][3];
        auction.MakeCall(validCall);

        Assert.AreEqual(validCall, auction.LastCall);
        Assert.AreEqual(players[playerIndex++ % 4], auction.CurrentPlayer);
    }


    [TestCase(0, CallType.Redouble, TestName = "ShouldThrow_InvalidCallException_WhenDealerMakesRedoubleCall")]
    [TestCase(0, CallType.Double, TestName = "ShouldThrow_InvalidCallException_WhenDealerMakesDoubleCall")]
    public void Auction_MakeCall_ShouldThrowException_WhenDealerMakesInvalidCall(int playerIndex, CallType callType) {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[playerIndex]);

        switch (callType) {
            case CallType.Double:
                Assert.Throws<InvalidCallException>(() => auction.MakeCall(new Double(players[playerIndex])));
                break;
            case CallType.Redouble:
                Assert.Throws<InvalidCallException>(() => auction.MakeCall(new Redouble(players[playerIndex])));
                break;
        }
    }

    [Test]
    public void Auction_MakeCall_ShouldThrowException_WhenDealerMakesCallFromAnotherPlayer() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        Assert.Throws<InvalidCallException>(() => auction.MakeCall(new BidCall(new Bid(1, Strain.Clubs), players[1])));
    }

    [TestCase(2, Strain.Clubs, 1, Strain.Diamonds)]
    [TestCase(1, Strain.NoTrump, 1, Strain.Diamonds)]
    [TestCase(3, Strain.Hearts, 3, Strain.Diamonds)]
    [TestCase(2, Strain.Spades, 2, Strain.Diamonds)]
    [TestCase(4, Strain.Clubs, 3, Strain.NoTrump)]
    public void Auction_MakeCall_ShouldThrowException_WhenCallerMakesLowerBidThanLastCall(int level1, Strain strain1, int level2, Strain strain2) {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new BidCall(new Bid(level1, strain1), players[0]));
        Assert.Throws<InvalidCallException>(() => auction.MakeCall(new BidCall(new Bid(level2, strain2), players[1])));
    }

    [Test]
    public void Auction_MakeCall_ShouldThrowException_WhenDoubleAfterNonBidCall() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new Pass(players[0]));
        Assert.Throws<InvalidCallException>(() => auction.MakeCall(new Double(players[1])));
    }

    [Test]
    public void Auction_MakeCall_ShouldThrowException_WhenRedoubleAfterNonDoubleCall() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new Pass(players[0]));
        Assert.Throws<InvalidCallException>(() => auction.MakeCall(new Redouble(players[1])));
        auction.MakeCall(new BidCall(new Bid(1, Strain.Clubs), players[1]));
        Assert.Throws<InvalidCallException>(() => auction.MakeCall(new Redouble(players[2])));
    }

    [Test]
    public void Auction_ShouldEnd_AfterFourInitialConsecutivePasses() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new Pass(players[0]));
        Assert.IsFalse(auction.IsOver);
        auction.MakeCall(new Pass(players[1]));
        Assert.IsFalse(auction.IsOver);
        auction.MakeCall(new Pass(players[2]));
        Assert.IsFalse(auction.IsOver);
        auction.MakeCall(new Pass(players[3]));
        Assert.IsTrue(auction.IsOver);
    }

    [Test]
    public void Auction_ShouldEnd_AfterThreeConsecutivePasses_WithBid() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new BidCall(new Bid(1, Strain.NoTrump), players[0]));
        auction.MakeCall(new Pass(players[1]));
        auction.MakeCall(new Pass(players[2]));
        auction.MakeCall(new Pass(players[3]));
        Assert.IsTrue(auction.IsOver);
        Assert.AreEqual(players[0], auction.Declarer);
        Assert.AreEqual(players[2], auction.Dummy);
        Assert.IsTrue(auction.OffendingSide.Contains(auction.Dummy));
        Assert.IsTrue(auction.OffendingSide.Contains(auction.Declarer));
    }

    [Test]
    public void Auction_ShouldEnd_AfterThreeConsecutivePasses_WithBid2() {
        var players = CreateTestPlayers();
        var auction = new Auction(players, players[0]);
        auction.MakeCall(new BidCall(new Bid(1, Strain.NoTrump), players[0]));
        auction.MakeCall(new Pass(players[1]));
        auction.MakeCall(new BidCall(new Bid(2, Strain.NoTrump), players[2]));
        auction.MakeCall(new Pass(players[3]));
        auction.MakeCall(new Pass(players[0]));
        auction.MakeCall(new Pass(players[1]));
        Assert.IsTrue(auction.IsOver);
        Assert.AreEqual(players[0], auction.Declarer);
        Assert.AreEqual(players[2], auction.Dummy);
        Assert.AreEqual(new Bid(2, Strain.NoTrump), auction.FinalContract);
        Assert.IsTrue(auction.OffendingSide.Contains(auction.Dummy));
        Assert.IsTrue(auction.OffendingSide.Contains(auction.Declarer));
    }
}