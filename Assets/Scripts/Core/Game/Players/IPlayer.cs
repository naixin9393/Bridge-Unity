using System;
using System.Collections.Generic;

public interface IPlayer {
    Position Position { get; }
    IHand Hand { get; }
    // ReadOnlyCollection<Card> Hand { get; }
    void ReceiveCards(List<Card> cards);
    void RequestPlayerPlayDecision(PlayingContext playingContext);
    void RequestPlayerCallDecision(List<BiddingSuggestion> biddingSuggestions);
    void PlayCard(Card card);
    event Action<Card, IPlayer> OnCardChosen;
    event Action<ICall> OnCallChosen;
}