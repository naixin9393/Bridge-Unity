public class Card {
    public enum CardRank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    public CardRank Rank { get; private set; }
    public Suit Suit { get; private set; }

    public Card(CardRank rank, Suit suit) {
        Rank = rank;
        Suit = suit;
    }

    public int CompareTo(Card other, Suit trumpSuit) {
        if (Suit == other.Suit) {
            return Rank.CompareTo(other.Rank);
        }
        if (Suit == trumpSuit) return 1;
        if (other.Suit == trumpSuit) return -1;
        return Suit.CompareTo(other.Suit);
    }
}
