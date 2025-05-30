namespace BridgeEdu.Engines.Play {
    using System.Collections.Generic;
    using BridgeEdu.Core;
    using BridgeEdu.Game.Bidding;
    using BridgeEdu.Utils;
    public class NTAttackerStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            // Check if the context is suitable for NT Attacker strategy
            if (!playingContext.IsAttackerTurn) return false;
            // Only apply if there are multiple possible cards to play
            if (playingContext.PossibleCards.Count <= 1) return false;
            if (playingContext.Contract.Strain != Strain.NoTrump) return false;
            return true;
        }

        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            // Generate suggestions based on the NT Attacker strategy
            var suggestions = new List<PlayingSuggestion>();
            if (context.PossibleCards.Count == 0) {
                return new List<PlayingSuggestion>();
            }

            // All possible cards are same suit and consecutive play lowest
            var cards = context.PossibleCards;

            if (HandUtils.AllSameSuit(cards) && HandUtils.AreConsecutive(cards)) {
                // Sort cards by rank
                cards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(
                    new PlayingSuggestion(
                        message: PlayingMessagesUtils.PlayLowestCardConsecutive(cards),
                        card: cards[0]
                    )
                );
            }

            return suggestions;
        }
    }
}