using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneNTOpenerResponseStrategy : IBiddingStrategy {
    public bool IsApplicable(BiddingContext context) {
        if (context.Calls.Count < 4) return false;
        if (context.Calls.Where(c => c.Type == CallType.Bid).Count() < 2) return false;
        if (context.Calls[^2].Type != CallType.Bid) return false;
        var partnerCall = context.Calls.Where(c => c.Type == CallType.Bid).Last() as BidCall;
        if (partnerCall.Bid.Strain == Strain.Clubs) return false;
        return true;
    }

    public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
        // Responses to 2NT, 4NT and 6NT
        var partnerBidCall = context.Calls.Where(c => c.Type == CallType.Bid).Last() as BidCall;
        var partnerBid = partnerBidCall.Bid;
        List<BiddingSuggestion> suggestions = new();

        // Response to 2NT
        if (context.Hand.HCP >= 15 && context.Hand.HCP <= 16 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(2, Strain.NoTrump)))
            // Pass when 15-16 HCP and balanced hand
            suggestions.Add(
                    new(message: BiddingMessages.OneNTRebidPass(context.Hand.HCP),
                    call: new Pass(null)));
        
        if (context.Hand.HCP == 17 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(2, Strain.NoTrump)))
            // 3NT when 17 HCP and balanced hand
            suggestions.Add(
                    new(message: BiddingMessages.OneNTRebid3NT(context.Hand.HCP),
                    call: new BidCall(new Bid(3, Strain.NoTrump), null)));

        // Response to 3NT
        if (partnerBid.Equals(new Bid(3, Strain.NoTrump)))
            suggestions.Add(
                    new(message: BiddingMessages.AuctionConcludedPass,
                    call: new Pass(null)));
        
        // Response to 4NT
        if (context.Hand.HCP >= 15 && context.Hand.HCP <= 16 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(4, Strain.NoTrump)))
            // Pass auction concluded
            suggestions.Add(
                    new(message: BiddingMessages.OneNTRebidPass2(context.Hand.HCP),
                    call: new Pass(null)));
        
        if (context.Hand.HCP == 17 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(4, Strain.NoTrump)))
            // 6NT when 17 HCP and balanced hand
            suggestions.Add(
                    new(message: BiddingMessages.OneNTRebid6NT(context.Hand.HCP),
                    call: new BidCall(new Bid(6, Strain.NoTrump), null)));
        
        // Response to 6NT
        if (context.Hand.IsBalanced && partnerBid.Equals(new Bid(6, Strain.NoTrump)))
            // Pass auction concluded
            suggestions.Add(
                    new(message: BiddingMessages.AuctionConcludedPass,
                    call: new Pass(null)));

        return suggestions;
    }
}