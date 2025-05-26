public interface IBiddingEngine {
    BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext);
    void SetState(IBiddingState state);
}