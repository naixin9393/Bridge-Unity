using System.Collections.Generic;

public class BiddingContextBuilder {
    private List<Card> _hand;
    private List<ICall> _calls = new();
    
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
        return new BiddingContext(_calls, _hand);
    }
}