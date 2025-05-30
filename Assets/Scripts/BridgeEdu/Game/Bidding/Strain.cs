using System;

namespace BridgeEdu.Game.Bidding {
    public enum Strain { Clubs, Diamonds, Hearts, Spades, NoTrump }
    public static class StrainExtensions {
        public static string ToSymbol(this Strain strain) {
            return strain switch {
                Strain.Clubs => "♣",
                Strain.Diamonds => "♦",
                Strain.Hearts => "♥",
                Strain.Spades => "♠",
                Strain.NoTrump => "NT",
                _ => throw new ArgumentException("Invalid strain"),
            };
        }
    }
}