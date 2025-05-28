using System.Collections.Generic;

public class NullBiddingEngine : IBiddingEngine {
    public List<BiddingSuggestion> GetBiddingSuggestions(BiddingContext biddingContext) => new();
}