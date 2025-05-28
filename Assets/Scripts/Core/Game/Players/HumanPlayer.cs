using System;
using System.Collections.Generic;

public class HumanPlayer : IPlayer {
    private readonly IHand _hand = new Hand();

    public Position Position { get; private set; }
    public event Action<Card, IPlayer> OnCardChosen = delegate { };
    public event Action<ICall> OnCallChosen = delegate { };
    public IHand Hand => _hand;

    public HumanPlayer(Position position) => Position = position;
    public void ReceiveCards(List<Card> cards) => _hand.AddCards(cards);

    public void PlayCard(Card card) {
        if (!_hand.HasCard(card))
            throw new CardNotInHandException(card, this);
        _hand.RemoveCard(card);
    }

    public override string ToString() => $"{Position}";
    public void RequestPlayerPlayDecision(PlayingContext _) { }
    public void RequestPlayerCallDecision(List<BiddingSuggestion> _) { }
}
