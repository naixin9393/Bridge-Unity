using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : Player {
    private IDelayService _coroutineStarter;
    public override event Action<Card, IPlayer> OnCardChosen = delegate { };
    public override event Action<ICall> OnCallChosen = delegate { };

    public ComputerPlayer(Position position, IDelayService coroutineStarter) : base(position){
        _coroutineStarter = coroutineStarter;
    }
    public override void RequestPlayerPlayDecision(PlayingContext playingContext) {
        if (playingContext.Dummy == this && IsPartner(playingContext.Human)) return; // Dummy can't play if declarer is human
        if (playingContext.Dummy == playingContext.Human && IsPartner(playingContext.Human)) return; // Human can't play if declarer is dummy
        if (playingContext.PossibleCards.Count == 0)
            throw new EmptyHandException(this);
        var possibleCards = playingContext.PossibleCards;
        var card = possibleCards[0];
        Debug.Log($"ComputerPlayer: PlayCard: {card}");
        _coroutineStarter.DelayAction(
            0.6f,
            () => OnCardChosen?.Invoke(card, this)
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

    public override void RequestPlayerCallDecision(List<BiddingSuggestion> biddingSuggestions) {
        ICall suggestedCall = new Pass(this);
        if (biddingSuggestions.Count != 0)
            suggestedCall = biddingSuggestions[0].Call;
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
            () => OnCallChosen?.Invoke(call)
        );
    }
}
