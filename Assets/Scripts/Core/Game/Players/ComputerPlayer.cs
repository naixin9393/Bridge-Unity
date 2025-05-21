using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ComputerPlayer : IPlayer {
    public Position Position { get; private set; }
    private readonly List<Card> _hand = new();

    public event Action<Card> OnCardChosen = delegate { };
    public event Action<ICall> OnCallChosen;

    public ReadOnlyCollection<Card> Hand => new(_hand);

    public ComputerPlayer(Position position) {
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
    
    public void RequestPlayerPlayDecision() {
        if (_hand.Count == 0)
            throw new EmptyHandException(this);
        Card card = _hand[^1];
        OnCardChosen?.Invoke(card);
        _hand.RemoveAt(_hand.Count - 1);
    }

    public void RequestPlayerCallDecision(AuctionContext auctionContext) {
        if (auctionContext.HighestBid == null)
            OnCallChosen?.Invoke(new BidCall(new Bid(1, Strain.NoTrump), this));
        if (auctionContext.HighestBid.Bid.Level == 7) {
                OnCallChosen?.Invoke(new Pass(this));
                return;
            }
        OnCallChosen?.Invoke(new BidCall(new Bid(auctionContext.HighestBid.Bid.Level + 1, auctionContext.HighestBid.Bid.Strain), this));
    }
}
