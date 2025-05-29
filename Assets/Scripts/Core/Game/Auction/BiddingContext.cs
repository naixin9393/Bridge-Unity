using System.Collections.Generic;

public readonly struct BiddingContext {
    public readonly List<ICall> Calls;
    public readonly IHand Hand { get; }
    public readonly Position CurrentPlayerPosition { get; }

    public BiddingContext(List<ICall> calls, IHand hand, Position currentPlayerPosition) {
        Calls = calls;
        Hand = hand;
        CurrentPlayerPosition = currentPlayerPosition;
    }
}