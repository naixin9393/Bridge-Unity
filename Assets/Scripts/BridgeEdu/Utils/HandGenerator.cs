using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BridgeEdu.Core;

namespace BridgeEdu.Utils {

    public class HandGenerator {
        private static readonly int[][] BalancedDistributions = new int[][] {
        new int [] { 4, 3, 3, 3 },
        new int [] { 4, 4, 3, 2 },
        new int [] { 5, 3, 3, 2 },
    };

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
            var randomNumber = new System.Random();
            if (balancedHand && !HandUtils.ContainsBalancedHand(deck.Cards.ToList())) throw new Exception("Balanced hand not possible");
            if (deck.Cards.Count < 13) throw new Exception("Deck is too small");
            if (minHCP > maxHCP || minHCP > 37) throw new Exception("HCP range is invalid");

            if (!balancedHand) return GenerateUnbalanced(deck, minHCP, maxHCP);

            int iter = 0;

            while (true) {
                iter++;
                IHand hand = new Hand();
                List<Card> remainingCards = new(deck.Cards);
                int[] distribution = BalancedDistributions[randomNumber.Next(0, BalancedDistributions.Length)];

                List<Suit> shuffledSuits = new(Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList());
                shuffledSuits.Shuffle();

                Dictionary<Suit, int> targetSuitLengths = new();
                for (int i = 0; i < shuffledSuits.Count; i++)
                    targetSuitLengths.Add(shuffledSuits[i], distribution[i]);

                List<Card> currentAttemptCards = new();

                List<Card> honorCards = remainingCards.Where(c => c.HCP > 0).ToList();
                List<Card> nonHonorCards = remainingCards.Where(c => c.HCP == 0).ToList();

                honorCards.Shuffle();
                nonHonorCards.Shuffle();

                int currentHCP = 0;

                foreach (var card in honorCards) {
                    if (currentHCP + card.HCP <= maxHCP &&
                        currentAttemptCards.Count(c => c.Suit == card.Suit) < targetSuitLengths[card.Suit] &&
                        currentAttemptCards.Count < 13) {
                        currentHCP += card.HCP;
                        currentAttemptCards.Add(card);
                    }
                    if (currentHCP >= minHCP && currentAttemptCards.Count >= 7)
                        break;
                }
                List<Card> allRemainingSorted = new();
                allRemainingSorted.AddRange(honorCards.Except(currentAttemptCards)); // Add remaining honors
                allRemainingSorted.AddRange(nonHonorCards);
                allRemainingSorted.Shuffle();

                foreach (var card in allRemainingSorted) {
                    if (currentAttemptCards.Count == 13) break; // Hand is full

                    // Check if adding this card would violate suit length or exceed maxHCP
                    if (currentAttemptCards.Count(c => c.Suit == card.Suit) < targetSuitLengths[card.Suit]) {
                        // Only add if it doesn't push HCP over max, OR if we NEED to add a card to reach 13
                        // and this is the only option without violating suit length.
                        if (currentHCP + card.HCP <= maxHCP || currentAttemptCards.Count < 13) // This condition can be tricky
                        {
                            currentAttemptCards.Add(card);
                            currentHCP += card.HCP;
                        }
                    }
                }

                if (currentAttemptCards.Count == 13) {
                    hand.AddCards(currentAttemptCards); // Assuming PlayerHand has an AddCards method

                    if (hand.HCP >= minHCP && hand.HCP <= maxHCP && hand.IsBalanced) {
                        Debug.Log("Found: " + iter);
                        return hand;
                    }
                }
            }
        }

        private static IHand GenerateUnbalanced(Deck deck, int minHCP, int maxHCP) {
            IHand hand = new Hand();
            // Reach minHCP
            List<Card> remainingCards = new(deck.Cards);

            List<Card> honorCards = remainingCards.Where(c => c.HCP > 0).ToList();
            List<Card> nonHonorCards = remainingCards.Where(c => c.HCP == 0).ToList();
            honorCards.Shuffle();
            nonHonorCards.Shuffle();

            List<Card> currentAttemptCards = new();

            int currentHCP = 0;
            foreach (var card in honorCards) {
                if (currentHCP + card.HCP <= maxHCP &&
                    currentAttemptCards.Count < 13) {
                    currentHCP += card.HCP;
                    currentAttemptCards.Add(card);
                }
                if (currentHCP >= minHCP && currentAttemptCards.Count >= 7)
                    break;
            }

            foreach (var card in nonHonorCards) {
                if (currentAttemptCards.Count == 13) break; // Hand is full
                currentAttemptCards.Add(card);
            }

            hand.AddCards(currentAttemptCards); // Assuming PlayerHand has an AddCards method

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

            if (hand.NumberOfCardsOfSuit(Suit.Spades) >= 5) return false;
            if (hand.NumberOfCardsOfSuit(Suit.Hearts) >= 5) return false;

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
            potentialAdditionalHCP += jacksToDraw;
            return potentialAdditionalHCP;
        }

    }
    public static class ListExtensions {
        private static System.Random _rng = new();
        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = _rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}