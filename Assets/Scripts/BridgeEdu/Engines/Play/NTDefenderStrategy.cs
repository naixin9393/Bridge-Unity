using System;
using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Utils;

namespace BridgeEdu.Engines.Play {

    public class NTDefenderStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            // Check if the context is suitable for NT Attacker strategy
            if (playingContext.IsAttackerTurn) return false;
            // Only apply if there are multiple possible cards to play
            if (playingContext.PossibleCards.Count <= 1) return false;
            if (playingContext.Contract.Strain != Strain.NoTrump) return false;
            // If possible cards doesnt contain lead suit, then not applicable
            var leadSuit = playingContext.CurrentTrick.LeadSuit;
            if (leadSuit != null && !playingContext.PossibleCards.Exists(card => card.Suit == leadSuit)) return false;
            return true;
        }

        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            // Generate suggestions based on the NT Attacker strategy
            var suggestions = new List<PlayingSuggestion>();

            if (context.PossibleCards.Count == 0) {
                return new List<PlayingSuggestion>();
            }

            return context.CurrentTrick.Plays.Count switch {
                0 => DefenderFirstTurn(context),
                1 => DefenderSecondTurn(context),
                2 => DefenderThirdTurn(context),
                3 => DefenderFourthTurn(context),
                _ => suggestions,
            };
        }

        private List<PlayingSuggestion> DefenderFirstTurn(IPlayingContext context) {
            return new List<PlayingSuggestion>();
        }

        private List<PlayingSuggestion> DefenderSecondTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();
            var leadCard = context.CurrentTrick.Plays[0].Card;
            var possibleCards = context.PossibleCards;

            // If the lead card is a honor, play bigger honor if possible
            if (CardUtils.IsHonor(leadCard)) {
                if (HandUtils.ContainsBiggerHonor(possibleCards, leadCard)) {
                    var biggerHonor = HandUtils.GetBiggerHonor(possibleCards, leadCard);
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.DefenderHonorOverHonor,
                        card: biggerHonor
                    ));
                }
            }

            possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            suggestions.Add(new PlayingSuggestion(
                message: PlayingMessagesUtils.DefenderSecondTurnPlayLowestCard,
                card: possibleCards[0]
            ));

            return suggestions;
        }

        private List<PlayingSuggestion> DefenderThirdTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();
            return suggestions;
        }

        private List<PlayingSuggestion> DefenderFourthTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();
            return suggestions;
        }
    }
}