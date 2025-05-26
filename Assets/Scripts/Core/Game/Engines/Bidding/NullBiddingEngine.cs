public class NullBiddingEngine : IBiddingEngine {
    public BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext) {
        return new BiddingSuggestion(message: BiddingMessages.Unknown, call: new Pass(null));
    }

    public BiddingSuggestion UpdateState(BiddingContext biddingContext) {
        return new BiddingSuggestion(message: BiddingMessages.Unknown, call: new Pass(null));
    }

    public void SetState(IBiddingState state) { }
}