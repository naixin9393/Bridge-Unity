using System;
using System.Collections.Generic;
using System.Linq;

public class HandGenerator {
    public static IHand Generate(Deck deck, bool balancedHand) {
        if (!HandUtils.ContainsBalancedHand(deck.Cards.ToList())) throw new Exception("Deck doesnt contain balanced hand");
        if (deck.Cards.Count < 13) throw new Exception("Hand is not full");

        IHand hand = new Hand();

        while (hand.NumberOfCards < 13) {
            Card card = deck.DealCard();
            hand.AddCard(card);

            if (balancedHand && !CouldBeBalancedHand(hand)) {
                hand.RemoveCard(card);
                deck.InsertCard(card);
                deck.Shuffle();
            }
        }

        return hand;
    }

    public static IHand Generate(Deck deck, bool balancedHand, int minHCP, int maxHCP) {
        if (balancedHand && !HandUtils.ContainsBalancedHand(deck.Cards.ToList())) throw new Exception("Balanced hand not possible");
        if (deck.Cards.Count < 13) throw new Exception("Deck is too small");
        if (minHCP > maxHCP || minHCP > 37) throw new Exception("HCP range is invalid");

        IHand hand = new Hand();
        List<Card> discardedCards = new();
        int HCP = -1;
        
        // Reach minHCP and balanced hand if necessary
        while (HCP < minHCP) {
            Card card = deck.DealCard();

            if (!IsHonorCard(card)) {
                discardedCards.Add(card);
                continue;
            }

            hand.AddCard(card);
            HCP = hand.HCP;

            if ((balancedHand && !CouldBeBalancedHand(hand))
                || !CouldReachMinHCP(hand, minHCP)
                || hand.HCP > maxHCP) {
                hand.RemoveCard(card);
                HCP = hand.HCP;
                discardedCards.Add(card);
                deck.Shuffle();
            }
        }

        // Put discarded cards back into deck
        foreach (Card card in discardedCards) {
            deck.InsertCard(card);
        }
        
        discardedCards.Clear();
        
        // Fill hand and respecting constraints
        while (hand.NumberOfCards < 13) {
            Card card = deck.DealCard();
            hand.AddCard(card);
            if (balancedHand && !CouldBeBalancedHand(hand) || hand.HCP > maxHCP) {
                hand.RemoveCard(card);
                discardedCards.Add(card);
                deck.Shuffle();
            }
        }

        // Put discarded cards back into deck
        foreach (Card card in discardedCards) {
            deck.InsertCard(card);
        }
        
        return hand;
    }
    
    private static bool IsHonorCard(Card card) {
        return card.Rank == Rank.Ace || card.Rank == Rank.King || card.Rank == Rank.Queen || card.Rank == Rank.Jack;
    }

    private static bool CouldBeBalancedHand(IHand hand) {
        List<int> numberOfEachSuit = new() {
            hand.NumberOfCardsOfSuit(Suit.Spades),
            hand.NumberOfCardsOfSuit(Suit.Hearts),
            hand.NumberOfCardsOfSuit(Suit.Diamonds),
            hand.NumberOfCardsOfSuit(Suit.Clubs)
        };

        if (numberOfEachSuit.Any(suit => suit > 5)) return false;

        if (numberOfEachSuit.Count(suit => suit == 5) == 0) {
            // Hand is 4432 or 4333
            return numberOfEachSuit.Count(suit => suit == 4) < 3;
        }
        // Hand is 5332
        return numberOfEachSuit.Count(suit => suit == 4) == 0 &&
               numberOfEachSuit.Count(suit => suit == 3) < 3;
    }

    private static bool CouldReachMinHCP(IHand hand, int minHCP) {
        int currentHcp = hand.HCP;
        int HCPNeeded = minHCP - currentHcp;
        if (HCPNeeded <= 0) return true;

        int cardsRemainingToDraw = 13 - hand.NumberOfCards;
        if (cardsRemainingToDraw <= 0) return false;

        int numberOfAcesInDeck = 4 - hand.NumberOfCardsOfRank(Rank.Ace);
        int numberOfKingsInDeck = 4 - hand.NumberOfCardsOfRank(Rank.King);
        int numberOfQueensInDeck = 4 - hand.NumberOfCardsOfRank(Rank.Queen);
        int numberOfJacksInDeck = 4 - hand.NumberOfCardsOfRank(Rank.Jack);

        int potentialAdditionalHCP = PotentialAdditionalHCP(cardsRemainingToDraw, numberOfAcesInDeck, numberOfKingsInDeck, numberOfQueensInDeck, numberOfJacksInDeck);

        return HCPNeeded <= potentialAdditionalHCP;
    }

    private static int PotentialAdditionalHCP(int cardsRemainingToDraw, int numberOfAcesInDeck, int numberOfKingsInDeck, int numberOfQueensInDeck, int numberOfJacksInDeck) {
        int potentialAdditionalHCP = 0;

        int acesToDraw = Math.Min(cardsRemainingToDraw, numberOfAcesInDeck);
        cardsRemainingToDraw -= acesToDraw;
        potentialAdditionalHCP += acesToDraw * 4;

        int kingsToDraw = Math.Min(cardsRemainingToDraw, numberOfKingsInDeck);
        cardsRemainingToDraw -= kingsToDraw;
        potentialAdditionalHCP += kingsToDraw * 3;

        int queensToDraw = Math.Min(cardsRemainingToDraw, numberOfQueensInDeck);
        cardsRemainingToDraw -= queensToDraw;
        potentialAdditionalHCP += queensToDraw * 2;

        int jacksToDraw = Math.Min(cardsRemainingToDraw, numberOfJacksInDeck);
        cardsRemainingToDraw -= jacksToDraw;
        potentialAdditionalHCP += jacksToDraw;
        return potentialAdditionalHCP;
    }
}