using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public interface IPlayer {
    Position Position { get; }
    ReadOnlyCollection<Card> Hand { get; }
    void ReceiveCards(List<Card> cards);
    void RequestPlayerPlayDecision();
    void RequestPlayerCallDecision();
    void PlayCard(Card card);
    event Action<Card> PlayedCard;
    event Action<ICall> MadeCall;
}