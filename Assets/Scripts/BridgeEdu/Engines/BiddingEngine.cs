using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Utils;

namespace BridgeEdu.Engines {
    public class BiddingEngine : IBiddingEngine {
        private List<IBiddingStrategy> _strategies;

        public BiddingEngine(List<IBiddingStrategy> strategies) {
            _strategies = strategies;
        }
        public List<BiddingSuggestion> GetBiddingSuggestions(BiddingContext biddingContext) {
            var suggestions = new List<BiddingSuggestion>();
            foreach (IBiddingStrategy strategy in _strategies) {
                if (strategy.IsApplicable(biddingContext))
                    suggestions.AddRange(strategy.GetSuggestions(biddingContext));
            }
            if (suggestions.Count == 0)
                suggestions.Add(new(message: BiddingMessagesUtils.Unknown, call: new Pass(null)));
            return suggestions;
        }
    }
}