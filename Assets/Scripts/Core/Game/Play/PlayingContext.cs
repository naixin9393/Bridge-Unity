using System.Collections.Generic;

public readonly struct PlayingContext {
    public readonly List<ITrick> Tricks;
    public readonly List<Card> PossibleCards;
    
    public PlayingContext(List<Card> possibleCards, List<ITrick> tricks) {
        PossibleCards = possibleCards;
        Tricks = tricks;
    }
}