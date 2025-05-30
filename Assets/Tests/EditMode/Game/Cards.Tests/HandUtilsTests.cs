using System.Collections.Generic;
using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Utils;
namespace BridgeEdu.Game.Cards.Tests {
    public class HandUtilsTests {
        [Test]
        public void IsBalancedHand_ReturnsFalse_IfHandHasMoreThan13Cards() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Clubs),
            new(Rank.Seven, Suit.Clubs),
            new(Rank.Six, Suit.Clubs),
            new(Rank.Five, Suit.Clubs),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Three, Suit.Clubs),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Ace, Suit.Spades)
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsFalse_IfHandHasLessThan13Cards() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
        };

            var hand = new Hand();
            hand.AddCards(cards);
            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsTrue_IfHand_Is5332() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsTrue_IfHand_Is4432() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsTrue_IfHand_Is4333() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Spades),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts),
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsFalse_IfHand_Is5431() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts)
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsBalancedHand_ReturnsFalse_IfHand_Is6421() {
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts)
        };

            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.IsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ContainsBalancedHand_ReturnsFalse_IfHandIsEmpty() {
            var hand = new List<Card>();
            var result = HandUtils.ContainsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ContainsBalancedHand_ReturnsTrue_IfHand_Is6332() {
            var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts)
        };

            var result = HandUtils.ContainsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ContainsBalancedHand_ReturnsTrue_IfHand_Is5532() {
            var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts)
        };

            var result = HandUtils.ContainsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ContainsBalancedHand_ReturnsTrue_IfHand_Is4443() {
            var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts)
        };

            var result = HandUtils.ContainsBalancedHand(hand);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ContainsBalancedHand_ReturnsFalse_IfHand_Is6422() {
            var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts)
        };

            var result = HandUtils.ContainsBalancedHand(hand);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void CalculateTotalPoints_ReturnsExpectedValue() {
            // HCP = 4+3+1 = 8, DP = singleton(2) + longsuit(1) = 3, total = 11
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Five, Suit.Hearts),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Clubs),
        };
            var fittingSuit = Suit.Hearts;
            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.CalculateTotalPoints(hand, fittingSuit);
            Assert.AreEqual(11, result);
        }

        [Test]
        public void CalculateTotalPoints_ReturnsExpectedValue2() {
            // HCP = 4+3+1 = 8, DP = void (3) + longsuit(2) = 5, total = 13
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Five, Suit.Hearts),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
        };
            var fittingSuit = Suit.Hearts;
            var hand = new Hand();
            hand.AddCards(cards);

            var result = HandUtils.CalculateTotalPoints(hand, fittingSuit);
            Assert.AreEqual(13, result);
        }
    }
}