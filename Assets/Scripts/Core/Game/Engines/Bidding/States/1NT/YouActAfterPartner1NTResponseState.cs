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
                    message: BiddingMessages.OneNTRebidNotEnoughHCP,
                    call: new Pass(null)
                );
            // Partner bid (2C, 2NT, 3NT)
            case CallType.Bid:
                bidCall = partnerCall as BidCall;
                bid = bidCall.Bid;

                // Partner bid 2C, stayman
                if (bid.Equals(new Bid(2, Strain.Clubs))) {
                    // Respond with 2D if hand does not contain 4 major cards
                    if (!HandUtils.Contains4MajorCards(hand))
                        return new BiddingSuggestion(
                            message: BiddingMessages.OneNTRebid2Diamonds,
                            call: new BidCall(new Bid(2, Strain.Diamonds), null)
                        );
                    // Respond with 2H if hand contains 4 hearts
                    if (HandUtils.Contains4Hearts(hand))
                        return new BiddingSuggestion(
                            message: BiddingMessages.OneNTRebid2Hearts,
                            call: new BidCall(new Bid(2, Strain.Hearts), null)
                        );
                    // Respond with 2S if hand contains 4 spades and no hearts
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTRebid2Spades,
                        call: new BidCall(new Bid(2, Strain.Spades), null)
                    );
                    
                }
                
                // Partner bid 2NT
                // Respond with pass if HCP 15-16.
                // Respond with 3NT if HCP 17.
                if (bid.Equals(new Bid(2, Strain.NoTrump))) {
                    if (HCP < 17)
                        return new BiddingSuggestion(
                            message: BiddingMessages.OneNTRebidPass(HCP),
                            call: new Pass(null)
                        );
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTRebidGame(HCP),
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