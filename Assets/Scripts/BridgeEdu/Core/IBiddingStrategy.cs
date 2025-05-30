using System.Collections.Generic;

using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IBiddingStrategy {
        bool IsApplicable(BiddingContext biddingContext);
        List<BiddingSuggestion> GetSuggestions(BiddingContext context);
    }
}