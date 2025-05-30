using System.Collections.Generic;
using BridgeEdu.Core;

namespace BridgeEdu.Engines.Play {
    public class OnlyOneCardPossibleStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            return playingContext.PossibleCards.Count == 1;
        }

        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            if (!IsApplicable(context)) return new List<PlayingSuggestion>();

            var card = context.PossibleCards[0];
            return new List<PlayingSuggestion> {
                new PlayingSuggestion(card, "Solo hay una carta posible para jugar")
            };
        }
    }
}