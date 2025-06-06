namespace BridgeEdu.Core {
    public enum Suit { Diamonds, Clubs, Hearts, Spades }

    public static class SuitExtensions {
        public static string ToSymbol(this Suit suit) {
            return suit switch {
                Suit.Clubs => "♣",
                Suit.Diamonds => "♦",
                Suit.Hearts => "♥",
                Suit.Spades => "♠",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}