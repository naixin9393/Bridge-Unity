public class BiddingEngine : IBiddingEngine {
    private IBiddingState _currentState;
    public BiddingEngine() {
        _currentState = new OpeningBidState();
    }
    
    public BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext) {
        return _currentState.CalculateCall(biddingContext);
    }

    public void UpdateState(ICall call) {
        _currentState = _currentState.GetNextState(call);
    }

    public void SetState(IBiddingState state) {
        _currentState = state;
    }
}
