public class Pass : ICall {
    public CallType Type => CallType.Pass;
    public Player Caller { get; private set; }

    public Pass(Player caller) {
        Caller = caller;
    }
}

public class Double : ICall {
    public CallType Type => CallType.Double;
    public Player Caller { get; private set; }

    public Double(Player caller) {
        Caller = caller;
    }
}

public class Redouble : ICall {
    public CallType Type => CallType.Redouble;
    public Player Caller { get; private set; }

    public Redouble(Player caller) {
        Caller = caller;
    }
}

public class BidCall : ICall {
    public Bid Bid { get; private set; }
    public CallType Type => CallType.Bid;
    public Player Caller { get; private set; }

    public BidCall(Bid bid, Player caller) {
        Bid = bid;
        Caller = caller;
    }
}