public interface IBiddingEngine {
    BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext);
    void UpdateState(ICall call);
    void SetState(IBiddingState state);
}