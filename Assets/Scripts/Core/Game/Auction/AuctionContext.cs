using System.Collections.Generic;

public readonly struct AuctionContext {
    public readonly BidCall HighestBid;
    public readonly List<ICall> Calls;

    public AuctionContext(BidCall highestBid, List<ICall> calls) {
        HighestBid = highestBid;
        Calls = calls;
    }
}