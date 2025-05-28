using System.Collections.Generic;

public readonly struct PlayingContext {
    public readonly List<ITrick> Tricks;
    public readonly List<Card> PossibleCards;
    public readonly IPlayer Dummy;
    public readonly IPlayer Human;
    
    public PlayingContext(List<Card> possibleCards, List<ITrick> tricks, IPlayer dummy, IPlayer human) {
        PossibleCards = possibleCards;
        Tricks = tricks;
        Dummy = dummy;
        Human = human;
    }
}