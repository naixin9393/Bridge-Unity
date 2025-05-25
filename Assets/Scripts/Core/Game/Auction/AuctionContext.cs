using System.Collections.Generic;

public readonly struct BiddingContext {
    public readonly BidCall HighestBid;
    public readonly List<ICall> Calls;
    public readonly List<Card> Hand { get; }

    public BiddingContext(BidCall highestBid, List<ICall> calls, List<Card> hand) {
        HighestBid = highestBid;
        Calls = calls;
        Hand = hand;
    }
}