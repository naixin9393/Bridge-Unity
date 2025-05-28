/*
public class ConcludedState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        // No intervention
        return new BiddingSuggestion(
            message: BiddingMessages.AuctionConcludedPass,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return new ConcludedState();
    }
}
*/