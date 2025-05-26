using System;

public class PartnerRespondTo1NTState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        var calls = biddingContext.Calls;
        if (calls.Count == 0)
            throw new Exception("No calls in bidding context");
        if (calls[^1].Type != CallType.Pass)
            // Opponent intervened to 1NT, just pass
            return new BiddingSuggestion(
                message: BiddingMessages.OneNTInterventionPass,
                call: new Pass(null)
            );
        
        var hand = biddingContext.Hand;
        int HCP = HandUtils.CalculateHighCardPoints(hand);

        // 0-7 HCP -> Pass
        if (HCP <= 7)
            return new BiddingSuggestion(
                message: BiddingMessages.OneNTResponsePass(HCP),
                call: new Pass(null));

        // Stayman 8+ HCP and contains 4 major cards
        if (HandUtils.Contains4MajorCards(hand))
            return new BiddingSuggestion(
                message: BiddingMessages.OneNTResponse2Clubs(HCP),
                call: new BidCall(new Bid(2, Strain.Clubs), null));

        // No 4 major cards
        // 8-9 HCP -> 2NT
        if (HCP < 10)
            return new BiddingSuggestion(
                message: BiddingMessages.OneNTResponse2NT(HCP),
                call: new BidCall(new Bid(2, Strain.NoTrump), null));

        // 10+ HCP -> 3NT
        return new BiddingSuggestion(
            message: BiddingMessages.OneNTResponse3NT(HCP),
            call: new BidCall(new Bid(3, Strain.NoTrump), null));
    }

    public IBiddingState GetNextState(ICall call) {
        return new OpponentActsAfterPartner1NTResponseState();
    }
}