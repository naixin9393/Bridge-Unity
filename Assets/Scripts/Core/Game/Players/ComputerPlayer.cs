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
        if (playingContext.Dummy == this && IsPartner(playingContext.Human)) return; // Dummy can't play if declarer is human
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

    private bool IsPartner(IPlayer human) {
        return human.Position switch {
            Position.North => Position == Position.South,
            Position.East => Position == Position.West,
            Position.South => Position == Position.North,
            Position.West => Position == Position.East,
            _ => throw new NotImplementedException(),
        };
    }

    public void RequestPlayerCallDecision(BiddingSuggestion biddingSuggestion) {
        var suggestedCall = biddingSuggestion.Call;
        ICall call;
        switch (suggestedCall.Type) {
            case CallType.Pass:
                call = new Pass(this);
                break;
            case CallType.Double:
                call = new Double(this);
                break;
            case CallType.Redouble:
                call = new Redouble(this);
                break;
            case CallType.Bid:
                var suggestedBidCall = suggestedCall as BidCall;
                call = new BidCall(suggestedBidCall.Bid, this);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(suggestedCall), suggestedCall, null);
        }
        _coroutineStarter.DelayAction(
            0.4f,
            () =>OnCallChosen?.Invoke(call)
        );
    }
}
