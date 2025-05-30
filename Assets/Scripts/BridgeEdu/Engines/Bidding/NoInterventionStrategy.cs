using System.Collections.Generic;
using System.Linq;

using BridgeEdu.Core;
using BridgeEdu.Utils;
using BridgeEdu.Game.Bidding;

using static BridgeEdu.Engines.Bidding.BiddingMessages.OneNTMessages;

namespace BridgeEdu.Engines.Bidding {
    public class NoInterventionStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext context) {
            if (context.Calls.Count == 0)
                return false;
            var calls = context.Calls;
            if (calls.Where(c => c.Type == CallType.Bid).Count() == 0) {
                return false;
            }
            var firstCall = calls.Where(c => c.Type == CallType.Bid).First();
            IPlayer firstCaller = firstCall.Caller;
            return PlayerUtils.OnDifferentTeam(firstCaller.Position, context.CurrentPlayerPosition);
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new() {
            new(message: NoIntervention,
            call: new Pass(null))
        };
            return suggestions;
        }
    }
}