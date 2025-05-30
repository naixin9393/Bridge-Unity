using System.Collections.Generic;
using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Game.Play.Exceptions;

namespace BridgeEdu.Game.Players.Tests {
    public class PlayerTests {
        [Test]
        public void Player_Constructor_SetsPosition() {
            Position position = Position.North;
            IPlayer player = new HumanPlayer(position);
            Assert.AreEqual(position, player.Position);
        }

        [Test]
        public void Player_Hand_StartsEmpty() {
            IPlayer player = new HumanPlayer(Position.North);
            Assert.AreEqual(0, player.Hand.NumberOfCards);
        }

        [Test]
        public void ReceiveCards_AddsCardsToHand() {
            IPlayer player = new HumanPlayer(Position.North);
            var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs)
        };
            player.ReceiveCards(cards);
            Assert.AreEqual(1, player.Hand.NumberOfCards);
        }

        [Test]
        public void PlayCard_RemovesCardFromHand() {
            var player = new HumanPlayer(Position.North);
            var card = new Card(Rank.Ace, Suit.Clubs);
            player.ReceiveCards(new List<Card> { card });
            player.PlayCard(card);
            Assert.AreEqual(0, player.Hand.NumberOfCards);
        }

        [Test]
        public void PlayCard_ThrowsException_IfCardIsNotInHand() {
            var player = new HumanPlayer(Position.North);
            var card = new Card(Rank.Ace, Suit.Clubs);
            var card2 = new Card(Rank.Ace, Suit.Spades);
            player.ReceiveCards(new List<Card> { card2 });
            Assert.Catch<CardNotInHandException>(() => player.PlayCard(card));
        }
    }
}