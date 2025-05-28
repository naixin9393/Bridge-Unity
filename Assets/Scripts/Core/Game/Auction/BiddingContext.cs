using System.Collections.Generic;

public readonly struct BiddingContext {
    public readonly List<ICall> Calls;
    public readonly IHand Hand { get; }

    public BiddingContext(List<ICall> calls, IHand hand) {
        Calls = calls;
        Hand = hand;
    }
}