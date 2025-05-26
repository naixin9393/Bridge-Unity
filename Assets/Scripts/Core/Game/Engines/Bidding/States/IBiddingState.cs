public interface IBiddingState {
    BiddingSuggestion CalculateCall(BiddingContext biddingContext);
    IBiddingState GetNextState(ICall call);
}