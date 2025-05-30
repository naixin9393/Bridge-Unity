using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Engines.Bidding {
    public class OpenerStrategyTests {
        [Test]
        public void GetSuggestions_Applicable_WhenNoBidPlaces() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { })
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_WhenNoBidPlaces_2() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new Pass(null) })
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_WhenNoBidPlaces_3() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new Pass(null), new Pass(null) })
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenThereIsABidPlaced() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.NoTrump), null) })
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_With15To17HCPAndBalanced_Returns1NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(1, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_WIth0To11HCPAndBalanced_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(11);
            mockHand.Setup(h => h.IsBalanced).Returns(true);
            var biddingContext = new BiddingContextBuilder()
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new OpenerStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        // Add more openings
    }
}