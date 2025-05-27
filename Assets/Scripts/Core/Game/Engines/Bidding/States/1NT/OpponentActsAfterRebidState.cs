public class OpponentActsAfterRebidState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        // No intervention
        return new BiddingSuggestion(
            message: BiddingMessages.Unknown,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return new PartnerRespondTo1NTRebidState();
    }
}