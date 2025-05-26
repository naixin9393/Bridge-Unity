public class BiddingEngine : IBiddingEngine {
    private IBiddingState _currentState;
    public BiddingEngine() {
        _currentState = new OpeningBidState();
    }
    
    public BiddingSuggestion GetBiddingSuggestion(BiddingContext biddingContext) {
        return _currentState.CalculateCall(biddingContext);
    }

    public BiddingSuggestion UpdateState(BiddingContext biddingContext) {
        var suggestion = _currentState.CalculateCall(biddingContext);
        _currentState = _currentState.GetNextState(suggestion.Call);
        return suggestion;
    }

    public void SetState(IBiddingState state) {
        _currentState = state;
    }
}
