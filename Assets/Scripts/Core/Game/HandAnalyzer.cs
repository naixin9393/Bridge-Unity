using System.Collections.Generic;
using System.Linq;
using Codice.CM.Client.Gui;

public class HandAnalyzer : IHandAnalyzer {

    private readonly Dictionary<Rank, int> _rankToHCP = new() {
        { Rank.Two, 0},
        { Rank.Three, 0},
        { Rank.Four, 0},
        { Rank.Five, 0},
        { Rank.Six, 0},
        { Rank.Seven, 0},
        { Rank.Eight, 0},
        { Rank.Nine, 0},
        { Rank.Ten, 0},
        { Rank.Jack, 1},
        { Rank.Queen, 2},
        { Rank.King, 3},
        { Rank.Ace, 4}
    };
    
    public int CalculateHighCardPoints(List<Card> hand) {
        int hcp = 0;
        foreach (Card card in hand) {
            if (_rankToHCP.ContainsKey(card.Rank)) {
                hcp += _rankToHCP[card.Rank];
            }
        }
        return hcp;
    }

    public bool ContainsBalancedHand(List<Card> hand) {
        if (hand.Count < 13) return false;
        List<int> numberOfEachSuit = hand.GroupBy(card => card.Suit)
            .Select(group => group.Count())
            .OrderBy(count => count)
            .ToList();
        if (numberOfEachSuit.Zip(new List<int>{2, 3, 3, 5}, (a, b) => a - b).All(diff => diff >= 0)) return true;
        if (numberOfEachSuit.Zip(new List<int>{2, 3, 4, 4}, (a, b) => a - b).All(diff => diff >= 0)) return true;
        if (numberOfEachSuit.Zip(new List<int>{3, 3, 3, 4}, (a, b) => a - b).All(diff => diff >= 0)) return true;
        return false;
    }

    public bool IsBalancedHand(List<Card> hand) {
        if (hand.Count != 13) return false;
        List<int> numberOfEachSuit = hand.GroupBy(card => card.Suit)
            .Select(group => group.Count())
            .ToList();
        if (Is5332(numberOfEachSuit)) return true;
        if (Is4432(numberOfEachSuit)) return true;
        if (Is4333(numberOfEachSuit)) return true;
        return false;
    }

    private bool Is4333(List<int> numberOfEachSuit) {
        return numberOfEachSuit.Count(suit => suit == 3) == 3 &&
               numberOfEachSuit.Count(suit => suit == 4) == 1;
    }

    private bool Is4432(List<int> numberOfEachSuit) {
        return numberOfEachSuit.Count(suit => suit == 4) == 2 &&
               numberOfEachSuit.Count(suit => suit == 3) == 1 &&
               numberOfEachSuit.Count(suit => suit == 2) == 1;
    }

    private bool Is5332(List<int> numberOfEachSuit) {
        return numberOfEachSuit.Count(suit => suit == 5) == 1 &&
               numberOfEachSuit.Count(suit => suit == 3) == 2 &&
               numberOfEachSuit.Count(suit => suit == 2) == 1;
    }
}