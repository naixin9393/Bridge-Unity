public class Open1NTState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        return new BiddingSuggestion(
            message: BiddingMessages.OneNTInterventionPass,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return call.Type switch {
            CallType.Pass => new Respond1NTState(),
            _ => new UnknownState(),
        };
    }
}