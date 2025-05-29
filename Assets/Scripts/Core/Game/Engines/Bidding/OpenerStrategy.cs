using System.Collections.Generic;
using System.Linq;

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
                new(message: BiddingMessages.OpeningPass(context.Hand.HCP),
                call: new Pass(null)));
        else if (context.Hand.HCP >= 15 && context.Hand.HCP <= 17 && context.Hand.IsBalanced)
            // 15-17 HCP and balanced hand, can open with 1NT
            suggestions.Add(
                new(message: BiddingMessages.OpeningOneNT(context.Hand.HCP),
                call: new BidCall(new Bid(1, Strain.NoTrump), null)));
        else 
            suggestions.Add(
            new(message: BiddingMessages.Unknown,
            call: new Pass(null)));
        return suggestions;
    }
}