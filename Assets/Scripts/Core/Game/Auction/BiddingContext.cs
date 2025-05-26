using System.Collections.Generic;

public readonly struct BiddingContext {
    public readonly List<ICall> Calls;
    public readonly List<Card> Hand { get; }

    public BiddingContext(List<ICall> calls, List<Card> hand) {
        Calls = calls;
        Hand = hand;
    }
}