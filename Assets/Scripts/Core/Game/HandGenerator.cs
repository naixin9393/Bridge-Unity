using System;
using System.Collections.Generic;
using System.Linq;

public class HandGenerator : IHandGenerator {
    public List<Card> Generate(Deck deck, bool balancedHand) {
        HandAnalyzer handAnalyzer = new();
        if (!handAnalyzer.ContainsBalancedHand(deck.Cards.ToList())) return new List<Card>();
        if (deck.Cards.Count < 13) return new List<Card>();

        List<Card> hand = new();

        while (hand.Count < 13) {
            Card card = deck.DealCard();
            hand.Add(card);

            if (balancedHand && !CouldBeBalancedHand(hand)) {
                hand.Remove(card);
                deck.InsertCard(card);
                deck.Shuffle();
            }
        }

        return hand;
    }

    public List<Card> Generate(Deck deck, bool balancedHand, int minHCP, int maxHCP) {
        HandAnalyzer handAnalyzer = new();
        if (balancedHand && !handAnalyzer.ContainsBalancedHand(deck.Cards.ToList())) throw new Exception("Balanced hand not possible");
        if (deck.Cards.Count < 13) throw new Exception("Deck is too small");
        if (minHCP > maxHCP || minHCP > 37) throw new Exception("HCP range is invalid");

        List<Card> hand = new();
        List<Card> discardedCards = new();
        int HCP = -1;
        
        // Reach minHCP and balanced hand if necessary
        while (HCP < minHCP) {
            Card card = deck.DealCard();

            if (!IsHonorCard(card)) {
                discardedCards.Add(card);
                continue;
            }

            hand.Add(card);
            HCP = handAnalyzer.CalculateHighCardPoints(hand);

            if ((balancedHand && !CouldBeBalancedHand(hand))
                || !CouldReachMinHCP(hand, minHCP)
                || handAnalyzer.CalculateHighCardPoints(hand) > maxHCP) {
                hand.Remove(card);
                HCP = handAnalyzer.CalculateHighCardPoints(hand);
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
        while (hand.Count < 13) {
            Card card = deck.DealCard();
            hand.Add(card);
            if (balancedHand && !CouldBeBalancedHand(hand) || handAnalyzer.CalculateHighCardPoints(hand) > maxHCP) {
                hand.Remove(card);
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
    
    private bool IsHonorCard(Card card) {
        return card.Rank == Rank.Ace || card.Rank == Rank.King || card.Rank == Rank.Queen || card.Rank == Rank.Jack;
    }

    private bool CouldBeBalancedHand(List<Card> hand) {
        List<int> numberOfEachSuit = hand.GroupBy(card => card.Suit)
            .Select(group => group.Count())
            .OrderBy(count => count)
            .ToList();

        if (numberOfEachSuit.Any(suit => suit > 5)) return false;

        if (numberOfEachSuit.Count(suit => suit == 5) == 0) {
            // Hand is 4432 or 4333
            return numberOfEachSuit.Count(suit => suit == 4) < 3;
        }
        // Hand is 5332
        return numberOfEachSuit.Count(suit => suit == 4) == 0 &&
               numberOfEachSuit.Count(suit => suit == 3) < 3;
    }

    private bool CouldReachMinHCP(List<Card> hand, int minHCP) {
        HandAnalyzer handAnalyzer = new();
        int currentHcp = handAnalyzer.CalculateHighCardPoints(hand);
        int HCPNeeded = minHCP - currentHcp;
        if (HCPNeeded <= 0) return true;

        int cardsRemainingToDraw = 13 - hand.Count;
        if (cardsRemainingToDraw <= 0) return false;

        int numberOfAcesInDeck = 4 - hand.Count(card => card.Rank == Rank.Ace);
        int numberOfKingsInDeck = 4 - hand.Count(card => card.Rank == Rank.King);
        int numberOfQueensInDeck = 4 - hand.Count(card => card.Rank == Rank.Queen);
        int numberOfJacksInDeck = 4 - hand.Count(card => card.Rank == Rank.Jack);

        int potentialAdditionalHCP = PotentialAdditionalHCP(cardsRemainingToDraw, numberOfAcesInDeck, numberOfKingsInDeck, numberOfQueensInDeck, numberOfJacksInDeck);

        return HCPNeeded <= potentialAdditionalHCP;
    }

    private int PotentialAdditionalHCP(int cardsRemainingToDraw, int numberOfAcesInDeck, int numberOfKingsInDeck, int numberOfQueensInDeck, int numberOfJacksInDeck) {
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