public class UnknownState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        return new BiddingSuggestion(
            message: BiddingMessages.Unknown,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return new UnknownState();
    }
}