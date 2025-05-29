using System.Collections.Generic;

public class StaymanSecondResponseStrategy : IBiddingStrategy {
    public bool IsApplicable(BiddingContext context) {
        if (context.Calls.Count < 8) return false;
        var partnerCall = context.Calls[^2];
        if (partnerCall.Type != CallType.Bid) return false;
        foreach (var sequence in StaymanSecondResponseSequences) {
            if (context.MatchesBidSequence(sequence))
                return true;
        }
        return false;
    }

    public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();

        foreach (var suggestion in StaymanDiamondBranchResponses(context))
            suggestions.Add(suggestion);
        
        foreach (var suggestion in StaymanHeartBranchResponses(context))
            suggestions.Add(suggestion);
        
        foreach (var suggestion in StaymanSpadeBranchResponses(context))
            suggestions.Add(suggestion);

        return suggestions;
    }

    private List<BiddingSuggestion> StaymanSpadeBranchResponses(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();
        var bidCallBranch = context.Calls[^4] as BidCall;
        bool isSpadeBranch = bidCallBranch.Bid.Strain == Strain.Spades;
        if (!isSpadeBranch) return suggestions;

        var partnerCall = context.Calls[^2];
        if (partnerCall.Type != CallType.Bid) return suggestions;
        
        int hcp = context.Hand.HCP;
        IHand hand = context.Hand;
        
        var partnerBid = (partnerCall as BidCall).Bid;
        
        bool is3SCall = partnerBid.Equals(new Bid(3, Strain.Spades));
        bool is4SCall = partnerBid.Equals(new Bid(4, Strain.Spades));
        bool is2NTCall = partnerBid.Equals(new Bid(2, Strain.NoTrump));
        bool is3NTCall = partnerBid.Equals(new Bid(3, Strain.NoTrump));
        
        int totalPointsSpades = HandUtils.CalculateTotalPoints(hand, Suit.Spades);
        bool isFitSpades = HandUtils.Contains4Spades(hand);
        
        // Response to 3S
        // pass when 15-16 TP
        if (isFitSpades && is3SCall && totalPointsSpades >= 15 && totalPointsSpades <= 16)
                suggestions.Add(new(message: BiddingMessages.Stayman2S3SSecondResponsePass(totalPointsSpades),
                    call: new Pass(null)));
        
        if (isFitSpades && is3SCall && totalPointsSpades == 17)
            suggestions.Add(new(message: BiddingMessages.Stayman2S3SSecondResponse4S(totalPointsSpades),
                call: new BidCall(new Bid(4, Strain.Spades), null)));
        
        // Response to 4S
        if (is4SCall)
            suggestions.Add(new(message: BiddingMessages.AuctionConcludedPass,
                call: new Pass(null)));

        // Response to 2NT
        if (is2NTCall) {
            if (hcp >= 15 && hcp <= 16)
                suggestions.Add(new(message: BiddingMessages.Stayman2S2NTSecondResponsePass(hcp),
                    call: new Pass(null)));
            if (hcp == 17)
                suggestions.Add(new(message: BiddingMessages.Stayman2S2NTSecondResponse3NT(hcp),
                    call: new BidCall(new Bid(3, Strain.NoTrump), null)));
        }
        
        // Response to 3NT
        if (is3NTCall) {
            if (hcp >= 15 && hcp <= 17)
                suggestions.Add(new(message: BiddingMessages.AuctionConcludedPass,
                        call: new Pass(null)));
        }

        return suggestions;
    }

    private IEnumerable<BiddingSuggestion> StaymanHeartBranchResponses(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();
        var bidCallBranch = context.Calls[^4] as BidCall;
        bool isHeartBranch = bidCallBranch.Bid.Strain == Strain.Hearts;
        if (!isHeartBranch) return suggestions;

        var partnerCall = context.Calls[^2];
        if (partnerCall.Type != CallType.Bid) return suggestions;
        
        int hcp = context.Hand.HCP;
        IHand hand = context.Hand;
        
        var partnerBid = (partnerCall as BidCall).Bid;

        bool is3HCall = partnerBid.Equals(new Bid(3, Strain.Hearts));
        bool is4HCall = partnerBid.Equals(new Bid(4, Strain.Hearts));
        bool is2NTCall = partnerBid.Equals(new Bid(2, Strain.NoTrump));
        bool is3NTCall = partnerBid.Equals(new Bid(3, Strain.NoTrump));
        
        int totalPointsHearts = HandUtils.CalculateTotalPoints(hand, Suit.Hearts); 
        int totalPointsSpades = HandUtils.CalculateTotalPoints(hand, Suit.Spades);
        bool isFitHearts = HandUtils.Contains4Hearts(hand);
        bool isFitSpades = HandUtils.Contains4Spades(hand);
        

        // Response to 3H
        // pass when 15-16 TP
        if (isFitHearts && is3HCall && totalPointsHearts >= 15 && totalPointsHearts <= 16)
                suggestions.Add(new(message: BiddingMessages.Stayman2H3HSecondResponsePass(totalPointsHearts),
                    call: new Pass(null)));
        
        // 4H when 17 TP
        if (isFitHearts && is3HCall && totalPointsHearts == 17)
            suggestions.Add(new(message: BiddingMessages.Stayman2H3HSecondResponse4H(totalPointsHearts),
                call: new BidCall(new Bid(4, Strain.Hearts), null)));
        
        // Response to 4H
        if (is4HCall)
            suggestions.Add(new(message: BiddingMessages.AuctionConcludedPass,
                call: new Pass(null)));

        // Response to 2NT partner has 4 spades
        if (is2NTCall) {
            if (!isFitSpades) {
                if (hcp >= 15 && hcp <= 16)
                    suggestions.Add(new(message: BiddingMessages.Stayman2H2NTSecondResponsePass(hcp),
                        call: new Pass(null)));
                if (hcp == 17)
                    suggestions.Add(new(message: BiddingMessages.Stayman2H2NTSecondResponse3NT(hcp),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)));
            }
            else {
                // 3S when 15 to 16 TP
                if (totalPointsSpades >= 15 && totalPointsSpades <= 16)
                    suggestions.Add(new(message: BiddingMessages.Stayman2H2NTSecondResponse3S(totalPointsSpades),
                        call: new BidCall(new Bid(3, Strain.Spades), null)));
                
                // 4S when 17 TP
                if (totalPointsSpades == 17)
                    suggestions.Add(new(message: BiddingMessages.Stayman2H2NTSecondResponse4S(totalPointsSpades),
                        call: new BidCall(new Bid(4, Strain.Spades), null)));
            }  
        }

        if (is3NTCall) {
            
            // 4S when 15 to 17 TP
            if (isFitSpades && totalPointsSpades >= 15 && totalPointsSpades <= 17)
                suggestions.Add(new(message: BiddingMessages.Stayman2H3NTSecondResponse4S(totalPointsSpades),
                    call: new BidCall(new Bid(4, Strain.Spades), null)));
        
            if (!isFitSpades && hcp >= 15 && hcp <= 17)
                suggestions.Add(new(message: BiddingMessages.Stayman2H3NTSecondResponsePass(hcp),
                        call: new Pass(null)));
        }


        return suggestions;
    }

    private IEnumerable<BiddingSuggestion> StaymanDiamondBranchResponses(BiddingContext context) {
        List<BiddingSuggestion> suggestions = new();
        var bidCallBranch = context.Calls[^4] as BidCall;
        bool isDiamondBranch = bidCallBranch.Bid.Strain == Strain.Diamonds;
        if (!isDiamondBranch) return suggestions;
        
        var partnerCall = context.Calls[^2];
        if (partnerCall.Type != CallType.Bid) return suggestions;
        
        int hcp = context.Hand.HCP;

        var partnerBid = partnerCall as BidCall;
        
        bool is2NTCall = partnerBid.Bid.Equals(new Bid(2, Strain.NoTrump));
        bool is3NTCall = partnerBid.Bid.Equals(new Bid(3, Strain.NoTrump));

        if (is2NTCall && hcp >= 15 && hcp <= 16)
            suggestions.Add(new(message: BiddingMessages.Stayman2D2NTSecondResponsePass(hcp),
                call: new Pass(null)));
        
        if (is2NTCall && hcp == 17)
            suggestions.Add(new(message: BiddingMessages.Stayman2D2NTSecondResponse3NT(hcp),
                call: new BidCall(new Bid(3, Strain.NoTrump), null)));

        if (is3NTCall)
            suggestions.Add(new(message: BiddingMessages.AuctionConcludedPass,
                call: new Pass(null)));

        return suggestions;
    }

    private List<(int, Strain)[]> StaymanSecondResponseSequences = new() {
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Diamonds), (2, Strain.NoTrump) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Diamonds), (3, Strain.NoTrump) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Hearts), (3, Strain.Hearts) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Hearts), (4, Strain.Hearts) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Hearts), (2, Strain.NoTrump) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Hearts), (3, Strain.NoTrump) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Spades), (3, Strain.Spades) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Spades), (4, Strain.Spades) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Spades), (2, Strain.NoTrump) },
        new [] { (1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Spades), (3, Strain.NoTrump) },
    };
}