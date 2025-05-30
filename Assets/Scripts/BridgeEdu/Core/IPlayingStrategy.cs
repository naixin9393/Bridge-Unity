namespace BridgeEdu.Core {
    using System.Collections.Generic;
    using BridgeEdu.Engines.Play;
    using BridgeEdu.Game.Play;

    public interface IPlayingStrategy {
        bool IsApplicable(PlayingContext playingContext);
        List<PlayingSuggestion> GetSuggestions(PlayingContext context);
    }
}