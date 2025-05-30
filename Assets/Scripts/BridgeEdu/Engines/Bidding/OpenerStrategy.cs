using System.Collections.Generic;
using System.Linq;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

using static BridgeEdu.Engines.Bidding.BiddingMessages.OpeningMessages;
using static BridgeEdu.Utils.BiddingMessagesUtils;

namespace BridgeEdu.Engines.Bidding {
    public class OpenerStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext context) {
            if (context.Calls.Count == 0) return true;
            if (context.Calls.Where(c => c.Type == CallType.Bid).Count() == 0) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new();

            if (context.Hand.HCP < 12)
                // Can't open with less than 12 HCP
                suggestions.Add(
                    new(message: OpeningPass(context.Hand.HCP),
                    call: new Pass(null)));
            else if (context.Hand.HCP >= 15 && context.Hand.HCP <= 17 && context.Hand.IsBalanced)
                // 15-17 HCP and balanced hand, can open with 1NT
                suggestions.Add(
                    new(message: OpeningOneNT(context.Hand.HCP),
                    call: new BidCall(new Bid(1, Strain.NoTrump), null)));
            else
                suggestions.Add(
                new(message: Unknown,
                call: new Pass(null)));
            return suggestions;
        }
    }
}