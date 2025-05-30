using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Engines.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;
using UnityEngine;

namespace BridgeEdu.Game.Bidding.Strategy.Tests {
    public class TransferStrategyTests {
        [Test]
        public void GetSuggestions_NotApplicable_When_FirstBidNot1NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null)
                })
                .Build();

            var strategy = new TransferStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_When_NoFiveMajorCards() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(TransferCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_With5PlusSpades_ReturnsTransfer2H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(5);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(TransferCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferStrategy();
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
        public void GetSuggestions_With5PlusHearts_ReturnsTransfer2D() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(5);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(TransferCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferStrategy();
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

        private List<ICall> TransferCalls() {
            return new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
            };
        }
    }
}