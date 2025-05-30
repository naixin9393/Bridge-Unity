using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Engines.Play;
using BridgeEdu.Game.Play;
using BridgeEdu.Utils;

public class PlayingEngine : IPlayingEngine {
    private List<IPlayingStrategy> _strategies;

    public PlayingEngine(List<IPlayingStrategy> strategies) {
        _strategies = strategies;
    }

    public List<PlayingSuggestion> GetSuggestions(PlayingContext context) {
        var suggestions = new List<PlayingSuggestion>();
        foreach (IPlayingStrategy strategy in _strategies) {
            if (strategy.IsApplicable(context)) {
                suggestions.AddRange(strategy.GetSuggestions(context));
            }
        }
        if (suggestions.Count == 0) {
            suggestions.Add(new PlayingSuggestion(message: PlayingMessagesUtils.Unknown, card: null));
        }
        return suggestions;
    }
}