using System.Collections.Generic;
using BridgeEdu.Engines.Play;
using BridgeEdu.Game.Play;
using BridgeEdu.Core;

namespace BridgeEdu.Engines {
    public class NullPlayingEngine : IPlayingEngine {
        public List<PlayingSuggestion> GetSuggestions(PlayingContext context) {
            // This engine does not provide any suggestions.
            return new List<PlayingSuggestion>();
        }
    }
}