using System.Collections.Generic;

public interface IBiddingEngine {
    List<BiddingSuggestion> GetBiddingSuggestions(BiddingContext biddingContext);
}