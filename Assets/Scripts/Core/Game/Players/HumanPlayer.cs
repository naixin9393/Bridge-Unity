using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class HumanPlayer : IPlayer {
    public Position Position { get; private set; }
    private readonly List<Card> _hand = new();

    public event Action<Card, IPlayer> OnCardChosen = delegate { };
    public event Action<ICall> OnCallChosen;

    public ReadOnlyCollection<Card> Hand => new(_hand);

    public HumanPlayer(Position position) {
        Position = position;
        _hand = new List<Card>();
    }

    public void ReceiveCards(List<Card> cards) {
        _hand.AddRange(cards);
    }

    public void PlayCard(Card card) {
        if (!_hand.Contains(card))
            throw new CardNotInHandException(card, this);
        _hand.Remove(card);
    }

    public override string ToString() {
        return $"{Position}";
    }
    
    public void RequestPlayerPlayDecision(PlayingContext _) {
        // Feedback from UI
    }

    public void RequestPlayerCallDecision(BiddingSuggestion _) {
        // Feedback from UI
    }
}
