public readonly struct BiddingSuggestion {
    public readonly string Message;
    public readonly ICall Call;

    public BiddingSuggestion(string message, ICall call) {
        Message = message;
        Call = call;
    }
}