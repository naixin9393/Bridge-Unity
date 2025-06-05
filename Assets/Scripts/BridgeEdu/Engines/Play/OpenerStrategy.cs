using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Utils;

namespace BridgeEdu.Engines.Play {
    public class OpenerStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            if (playingContext.Tricks.Count != 1) return false;
            if (playingContext.Tricks[0].LeadSuit != null) return false;
            if (playingContext.Contract == null) return false;
            return true;
        }
        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();

            Bid contract = context.Contract;

            // If the contract is No Trump, we can play the first card of an honor sequence
            if (contract.Strain == Strain.NoTrump)
                suggestions.AddRange(NoTrumpSuggestions(context));

            return suggestions;
        }

        private IEnumerable<PlayingSuggestion> NoTrumpSuggestions(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();
            // If the hand contains an honor sequence, suggest playing the first card of that sequence
            if (HandUtils.ContainsHonorSequence(context.Hand)) {
                List<Card> honorSequence = HandUtils.GetHonorSequence(context.Hand);
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.OpeningHonorSequence(honorSequence),
                    card: honorSequence[0]
                ));
            }

            // If suit has 5 or more cards and contains honor, suggest the fourth highest card
            if (HandUtils.Contains5CardSuitWithHonor(context.Hand)) {
                List<Card> suitWithHonor = HandUtils.GetFifthSuitWithHonor(context.Hand);
                Card fourthHighest = suitWithHonor.Count >= 4 ? suitWithHonor[3] : null;
                if (fourthHighest != null) {
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.OpeningFourthHighest(suitWithHonor),
                        card: fourthHighest
                    ));
                }
            }

            // If suit has 2 consecutive honors, and a third card 2 below the lowest honor, suggest playing highest honor
            if (HandUtils.ContainsTwoConsecutiveHonorsAndTwoBelow(context.Hand)) {
                List<Card> twoBelow = HandUtils.GetTwoBelowCards(context.Hand);
                Card highestHonor = twoBelow[0]; // Assuming the first card is the highest honor
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.OpeningTwoHonorTwoBelow(twoBelow),
                    card: highestHonor
                ));
            }

            return suggestions;
        }
    }
}