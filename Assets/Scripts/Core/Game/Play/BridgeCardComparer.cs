using System;
using System.Collections.Generic;

public class BridgeCardComparer : IComparer<Card> {
    private readonly Suit _leadSuit;
    private readonly Strain _contractStrain;
    public BridgeCardComparer(Suit leadSuit, Strain contractStrain) {
        _leadSuit = leadSuit;
        _contractStrain = contractStrain;
    }

    public int Compare(Card x, Card y) {
        if (_contractStrain == Strain.NoTrump) {
            if (BothFollowsLead(x, y))
                return x.Rank.CompareTo(y.Rank);
            if (OneFollowsLead(x, y))
                return x.Suit == _leadSuit ? 1 : -1;
            // Comparison for display, not relevant for play
            if (BothSameSuit(x, y))
                return x.Rank.CompareTo(y.Rank);
            return x.Suit.CompareTo(y.Suit);
        }
        if (BothTrump(x, y))
            return x.Rank.CompareTo(y.Rank);
        if (OneTrump(x, y)) {
            Suit trumpSuit = GetTrumpSuit(_contractStrain);
            return x.Suit == trumpSuit ? 1 : -1;
        }
        // Comparison for display, not relevant for play
        if (BothSameSuit(x, y))
            return x.Rank.CompareTo(y.Rank);
        return x.Suit.CompareTo(y.Suit);
    }

    private bool BothTrump(Card x, Card y) {
        Suit trumpSuit = GetTrumpSuit(_contractStrain);
        return x.Suit == trumpSuit && y.Suit == trumpSuit;
    }

    private bool BothFollowsLead(Card card1, Card card2) {
        return card1.Suit == _leadSuit && card2.Suit == _leadSuit;
    }

    private bool OneFollowsLead(Card card1, Card card2) {
        return card1.Suit == _leadSuit || card2.Suit == _leadSuit;
    }

    private bool BothSameSuit(Card card1, Card card2) {
        return card1.Suit == card2.Suit;
    }

    private Suit GetTrumpSuit(Strain strain) {
        return strain switch {
            Strain.NoTrump => throw new InvalidOperationException("NoTrump is not a trump strain"),
            Strain.Spades => Suit.Spades,
            Strain.Hearts => Suit.Hearts,
            Strain.Diamonds => Suit.Diamonds,
            Strain.Clubs => Suit.Clubs,
            _ => throw new InvalidOperationException("Unknown strain")
        };
    }

    private bool OneTrump(Card x, Card y) {
        Suit trumpSuit = GetTrumpSuit(_contractStrain);
        return x.Suit == trumpSuit || y.Suit == trumpSuit;
    }
}