using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;
using NUnit.Framework;
using UnityEngine;

namespace BridgeEdu.Engines.Bidding {
    public class TransferResponseStrategyTests {
        [Test]
        public void GetSuggestions_NotApplicable_When_FirstBidNot1NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                })
                .Build();

            var strategy = new TransferResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_When_LastBidNot2D_or_2H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null)})
                .Build();

            var strategy = new TransferResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_1NT2H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2HCalls())
                .Build();

            var strategy = new TransferResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_1NT2D() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2DCalls())
                .Build();

            var strategy = new TransferResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_ResponseTo2H_Returns2S() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2HCalls())
                .Build();

            var strategy = new TransferResponseStrategy();

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

        [Test]
        public void GetSuggestions_ResponseTo2D_Returns2H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2DCalls())
                .Build();

            var strategy = new TransferResponseStrategy();

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

        private List<ICall> Transfer2HCalls() {
            return new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Hearts), null),
                new Pass(null)
            };
        }

        private List<ICall> Transfer2DCalls() {
            return new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null)
            };
        }
    }
}