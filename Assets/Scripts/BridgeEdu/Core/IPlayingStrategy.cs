namespace BridgeEdu.Core {
    using System.Collections.Generic;
    using BridgeEdu.Engines.Play;

    public interface IPlayingStrategy {
        bool IsApplicable(IPlayingContext playingContext);
        List<PlayingSuggestion> GetSuggestions(IPlayingContext context);
    }
}