using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BridgeEdu.Core {
    public class Deck {
        private readonly List<Card> _cards = new();
        public ReadOnlyCollection<Card> Cards => new(_cards);

        public Deck() {
            foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach (Rank rank in Enum.GetValues(typeof(Rank))) {
                    _cards.Add(new Card(rank, suit));
                }
            }
        }

        public void Shuffle() {
            for (int i = _cards.Count - 1; i > 0; i--) {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);

                Card temp = _cards[i];
                _cards[i] = _cards[randomIndex];
                _cards[randomIndex] = temp;
            }
        }

        public Card DealCard() {
            Card lastCard = _cards[^1];
            _cards.RemoveAt(_cards.Count - 1);
            return lastCard;
        }

        public List<Card> DealCards(int count) {
            List<Card> cards = new();
            for (int i = 0; i < count; i++) {
                cards.Add(DealCard());
            }
            return cards;
        }

        public void InsertCard(Card card) {
            _cards.Insert(0, card);
        }

        public void RemoveCards(List<Card> cards) {
            foreach (var card in cards) {
                _cards.Remove(card);
            }
        }
    }
}