using System.Collections.Generic;

public interface IPlayingEngine {
    List<PlayingSuggestion> GetSuggestions(PlayingContext context);
}