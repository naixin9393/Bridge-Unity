using System.Collections.Generic;

public class BiddingContextBuilder {
    private IHand _hand;
    private List<ICall> _calls = new();
    
    public BiddingContextBuilder WithHand(IHand hand) {
        _hand = hand;
        return this;
    }

    public BiddingContextBuilder WithCalls(List<ICall> calls) {
        _calls = calls;
        return this;
    }

    public BiddingContext Build() {
        return new BiddingContext(_calls, _hand);
    }
}