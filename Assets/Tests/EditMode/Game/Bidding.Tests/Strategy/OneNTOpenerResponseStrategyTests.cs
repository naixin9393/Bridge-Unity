using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class OneNTOpenerResponseStrategyTests {
    [Test]
    public void GetSuggestions_Applicable_When_AtLeastTwoBids() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(2, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();
        Assert.IsTrue(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_When_OneBid() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_NotApplicable_When_Stayman() {
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(2, Strain.Clubs), null),
                    new Pass(null)
                }
            )
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();
        Assert.IsFalse(strategy.IsApplicable(biddingContext));
    }

    [Test]
    public void GetSuggestions_ResponseTo2NTWith15To16HCP_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(15);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(2, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }

    [Test]
    public void GetSuggestions_ResponseTo2NTWith17HCP_Returns3NT() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(17);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(2, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
    }

    [Test]
    public void GetSuggestions_ResponseTo3NT_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(3, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }

    [Test]
    public void GetSuggestions_ResponseTo4NTWith15To16HCP_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(16);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(4, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }

    [Test]
    public void GetSuggestions_ResponseTo4NTWith17HCP_Returns6NT() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(17);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(4, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();

        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(6, Strain.NoTrump), bid);
    }

    [Test]
    public void GetSuggestions_ResponseTo6NT_ReturnsPass() {
        var mockHand = new Mock<IHand>();
        mockHand.Setup(h => h.HCP).Returns(8);
        mockHand.Setup(h => h.IsBalanced).Returns(true);
        var biddingContext = new BiddingContextBuilder()
            .WithCalls(new
                List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null),
                    new BidCall(new Bid(6, Strain.NoTrump), null),
                    new Pass(null)
                }
            )
            .WithHand(mockHand.Object)
            .Build();
        
        var strategy = new OneNTOpenerResponseStrategy();

        var suggestions = strategy.GetSuggestions(biddingContext);
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }
}