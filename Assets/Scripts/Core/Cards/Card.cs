public class Card {
    public Rank Rank { get; private set; }
    public Suit Suit { get; private set; }

    public Card(Rank rank, Suit suit) {
        Rank = rank;
        Suit = suit;
    }

    public override bool Equals(object other) {
        if (other == null || GetType() != other.GetType()) return false;
        Card otherCard = (Card)other;
        return Rank == otherCard.Rank && Suit == otherCard.Suit;
    }

    public override int GetHashCode() {
        return 5 * Rank.GetHashCode() + 17 * Suit.GetHashCode();
    }
    public override string ToString() {
        return $"{Rank.ToString().ToLower()} of {Suit.ToString().ToLower()}";
    }
}
