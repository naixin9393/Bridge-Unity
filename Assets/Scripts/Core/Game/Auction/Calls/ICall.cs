public interface ICall {
    CallType Type { get; }
    IPlayer Caller { get; }
}

public enum CallType {
    Bid,
    Pass,
    Double,
    Redouble
}