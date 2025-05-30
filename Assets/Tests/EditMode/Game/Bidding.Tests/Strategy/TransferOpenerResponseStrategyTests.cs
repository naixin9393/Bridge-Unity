using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;
using Moq;
using NUnit.Framework;

namespace BridgeEdu.Engines.Bidding {
    public class TransferOpenerResponseStrategyTests {
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

            var strategy = new TransferOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenSecondBidNot2D_or_2H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null)
                })
                .Build();

            var strategy = new TransferOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_NotApplicable_WhenThirdBidNot2H_or_2S() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Clubs), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null),
                })
                .Build();

            var strategy = new TransferOpenerResponseStrategy();
            Assert.IsFalse(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_1NT2H2S() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .Build();

            var strategy = new TransferOpenerResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_Applicable_When_1NT2D2H() {
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2HCalls())
                .Build();

            var strategy = new TransferOpenerResponseStrategy();
            Assert.IsTrue(strategy.IsApplicable(biddingContext));
        }

        [Test]
        public void GetSuggestions_With0To7HCP_ReturnsPass() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(7);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_With0To7HCP_ReturnsPass2() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(7);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Pass, call.Type);
        }

        [Test]
        public void GetSuggestions_With8To9HCP_Returns2NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(5);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_With8To9HCP_Returns2NT2() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(5);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2HCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_With10PlusHCP_Returns3NT() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(5);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];

            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
        }

        [Test]
        public void GetSuggestions_With8To9HCPAnd6PlusHearts_Returns3H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(6);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(3, Strain.Hearts), bid);
        }

        [Test]
        public void GetSuggestions_With8To9HCPAnd6PlusSpades_Returns3S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(8);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(6);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(3, Strain.Spades), bid);
        }

        [Test]
        public void GetSuggestions_With10PlusHCPAnd6PlusHearts_Returns4H() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Hearts)).Returns(6);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(4, Strain.Hearts), bid);
        }

        [Test]
        public void GetSuggestions_With10PlusHCPAnd6PlusSpades_Returns4S() {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.HCP).Returns(10);
            mockHand.Setup(h => h.NumberOfCardsOfSuit(Suit.Spades)).Returns(6);
            var biddingContext = new BiddingContextBuilder()
                .WithCalls(Transfer2SCalls())
                .WithHand(mockHand.Object)
                .Build();

            var strategy = new TransferOpenerResponseStrategy();

            var suggestions = strategy.GetSuggestions(biddingContext);
            Assert.AreEqual(1, suggestions.Count);

            var suggestion = suggestions[0];
            var call = suggestion.Call;

            Assert.AreEqual(CallType.Bid, call.Type);

            var bidCall = call as BidCall;
            var bid = bidCall.Bid;

            Assert.AreEqual(new Bid(4, Strain.Spades), bid);
        }

        private List<ICall> Transfer2SCalls() {
            return new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Hearts), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Spades), null),
                new Pass(null)
            };
        }

        private List<ICall> Transfer2HCalls() {
            return new List<ICall>() {
                new BidCall(new Bid(1, Strain.NoTrump), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Diamonds), null),
                new Pass(null),
                new BidCall(new Bid(2, Strain.Hearts), null),
                new Pass(null)
            };
        }
    }
}