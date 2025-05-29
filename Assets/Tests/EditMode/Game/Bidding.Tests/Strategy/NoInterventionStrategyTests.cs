using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class NoInterventionStrategyTests {
    [Test]
    public void GetSuggestions_NotApplicable_OnOpeningBid() {
        var biddingContext = new BiddingContextBuilder()
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.False(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_WhenNoBidPlaced() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new Pass(null) })
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.False(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_WhenPartnerTurn() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), new HumanPlayer(Position.East)), new Pass(null) })
            .WithCurrentPosition(Position.West)
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.False(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_Applicable_WhenRightOpponentTurn() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), new HumanPlayer(Position.South)) })
            .WithCurrentPosition(Position.East)
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.True(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_Applicable_WhenLeftOpponentTurn() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), new HumanPlayer(Position.South)), new Pass(null), new Pass(null) })
            .WithCurrentPosition(Position.West)
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.True(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_RightOpponent_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(15);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), new HumanPlayer(Position.East)) })
            .WithCurrentPosition(Position.North)
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }

    [Test]
    public void GetSuggestions_LeftOpponent_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(15);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), new HumanPlayer(Position.East)), new Pass(null), new BidCall(new Bid(3, Strain.NoTrump), new HumanPlayer(Position.West)) })
            .WithCurrentPosition(Position.South)
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new NoInterventionStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }
}