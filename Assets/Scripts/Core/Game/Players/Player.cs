using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Player {
    public Position Position { get; private set; }
    private readonly List<Card> _hand = new();

    public ReadOnlyCollection<Card> Hand => new(_hand);

    public Player(Position position) {
        Position = position;
        _hand = new List<Card>();
    }

    public void ReceiveCards(List<Card> cards) {
        _hand.AddRange(cards);
    }

    public void PlayCard(Card card) {
        _hand.Remove(card);
    }
}
