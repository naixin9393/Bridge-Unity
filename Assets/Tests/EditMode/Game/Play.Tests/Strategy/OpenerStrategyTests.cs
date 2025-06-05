using System.Collections.Generic;
using BridgeEdu.Core;
using Moq;
using NUnit.Framework;
using UnityEngine;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Play;
using System.Linq;

namespace BridgeEdu.Engines.Play {
    public class OpenerStrategyTests {
        private OpenerStrategy _strategy;

        [SetUp]
        public void Setup() {
            _strategy = new OpenerStrategy();
        }

        [Test]
        public void IsApplicable_ReturnsFalse_WhenNotFirstPlayOfGame() {
            var mockTricks = new Mock<IList<ITrick>>();
            mockTricks.Setup(t => t.Count).Returns(2);
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Tricks).Returns(mockTricks.Object.ToList());
            Assert.IsFalse(_strategy.IsApplicable(mockContext.Object));
        }

        [Test]
        public void IsApplicable_ReturnsTrue_WhenFirstPlayOfGame() {
            var trick = new Trick(
                players: new List<IPlayer>(),
                strain: Strain.NoTrump,
                currentPlayer: null
            );
            var tricks = new List<ITrick> { trick };
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Tricks).Returns(tricks);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            Assert.IsTrue(_strategy.IsApplicable(mockContext.Object));
        }


        [Test, TestCaseSource(nameof(HonorSequenceTestCases))]
        public void GetSuggestions_ReturnsFirstOfHonor_WhenHonorSequenceAndContractNT(List<Card> hand, Card expected) {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.Cards).Returns(hand);
            var trick = new Trick(
                players: new List<IPlayer>(),
                strain: Strain.NoTrump,
                currentPlayer: null
            );
            var tricks = new List<ITrick> { trick };
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Tricks).Returns(tricks);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(hand);
            mockContext.Setup(c => c.Hand).Returns(mockHand.Object);

            var suggestions = _strategy.GetSuggestions(mockContext.Object);
            Assert.IsNotEmpty(suggestions);

            var firstSuggestion = suggestions[0];
            Assert.IsNotNull(firstSuggestion);

            Debug.Log(firstSuggestion.Message);

            Assert.AreEqual(expected, suggestions[0].Card);
        }

        [Test, TestCaseSource(nameof(FifthSuitWithHonorTestCases))]
        public void GetSuggestions_ReturnsFourthCard_WhenFifthSuitWithHonor(List<Card> hand, Card expected) {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.Cards).Returns(hand);
            var trick = new Trick(
                players: new List<IPlayer>(),
                strain: Strain.NoTrump,
                currentPlayer: null
            );
            var tricks = new List<ITrick> { trick };
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Tricks).Returns(tricks);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(hand);
            mockContext.Setup(c => c.Hand).Returns(mockHand.Object);

            var suggestions = _strategy.GetSuggestions(mockContext.Object);
            Assert.IsNotEmpty(suggestions);

            var firstSuggestion = suggestions[0];
            Assert.IsNotNull(firstSuggestion);

            Debug.Log(firstSuggestion.Message);

            Assert.AreEqual(expected, suggestions[0].Card);
        }

        [Test, TestCaseSource(nameof(SequenceTwoBelowHonorTestCases))]
        public void GetSuggestions_ReturnsHighestHonor_WhenSequenceOfTwoHonorAndThirdTwoLowerThanHonor(List<Card> hand, Card expected) {
            var mockHand = new Mock<IHand>();
            mockHand.Setup(h => h.Cards).Returns(hand);
            var trick = new Trick(
                players: new List<IPlayer>(),
                strain: Strain.NoTrump,
                currentPlayer: null
            );
            var tricks = new List<ITrick> { trick };
            var mockContext = new Mock<IPlayingContext>();
            mockContext.Setup(c => c.Tricks).Returns(tricks);
            mockContext.Setup(c => c.Contract).Returns(new Bid(1, Strain.NoTrump));
            mockContext.Setup(c => c.PossibleCards).Returns(hand);
            mockContext.Setup(c => c.Hand).Returns(mockHand.Object);

            var suggestions = _strategy.GetSuggestions(mockContext.Object);
            Assert.IsNotEmpty(suggestions);

            var firstSuggestion = suggestions[0];
            Assert.IsNotNull(firstSuggestion);

            Debug.Log(firstSuggestion.Message);

            Assert.AreEqual(expected, suggestions[0].Card);
        }

        private static IEnumerable<TestCaseData> SequenceTwoBelowHonorTestCases {
            get {
                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Ace, Suit.Clubs),
                        new(Rank.King, Suit.Clubs),
                        new(Rank.Jack, Suit.Clubs),
                        new(Rank.Ten, Suit.Clubs)
                    },
                    new Card(Rank.Ace, Suit.Clubs)
                ).SetName("Sequence A K J 10 returns A");

                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.King, Suit.Hearts),
                        new(Rank.Eight, Suit.Hearts),
                        new(Rank.Queen, Suit.Hearts),
                        new(Rank.Ten, Suit.Hearts)
                    },
                    new Card(Rank.King, Suit.Hearts)
                ).SetName("Sequence K Q 10 8 returns K");

                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Queen, Suit.Hearts),
                        new(Rank.Nine, Suit.Hearts),
                        new(Rank.Jack, Suit.Hearts),
                        new(Rank.Seven, Suit.Hearts)
                    },
                    new Card(Rank.Queen, Suit.Hearts)
                ).SetName("Sequence Q J 9 7 returns Q");

                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Jack, Suit.Hearts),
                        new(Rank.Ten, Suit.Hearts),
                        new(Rank.Eight, Suit.Hearts),
                        new(Rank.Seven, Suit.Hearts)
                    },
                    new Card(Rank.Jack, Suit.Hearts)
                ).SetName("Sequence J 10 8 7 returns J");
            }
        }

        private static IEnumerable<TestCaseData> FifthSuitWithHonorTestCases {
            get {
                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Ace, Suit.Clubs),
                        new(Rank.Ten, Suit.Clubs),
                        new(Rank.Seven, Suit.Clubs),
                        new(Rank.Three, Suit.Clubs),
                        new(Rank.Two, Suit.Clubs)
                    },
                    new Card(Rank.Three, Suit.Clubs)
                ).SetName("Fifth suit with honor A 10 7 3 2 returns 3");

                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Five, Suit.Hearts),
                        new(Rank.Six, Suit.Hearts),
                        new(Rank.Nine, Suit.Hearts),
                        new(Rank.Queen, Suit.Hearts),
                        new(Rank.Two, Suit.Hearts)
                    },
                    new Card(Rank.Five, Suit.Hearts)
                ).SetName("Fifth suit with honor Q 9 6 5 2 returns 5");
            }
        }

        private static IEnumerable<TestCaseData> HonorSequenceTestCases {
            get {
                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Ace, Suit.Clubs),
                        new(Rank.King, Suit.Clubs),
                        new(Rank.Queen, Suit.Clubs)
                    },
                    new Card(Rank.Ace, Suit.Clubs)
                ).SetName("Honor sequence AKQ returns A");

                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.King, Suit.Clubs),
                        new(Rank.Queen, Suit.Clubs),
                        new(Rank.Jack, Suit.Clubs),
                    },
                    new Card(Rank.King, Suit.Clubs)
                ).SetName("Honor sequence KQJ returns K");
                yield return new TestCaseData(
                    new List<Card> {
                        new(Rank.Queen, Suit.Clubs),
                        new(Rank.Jack, Suit.Clubs),
                        new(Rank.Ten, Suit.Clubs),
                    },
                    new Card(Rank.Queen, Suit.Clubs)
                ).SetName("Honor sequence AKQ returns  Q");
            }
        }
    }
}