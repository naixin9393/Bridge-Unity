using System;
using System.Collections.Generic;

public class BiddingContextBuilder {
    private List<Card> _hand;
    private List<ICall> _calls;
    
    public BiddingContextBuilder WithHand(List<Card> hand) {
        _hand = hand;
        return this;
    }

    public BiddingContextBuilder WithCalls(List<ICall> calls) {
        _calls = calls;
        return this;
    }

    public BiddingContextBuilder WithBalancedDistribution() {
        _hand = HandGenerator.Generate(new Deck(), true);
        return this;
    }

    public BiddingContext Build() {
        return new BiddingContext(highestBid: GetHighestBid(_calls), _calls, _hand);
    }
    
    private Bid GetHighestBid(List<ICall> calls) {
        if (calls == null || calls.Count == 0) return null;
        for (int i = calls.Count - 1; i >= 0; i--) {
            if (calls[i].Type == CallType.Bid) {
                return (calls[i] as BidCall).Bid;
            }
        }
        return null;
    }
}