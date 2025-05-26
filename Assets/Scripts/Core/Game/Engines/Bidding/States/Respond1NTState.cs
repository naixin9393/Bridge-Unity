public class Respond1NTState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
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
        return new Open1NTState();
    }
}