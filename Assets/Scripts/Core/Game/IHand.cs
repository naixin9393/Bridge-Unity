using System.Collections.Generic;

public interface IHand {
    int NumberOfCards { get; }
    int HCP { get; }
    List<Card> Cards { get; }

    List<Card> GetCardsOfSuit(Suit suit);

    bool IsBalanced { get; }
    bool HasCard(Card card);
    bool HasCardOfSuit(Suit suit);
    void AddCard(Card card);
    void AddCards(List<Card> cards);
    int NumberOfCardsOfSuit(Suit suit);
    int NumberOfCardsOfRank(Rank rank);
    void RemoveCard(Card card);
}