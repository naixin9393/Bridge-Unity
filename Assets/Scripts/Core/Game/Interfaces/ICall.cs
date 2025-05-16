public interface ICall {
    CallType Type { get; }
    Player Caller { get; }
}

public enum CallType {
    Bid,
    Pass,
    Double,
    Redouble
}