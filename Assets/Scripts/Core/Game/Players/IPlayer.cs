using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public interface IPlayer {
    Position Position { get; }
    ReadOnlyCollection<Card> Hand { get; }
    void ReceiveCards(List<Card> cards);
    void RequestPlayerPlayDecision(PlayingContext playingContext);
    void RequestPlayerCallDecision(BiddingContext auctionContext);
    void PlayCard(Card card);
    event Action<Card, IPlayer> OnCardChosen;
    event Action<ICall> OnCallChosen;
}