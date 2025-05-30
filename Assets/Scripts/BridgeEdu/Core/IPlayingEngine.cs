using System.Collections.Generic;

using BridgeEdu.Engines.Play;
using BridgeEdu.Game.Play;

namespace BridgeEdu.Core {
    public interface IPlayingEngine {
        List<PlayingSuggestion> GetSuggestions(PlayingContext context);
    }
}