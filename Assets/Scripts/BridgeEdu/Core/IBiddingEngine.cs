using System.Collections.Generic;

using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IBiddingEngine {
        List<BiddingSuggestion> GetBiddingSuggestions(BiddingContext biddingContext);
    }
}