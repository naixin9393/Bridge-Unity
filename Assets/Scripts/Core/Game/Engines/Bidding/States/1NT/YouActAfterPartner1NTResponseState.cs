using System;

public class YouActAfterPartner1NTResponseState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        var calls = biddingContext.Calls;
        if (calls.Count == 0)
            throw new Exception("No calls in bidding context");
        if (calls[^1].Type != CallType.Pass)
            // Opponent intervened, just pass
            return new BiddingSuggestion(
                message: BiddingMessages.OneNTInterventionPass,
                call: new Pass(null)
            );
        
        var partnerCall = calls[^2];
        BidCall bidCall;
        Bid bid;
        var hand = biddingContext.Hand;
        int HCP = HandUtils.CalculateHighCardPoints(hand);
        
        switch (partnerCall.Type) {
            // Partner passed, not enough HCP to support 1NT
            case CallType.Pass:
                return new BiddingSuggestion(
                    message: BiddingMessages.OneNTPartnerNotEnoughHCP,
                    call: new Pass(null)
                );
            // Partner bid (2C, 2NT, 3NT)
            case CallType.Bid:
                bidCall = partnerCall as BidCall;
                bid = bidCall.Bid;
                
                // Partner bid 2C, stayman
                if (bid.Equals(new Bid(2, Strain.Clubs)))
                    throw new Exception("Not implemented stayman");
                
                // Partner bid 2NT
                // Respond with pass if HCP 15-16.
                // Respond with 3NT if HCP 17.
                if (bid.Equals(new Bid(2, Strain.NoTrump))) {
                    if (HCP < 17)
                        return new BiddingSuggestion(
                            message: BiddingMessages.OneNTPartner2NT(HCP),
                            call: new Pass(null)
                        );
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTPartner3NT(HCP),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)
                    );
                }
                break;
        }
        return new BiddingSuggestion(
            message: BiddingMessages.OneNTInterventionPass,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return new UnknownState();
    }
}