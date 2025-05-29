using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class StaymanResponseStrategyTests {
    [Test]
    public void GetSuggestions_NotApplicable_WhenThereIsNoBidPlaced() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new Pass(null) })
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_WhenThereIsNoBidPlaced_2() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new Pass(null), new Pass(null) })
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_WhenNot2Clubs() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), null) })
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_WhenNot1NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null) })
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NoApplicable_WhenLastBidNot2Clubs() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                })
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_Applicable_When2ClubsAnd1NTOpening() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(StaymanCalls())
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_WithNoFourMajor_Returns2Diamonds() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(3);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(StaymanCalls())
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Diamonds), bid);
    }

    [Test]
    public void GetSuggestions_WithFourHearts_Returns2Hearts() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(StaymanCalls())
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);
        
        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Hearts), bid);
    }

    [Test]
    public void GetSuggestions_WithFourSpadesAndNoFourHearts_Returns2Spades() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
        mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(3);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(StaymanCalls())
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new StaymanResponseStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Spades), bid);
    }

    private List<ICall> StaymanCalls() {
        return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null)
        };
    }
}