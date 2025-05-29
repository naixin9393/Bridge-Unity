using System.Collections.Generic;
using System.Linq;

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
            new(message: BiddingMessages.NoIntervention,
            call: new Pass(null))
        };
        return suggestions;
    }
}