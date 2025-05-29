using System.Collections.Generic;
using System.Linq;

public readonly struct BiddingContext {
    public readonly List<ICall> Calls;
    public readonly IHand Hand { get; }
    public readonly Position CurrentPlayerPosition { get; }

    public BiddingContext(List<ICall> calls, IHand hand, Position currentPlayerPosition) {
        Calls = calls;
        Hand = hand;
        CurrentPlayerPosition = currentPlayerPosition;
    }
    public bool MatchesBidSequence(params (int level, Strain strain)[] sequence) {
        if (Calls.Count < sequence.Length) return false;
        var bidCalls = Calls.Where(c => c.Type == CallType.Bid).ToList();
        
        if (bidCalls.Count != sequence.Length) return false;
        
        for (int i = 0; i < sequence.Length; i++) {
            var bidCall = bidCalls[i] as BidCall;
            if (!bidCall.Bid.Equals(new Bid(sequence[i].level, sequence[i].strain)))
                return false;
        }
        return true;
    }
}