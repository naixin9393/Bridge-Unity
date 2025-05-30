using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Engines {
    public class NullBiddingEngine : IBiddingEngine {
        public List<BiddingSuggestion> GetBiddingSuggestions(BiddingContext biddingContext) => new();
    }
}
