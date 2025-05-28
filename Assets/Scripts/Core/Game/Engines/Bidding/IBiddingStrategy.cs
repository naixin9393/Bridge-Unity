using System.Collections.Generic;

public interface IBiddingStrategy {
    bool IsApplicable(BiddingContext biddingContext);
    List<BiddingSuggestion> GetSuggestions(BiddingContext context);
}