using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

using BridgeEdu.Core;
using BridgeEdu.Engines.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;

namespace BridgeEdu.Game.Bidding.Strategy.Tests {
    public class StaymanOpenerResponseStrategyTests {
        [Test]
        public void GetSuggestions_NotApplicable_WhenThereIsNoBidPlaced() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new Pass(null) })
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenNot1NTOpening() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new BidCall(new Bid(1, Strain.Clubs), null) })
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenNot3Bids() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(3, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(4, Strain.NoTrump), null)
                })
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenSecondBidNot2Clubs() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.NoTrump), null),
                })
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenNotTurn() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null),
                new Pass(null)
                })
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_FirstBid1NTSecondBid2ClubsThirdBid2Diamonds() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2DCalls())
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_FirstBid1NTSecondBid2ClubsThirdBid2Hearts() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2HCalls())
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_FirstBid1NTSecondBid2ClubsThirdBid2Spades() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2SCalls())
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_ResponseTo2D_With8To9HCP_Returns2NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2DCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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
        public void GetSuggestions_ResponseTo2D_With10To15HCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2DCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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
        public void GetSuggestions_ResponseTo2H_With8To9TPAndFourHearts_Returns3H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // doubleton 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(3, Strain.Hearts), bid);
        }

        [Test]
        public void GetSuggestions_ResponseTo2H_With10To15TPAndFourHearts_Returns4H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(9);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // doubleton 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(4, Strain.Hearts), bid);
        }

        [Test]
        public void GetSuggestions_ResponseTo2H_With8To9HCPAndFourSpades_Returns2NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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
        public void GetSuggestions_ResponseTo2H_With10To15HCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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
        public void GetSuggestions_ResponseTo2S_With8To9TPAndFourSpades_Returns3S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(3, Strain.Spades), bid);
        }

        [Test]
        public void GetSuggestions_ResponseTo2S_With10PlusTPAndFourSpades_Returns4S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(9);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(4, Strain.Spades), bid);
        }

        [Test]
        public void GetSuggestions_ResponseTo2S_With8To9HCPAndFourHeartsNoFourSpades_Returns2NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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
        public void GetSuggestions_ResponseTo2S_With10To15HCPAndFourHeartsNoFourSpades_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanOpenerResponse2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanOpenerResponseStrategy();

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

        private List<ICall> StaymanOpenerResponse2DCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Diamonds), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanOpenerResponse2HCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Hearts), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanOpenerResponse2SCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Spades), null),
            new Pass(null)
        };
        }
    }
}