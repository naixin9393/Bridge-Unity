using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

using BridgeEdu.Core;
using BridgeEdu.Engines.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;

namespace BridgeEdu.Game.Bidding.Strategy.Tests {
    public class StaymanSecondResponseStrategyTests {
        [Test]
        public void GetSuggestions_NotApplicable_WhenThereIsNoBidPlaced() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() { new Pass(null) })
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenFirstBidNot1NT() {
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

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenSecondBidNot2Clubs() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null)
                })
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
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
                new BidCall(new Bid(2, Strain.NoTrump), null),
                })
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2D2NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2D2NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2D3NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2D3NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2H3H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3HCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2H4H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H4HCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2H2NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H2NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2H3NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2S3S() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S3SCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2S4S() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S4SCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2S2NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S2NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When2S3NT() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S3NTCalls())
                .Build();

            var strategy = new StaymanSecondResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_RespondTo2D2NT_With15To16HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2D2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2D2NT_With17HCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(17);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2D2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2D3NT_With15To16HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2D3NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2H3H_With15To16TP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2H3H_With17TP_Returns4H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(16);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2H4H_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(16);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H4HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2H2NT_With15To16HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2H2NT_With15To16TP_Returns3S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2H2NT_With17HCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(17);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2H2NT_With17TP_Returns4S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(17);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2H3NT_With15To17HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(3);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2H3NT_With15To17TP_Returns4S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2H3NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2S3S_With15To16TP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S3SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2S3S_With17TP_Returns4S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(16);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S3SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2S4S_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S4SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2S2NT_With15To16HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_RespondTo2S2NT_With17HCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(17);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S2NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

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
        public void GetSuggestions_RespondTo2S3NT_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(15);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(4);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Diamonds)).Returns(3);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Clubs)).Returns(2); // 1 dp
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(StaymanSecondResponse2S3NTCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new StaymanSecondResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;
            Debug.Log(suggestion.Message);

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        private List<ICall> StaymanSecondResponse2D2NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Diamonds), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.NoTrump), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2D3NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Diamonds), null),
            new Pass(null),
            new BidCall(new Bid(3, Strain.NoTrump), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2H3HCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Hearts), null),
            new Pass(null),
            new BidCall(new Bid(3, Strain.Hearts), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2H4HCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Hearts), null),
            new Pass(null),
            new BidCall(new Bid(4, Strain.Hearts), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2H2NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Hearts), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.NoTrump), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2H3NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Hearts), null),
            new Pass(null),
            new BidCall(new Bid(3, Strain.NoTrump), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2S3SCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Spades), null),
            new Pass(null),
            new BidCall(new Bid(3, Strain.Spades), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2S4SCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Spades), null),
            new Pass(null),
            new BidCall(new Bid(4, Strain.Spades), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2S2NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Spades), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.NoTrump), null),
            new Pass(null)
        };
        }

        private List<ICall> StaymanSecondResponse2S3NTCalls() {
            return new List<ICall>() {
            new BidCall(new Bid(1, Strain.NoTrump), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Clubs), null),
            new Pass(null),
            new BidCall(new Bid(2, Strain.Spades), null),
            new Pass(null),
            new BidCall(new Bid(3, Strain.NoTrump), null),
            new Pass(null)
        };
        }
    }
}