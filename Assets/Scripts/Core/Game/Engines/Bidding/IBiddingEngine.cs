public interface IBiddingEngine {
    BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext);
    BiddingSuggestion UpdateState(BiddingContext biddingContext);
    void SetState(IBiddingState state);
}