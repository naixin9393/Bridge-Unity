using System.Collections.Generic;
using System.Linq;

public class StaymanResponseStrategy : IBiddingStrategy {
    public bool IsApplicable(BiddingContext context) {
        if (context.Calls.Count < 4) return false;
        if (context.Calls.Where(c => c.Type == CallType.Bid).Count() != 2) return false;
        var firstBidCall = context.Calls.Where(c => c.Type == CallType.Bid).First() as BidCall;
        if (!firstBidCall.Bid.Equals(new Bid(1, Strain.NoTrump))) return false;
        var secondBidCall = context.Calls.Where(c => c.Type == CallType.Bid).Skip(1).First() as BidCall;
        if (!secondBidCall.Bid.Equals(new Bid(2, Strain.Clubs))) return false;
        var lastBidCall = context.Calls.Where(c => c.Type == CallType.Bid).Last() as BidCall;
        if (!lastBidCall.Bid.Equals(new Bid(2, Strain.Clubs))) return false;
        if (context.Hand.HCP < 8) return false; // Minimum HCP for Stayman
        return true;
    }

    public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();
        IHand hand = context.Hand;
        
        // 2D if no 4+ major cards
        if (!HandUtils.Contains4MajorCards(hand))
            suggestions.Add(new(message: BiddingMessages.StaymanResponse2D,
                call: new BidCall(new Bid(2, Strain.Diamonds), null)));
        
        // 2H if 4+ hearts
        if (HandUtils.Contains4Hearts(hand))
            suggestions.Add(new(message: BiddingMessages.StaymanResponse2H,
                call: new BidCall(new Bid(2, Strain.Hearts), null)));
        
        // 2S if 4+ spades and no 4+ hearts
        if (HandUtils.Contains4Spades(hand) && !HandUtils.Contains4Hearts(hand))
            suggestions.Add(new(message: BiddingMessages.StaymanResponse2S,
                call: new BidCall(new Bid(2, Strain.Spades), null)));

        return suggestions;
    }
}