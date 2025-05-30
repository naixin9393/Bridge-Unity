using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Play;
using BridgeEdu.Utils;

namespace BridgeEdu.Engines.Play {
    public class OpenerStrategy : IPlayingStrategy {
        public bool IsApplicable(PlayingContext playingContext) {
            if (playingContext.Tricks.Count != 1) return false;
            if (playingContext.Tricks[0].LeadSuit != null) return false;
            return true;
        }
        public List<PlayingSuggestion> GetSuggestions(PlayingContext context) {
            List<PlayingSuggestion> suggestions = new();

            if (HandUtils.ContainsHonorSequence(context.Hand)) {
                List<Card> honorSequence = HandUtils.GetHonorSequence(context.Hand);

                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.HonorSequence(honorSequence),
                    card: honorSequence[0]
                ));
            }

            return suggestions;
        }
    }
}