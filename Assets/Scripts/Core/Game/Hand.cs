using System.Collections.Generic;
using System.Linq;

public class Hand : IHand {
    private static readonly Dictionary<Rank, int> RankToHCP = new() {
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
    private readonly List<Card> _cards = new();
    public int NumberOfCards => _cards.Count;
    public int HCP => _cards.Sum(card => RankToHCP[card.Rank]);
    public List<Card> Cards => _cards;
    public bool HasCard(Card card) => _cards.Contains(card);
    public void AddCard(Card card) => _cards.Add(card);
    public void AddCards(List<Card> cards) => _cards.AddRange(cards);
    public void RemoveCard(Card card) => _cards.Remove(card);
    public int NumberOfCardsOfSuit(Suit suit) => _cards.Count(card => card.Suit == suit);
    public int NumberOfCardsOfRank(Rank rank) => _cards.Count(card => card.Rank == rank);
    public bool HasCardOfSuit(Suit suit) => _cards.Any(card => card.Suit == suit);
    public List<Card> GetCardsOfSuit(Suit suit) => _cards.Where(card => card.Suit == suit).ToList();
    public bool IsBalanced => HandUtils.IsBalancedHand(this);
}   