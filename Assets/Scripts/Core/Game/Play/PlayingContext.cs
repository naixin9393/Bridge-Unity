using System.Collections.Generic;

public readonly struct PlayingContext {
    public readonly List<ITrick> Tricks;
    public readonly List<Card> PossibleCards;
    public readonly IPlayer Dummy;
    
    public PlayingContext(List<Card> possibleCards, List<ITrick> tricks, IPlayer dummy) {
        PossibleCards = possibleCards;
        Tricks = tricks;
        Dummy = dummy;
    }
}