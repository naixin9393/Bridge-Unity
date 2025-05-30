using BridgeEdu.Core;

namespace BridgeEdu.Game.Bidding {
    public record Pass : ICall {
        public CallType Type => CallType.Pass;
        public IPlayer Caller { get; private set; }

        public Pass(IPlayer caller) {
            Caller = caller;
        }

        public override string ToString() => "PASS";
    }

    public record Double : ICall {
        public CallType Type => CallType.Double;
        public IPlayer Caller { get; private set; }

        public Double(IPlayer caller) {
            Caller = caller;
        }

        public override string ToString() => "X";
    }

    public record Redouble : ICall {
        public CallType Type => CallType.Redouble;
        public IPlayer Caller { get; private set; }

        public Redouble(IPlayer caller) {
            Caller = caller;
        }

        public override string ToString() => "XX";
    }

    public record BidCall : ICall {
        public Bid Bid { get; private set; }
        public CallType Type => CallType.Bid;
        public IPlayer Caller { get; private set; }

        public BidCall(Bid bid, IPlayer caller) {
            Bid = bid;
            Caller = caller;
        }
    }
}