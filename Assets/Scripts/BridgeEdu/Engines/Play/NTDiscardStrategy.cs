using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Utils;

namespace BridgeEdu.Engines.Play {
    public class NTDiscardStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            if (playingContext.Contract.Strain != Strain.NoTrump) return false;
            var cards = playingContext.PossibleCards;
            var leadingSuit = playingContext.CurrentTrick.LeadSuit;
            if (leadingSuit == null) return false;

            // Not applicable if theres is card of same suit
            if (cards.Exists(card => card.Suit == leadingSuit)) {
                return false;
            }
            return true;
        }

        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            if (!IsApplicable(context)) return new List<PlayingSuggestion>();

            var suggestions = new List<PlayingSuggestion>();
            if (context.PossibleCards.Count == 0) {
                return new List<PlayingSuggestion>();
            }

            var cards = context.PossibleCards;

            // Discard the lowest card
            cards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            suggestions.Add(
                new PlayingSuggestion(
                    message: PlayingMessagesUtils.DiscardLowestCard(cards),
                    card: cards[0]
                )
            );

            return suggestions;
        }
    }
}