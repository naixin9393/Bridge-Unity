using System.Collections.Generic;

using BridgeEdu.Game.Play;

namespace BridgeEdu.Engines.Play {
    public interface IPlayingEngine {
        List<PlayingSuggestion> GetSuggestions(PlayingContext context);
    }
}