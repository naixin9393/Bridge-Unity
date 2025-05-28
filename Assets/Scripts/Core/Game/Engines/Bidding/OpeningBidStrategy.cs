using System.Collections.Generic;

public class OpeningBidStrategy : IBiddingStrategy {
    public bool IsApplicable(BiddingContext biddingContext) {
        return true;
    }

    public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
        return new List<BiddingSuggestion> {
            new(message: BiddingMessages.OpeningOneNT(16),
                new BidCall(bid: new Bid(1, Strain.NoTrump), caller: null)
        )};
    }
}