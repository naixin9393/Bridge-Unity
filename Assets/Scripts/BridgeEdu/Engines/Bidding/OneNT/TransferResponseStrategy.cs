using System.Collections.Generic;
using System.Linq;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

using static BridgeEdu.Engines.Bidding.BiddingMessages.OneNTMessages;

namespace BridgeEdu.Engines.Bidding.OneNT {
    public class TransferResponseStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext context) {
            if (context.Calls.Count < 2) return false;
            if (context.Calls.Where(c => c.Type == CallType.Bid).Count() != 2) return false;
            var lastBidCall = context.Calls.Last(c => c.Type == CallType.Bid) as BidCall;
            if (lastBidCall.Bid.Equals(new Bid(2, Strain.Diamonds))) return true;
            if (lastBidCall.Bid.Equals(new Bid(2, Strain.Hearts))) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new();
            var bidCall = context.Calls.Last(c => c.Type == CallType.Bid) as BidCall;
            var bid = bidCall.Bid;

            // 2H when 2D
            if (bid.Equals(new Bid(2, Strain.Diamonds)))
                suggestions.Add(new BiddingSuggestion(
                    Transfer2DResponse2H,
                    new BidCall(new Bid(2, Strain.Hearts), null)));

            // 2S when 2H
            if (bid.Equals(new Bid(2, Strain.Hearts)))
                suggestions.Add(new BiddingSuggestion(
                    Transfer2HResponse2S,
                    new BidCall(new Bid(2, Strain.Spades), null)));

            return suggestions;
        }
    }
}