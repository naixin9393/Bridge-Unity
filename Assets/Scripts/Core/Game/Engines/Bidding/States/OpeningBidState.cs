public class OpeningBidState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        var hand = biddingContext.Hand;
        int HCP = HandUtils.CalculateHighCardPoints(hand);
        if (HCP < 12)
            // Can't open with less than 12 HCP
            return new BiddingSuggestion(
                message: BiddingMessages.OpeningPass(HCP),
                call: new Pass(null)
            );
        else if (HCP <= 14)
            // 12-14
            return new BiddingSuggestion(
                message: BiddingMessages.Unknown,
                call: new Pass(null)
            );
        else if (HCP <= 17 && HandUtils.IsBalancedHand(hand))
            // 15-17 HCP and balanced hand, can open with 1NT
            return new BiddingSuggestion(
                message: BiddingMessages.OpeningOneNT(HCP),
                call: new BidCall(new Bid(1, Strain.NoTrump), null)
            );
        return new BiddingSuggestion(
            message: BiddingMessages.Unknown,
            call: new Pass(null));
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