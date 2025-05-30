using System.Collections.Generic;

using BridgeEdu.Engines.Play;

namespace BridgeEdu.Core {
    public interface IPlayingEngine {
        List<PlayingSuggestion> GetSuggestions(IPlayingContext context);
    }
}