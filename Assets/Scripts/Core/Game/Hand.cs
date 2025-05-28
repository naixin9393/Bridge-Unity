using System.Collections.Generic;
using System.Linq;

public class Hand : IHand {
    private readonly List<Card> _cards = new();
    public int NumberOfCards => _cards.Count;
    public int HCP => _cards.Sum(card => HandUtils.RankToHCP[card.Rank]);
    public List<Card> Cards => _cards;
    public bool HasCard(Card card) => _cards.Contains(card);
    public void AddCard(Card card) => _cards.Add(card);
    public void AddCards(List<Card> cards) => _cards.AddRange(cards);
    public void RemoveCard(Card card) => _cards.Remove(card);
    public int NumberOfCardsOfSuit(Suit suit) => _cards.Count(card => card.Suit == suit);
    public int NumberOfCardsOfRank(Rank rank) => _cards.Count(card => card.Rank == rank);
    public bool HasCardOfSuit(Suit suit) => _cards.Any(card => card.Suit == suit);
    public List<Card> GetCardsOfSuit(Suit suit) => _cards.Where(card => card.Suit == suit).ToList();
}   