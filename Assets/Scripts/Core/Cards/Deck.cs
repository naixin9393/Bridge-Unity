using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Deck{
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

    public void InsertCard(Card card) {
        _cards.Add(card);
    }
}
