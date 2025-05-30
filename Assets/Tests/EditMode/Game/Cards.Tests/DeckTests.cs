using NUnit.Framework;

using BridgeEdu.Core;

namespace BridgeEdu.Game.Cards.Tests {
    public class DeckTests {
        [Test]
        public void NewDeck_Has52Cards() {
            var deck = new Deck();
            Assert.AreEqual(52, deck.Cards.Count);
        }

        [Test]
        public void DealingCards_ReducesCardCount() {
            var deck = new Deck();
            deck.DealCard();
            Assert.AreEqual(51, deck.Cards.Count);
        }

        [Test]
        public void InsertCard_IntoDeck_IncreasesCardCount() {
            var deck = new Deck();
            deck.DealCard();
            deck.DealCard();
            var card = new Card(Rank.Ace, Suit.Clubs);
            deck.InsertCard(card);
            Assert.AreEqual(51, deck.Cards.Count);
        }
    }
}