public readonly struct PlayingSuggestion {
    public readonly Card Card;
    public readonly string Message;
    public PlayingSuggestion(Card card, string message) {
        Message = message;
        Card = card;
    }
}