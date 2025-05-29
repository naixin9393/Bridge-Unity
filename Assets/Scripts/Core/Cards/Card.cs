public record Card(Rank Rank, Suit Suit) {
    public override string ToString() => $"{Rank.ToSymbol()}{Suit.ToSymbol()}";
    public int HCP => Rank switch {
        Rank.Ace => 4,
        Rank.King => 3,
        Rank.Queen => 2,
        Rank.Jack => 1,
        _ => 0
    };
}