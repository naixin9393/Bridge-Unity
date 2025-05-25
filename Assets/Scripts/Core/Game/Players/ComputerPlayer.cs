using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ComputerPlayer : IPlayer {
    public Position Position { get; private set; }
    private readonly List<Card> _hand = new();

    public event Action<Card, IPlayer> OnCardChosen = delegate { };
    public event Action<ICall> OnCallChosen;

    public ReadOnlyCollection<Card> Hand => new(_hand);
    
    private IDelayService _coroutineStarter;

    public ComputerPlayer(Position position, IDelayService coroutineStarter) {
        Position = position;
        _coroutineStarter = coroutineStarter;
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

    public void RequestPlayerPlayDecision(PlayingContext playingContext) {
        //if (playingContext.Dummy == this) return; // Dummy can't play
        if (playingContext.PossibleCards.Count == 0)
            throw new EmptyHandException(this);
        var possibleCards = playingContext.PossibleCards;
        var card = possibleCards[0];
        Debug.Log($"ComputerPlayer: PlayCard: {card}");
        _coroutineStarter.DelayAction(
            0.6f,
            () =>OnCardChosen?.Invoke(card, this)
        );
    }

    public void RequestPlayerCallDecision(BiddingContext auctionContext) {
        if (auctionContext.HighestBid == null) {
            _coroutineStarter.DelayAction(
                0.4f,
                () => OnCallChosen?.Invoke(new BidCall(new Bid(1, Strain.NoTrump), this))
            );
            return;
        }
        if (auctionContext.HighestBid.Bid.Level == 7) {
            _coroutineStarter.DelayAction(
                0.4f,
                () => OnCallChosen?.Invoke(new Pass(this))
        );
            return;
        }
        _coroutineStarter.DelayAction(
            0.4f,
            () =>OnCallChosen?.Invoke(new BidCall(new Bid(auctionContext.HighestBid.Bid.Level + 1, auctionContext.HighestBid.Bid.Strain), this))
        );
    }
}
