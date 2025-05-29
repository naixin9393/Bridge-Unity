using System.Collections.Generic;

public class OneNTResponseStrategy : IBiddingStrategy {
    public bool IsApplicable(BiddingContext context) {
        if (context.Calls.Count < 2) return false;
        var partnerCall = context.Calls[^2];
        if (partnerCall.Type != CallType.Bid) return false;
        var partnerBidCall = partnerCall as BidCall;
        if (partnerBidCall.Bid.Strain != Strain.NoTrump) return false;
        if (partnerBidCall.Bid.Level != 1) return false;
        return true;
    }

    public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();
        var hand = context.Hand;

        // Pass when hand is balanced and 0-7 HCP
        if (hand.HCP <= 7 && hand.IsBalanced)
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponsePass(hand.HCP),
                call: new Pass(null)));
        
        // 2C when hand contains 4+ major cards and 8+ HCP (stayman)
        if (hand.HCP >= 8 && HandUtils.Contains4MajorCards(hand))
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponse2Clubs(hand.HCP),
                call: new BidCall(new Bid(2, Strain.Clubs), null)));

        // 2NT when hand is balanced and 8-9 HCP
        if (hand.HCP >= 8 && hand.HCP <= 9 && hand.IsBalanced)
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponse2NT(hand.HCP),
                call: new BidCall(new Bid(2, Strain.NoTrump), null)));
        
        // 3NT when hand is balanced and 10-15 HCP
        if (hand.HCP >= 10 && hand.HCP <= 15 && hand.IsBalanced)
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponse3NT(hand.HCP),
                call: new BidCall(new Bid(3, Strain.NoTrump), null)));
        
        // 4NT when hand is balanced and 16-17 HCP
        if (hand.HCP >= 16 && hand.HCP <= 17 && hand.IsBalanced)
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponse4NT(hand.HCP),
                call: new BidCall(new Bid(4, Strain.NoTrump), null)));
        
        // 6NT when hand is balanced and 18+ HCP
        if (hand.HCP >= 18 && hand.IsBalanced)
            suggestions.Add(
                new(message: BiddingMessages.OneNTResponse6NT(hand.HCP),
                call: new BidCall(new Bid(6, Strain.NoTrump), null)));
        
        return suggestions;
    }
}