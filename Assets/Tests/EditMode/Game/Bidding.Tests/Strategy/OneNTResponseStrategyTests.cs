using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;

namespace BridgeEdu.Engines.Bidding {
    public class OneNTResponseStrategyTests {
        [Test]
        public void GetSuggestions_Applicable_After1NTBid() {
            var mockHand = new Mock<IHand>();
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();
            var strategy = new OneNTResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_After1NTBid_2() {
            var mockHand = new Mock<IHand>();
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new Pass(null),
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();
            var strategy = new OneNTResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_Without1NTBid() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(7);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new Pass(null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NoApplicable_WhenLastBidNot1NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(3, Strain.NoTrump), null),
                new Pass(null)
                    })
                .Build();

            var strategy = new OneNTResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_With8To9HCP_Balanced_Returns2NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_With10To15HCP_Balanced_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();

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
        public void GetSuggestions_With16To17HCP_Balanced_Returns4NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(17);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(4, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_With18PlusHCP_Balanced_Returns6NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(18);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();

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
        public void GetSuggestions_With4MajorAnd8PlusHCP_Returns2Clubs() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(
                    new List<ICall>() {
                    new BidCall(new Bid(1, Strain.NoTrump), null),
                    new Pass(null)
                    })
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OneNTResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(2, Strain.Clubs), bid);
        }

    }
}