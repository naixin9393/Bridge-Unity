using System;
using System.Collections.Generic;
using System.Linq;

using BridgeEdu.Core;

namespace BridgeEdu.Utils {
    public static class HandUtils {
        public static readonly Dictionary<Rank, int> RankToHCP = new() {
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

        public static bool ContainsBalancedHand(List<Card> hand) {
            if (hand.Count < 13) return false;
            List<int> numberOfEachSuit = hand.GroupBy(card => card.Suit)
                .Select(group => group.Count())
                .OrderBy(count => count)
                .ToList();
            if (numberOfEachSuit.Zip(new List<int> { 2, 3, 3, 5 }, (a, b) => a - b).All(diff => diff >= 0)) return true;
            if (numberOfEachSuit.Zip(new List<int> { 2, 3, 4, 4 }, (a, b) => a - b).All(diff => diff >= 0)) return true;
            if (numberOfEachSuit.Zip(new List<int> { 3, 3, 3, 4 }, (a, b) => a - b).All(diff => diff >= 0)) return true;
            return false;
        }

        public static bool IsBalancedHand(IHand hand) {
            if (hand.NumberOfCards != 13) return false;
            if (hand.NumberOfCardsOfSuit(Suit.Spades) >= 5) return false;
            if (hand.NumberOfCardsOfSuit(Suit.Hearts) >= 5) return false;

            var numberOfEachSuit = new List<int>() {
            hand.NumberOfCardsOfSuit(Suit.Spades),
            hand.NumberOfCardsOfSuit(Suit.Hearts),
            hand.NumberOfCardsOfSuit(Suit.Diamonds),
            hand.NumberOfCardsOfSuit(Suit.Clubs)
        };

            bool hasVoid = numberOfEachSuit.Any(suitCount => suitCount == 0);
            bool hasSingleton = numberOfEachSuit.Any(suitCount => suitCount == 1);
            int doubletonCount = numberOfEachSuit.Count(suitCount => suitCount == 2);
            int maxSuitLength = numberOfEachSuit.Max();

            return !hasVoid && !hasSingleton && doubletonCount <= 1 && maxSuitLength <= 5;
        }

        public static int CalculateTotalPoints(IHand hand, Suit fittingSuit) {
            return hand.HCP + CalculateDP(hand, fittingSuit);
        }

        private static int CalculateDP(IHand hand, Suit fittingSuit) {
            int dp = 0;
            int numberOfCardsOfSuit = hand.NumberOfCardsOfSuit(fittingSuit);

            // longsuit points
            dp += numberOfCardsOfSuit - 4;

            var numberOfCardsOfEachSuit = new List<int>() {
            hand.NumberOfCardsOfSuit(Suit.Spades),
            hand.NumberOfCardsOfSuit(Suit.Hearts),
            hand.NumberOfCardsOfSuit(Suit.Diamonds),
            hand.NumberOfCardsOfSuit(Suit.Clubs)
        };

            dp += numberOfCardsOfEachSuit.Count(n => n == 0) * 3;

            // singleton doubleton points
            foreach (var suitCardCount in numberOfCardsOfEachSuit) {
                if (suitCardCount == 1) dp += 2;
                else if (suitCardCount == 2) dp += 1;
            }

            return dp;
        }

        public static bool Contains4Hearts(IHand hand) {
            return hand.NumberOfCardsOfSuit(Suit.Hearts) >= 4;
        }

        public static bool Contains4Spades(IHand hand) {
            return hand.NumberOfCardsOfSuit(Suit.Spades) >= 4;
        }

        public static bool Contains4MajorCards(IHand hand) {
            return hand.NumberOfCardsOfSuit(Suit.Spades) >= 4 || hand.NumberOfCardsOfSuit(Suit.Hearts) >= 4;
        }

        public static bool ContainsHonorSequence(IHand hand) {
            return GetHonorSequence(hand).Count > 0;
        }

        public static List<Card> GetHonorSequence(IHand hand) {
            var groupedCardsBySuit = hand.Cards.GroupBy(card => card.Suit);

            foreach (var group in groupedCardsBySuit) {
                var orderedRanks = group
                    .Select(card => card.Rank)
                    .OrderByDescending(rank => rank)
                    .ToList();

                var honorSequence = new List<Card>();

                // AQK
                if (orderedRanks.Contains(Rank.Ace) && orderedRanks.Contains(Rank.Queen) && orderedRanks.Contains(Rank.King)) {
                    honorSequence.Add(new Card(Rank.Ace, group.Key));
                    honorSequence.Add(new Card(Rank.Queen, group.Key));
                    honorSequence.Add(new Card(Rank.King, group.Key));
                }

                // KQJ
                else if (orderedRanks.Contains(Rank.King) && orderedRanks.Contains(Rank.Queen) && orderedRanks.Contains(Rank.Jack)) {
                    honorSequence.Add(new Card(Rank.King, group.Key));
                    honorSequence.Add(new Card(Rank.Queen, group.Key));
                    honorSequence.Add(new Card(Rank.Jack, group.Key));
                }
                // QJ10
                else if (orderedRanks.Contains(Rank.Queen) && orderedRanks.Contains(Rank.Jack) && orderedRanks.Contains(Rank.Ten)) {
                    honorSequence.Add(new Card(Rank.Queen, group.Key));
                    honorSequence.Add(new Card(Rank.Jack, group.Key));
                    honorSequence.Add(new Card(Rank.Ten, group.Key));
                }
                return honorSequence;
            }

            return new List<Card>();
        }

        private static bool Is4333(List<int> numberOfEachSuit) {
            return numberOfEachSuit.Count(suit => suit == 3) == 3 &&
                   numberOfEachSuit.Count(suit => suit == 4) == 1;
        }

        private static bool Is4432(List<int> numberOfEachSuit) {
            return numberOfEachSuit.Count(suit => suit == 4) == 2 &&
                   numberOfEachSuit.Count(suit => suit == 3) == 1 &&
                   numberOfEachSuit.Count(suit => suit == 2) == 1;
        }

        private static bool Is5332(List<int> numberOfEachSuit) {
            return numberOfEachSuit.Count(suit => suit == 5) == 1 &&
                   numberOfEachSuit.Count(suit => suit == 3) == 2 &&
                   numberOfEachSuit.Count(suit => suit == 2) == 1;
        }
    }
}