public class OpeningBidState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        var hand = biddingContext.Hand;
        int HCP = HandUtils.CalculateHighCardPoints(hand);
        return new BiddingSuggestion(
            message: BiddingMessages.OneNT(HCP),
            call: new BidCall(new Bid(1, Strain.NoTrump), null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        BidCall bidCall;
        Bid bid;
        switch (call.Type) {
            // Stay in opening bid state if pass
            case CallType.Pass:
                return new OpeningBidState();
            case CallType.Bid:
                bidCall = call as BidCall;
                bid = bidCall.Bid;
                break;
            default:
                return new UnknownState();
        }
        // Move to Open1NTState if bid is 1NT
        if (bid.Equals(new Bid(1, Strain.NoTrump)))
            return new OpponentActsAfter1NTState();
        // Unknown openings (1C, 1D, 1H, 1P...)
        return new UnknownState();
    }
}