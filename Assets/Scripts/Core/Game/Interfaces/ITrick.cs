using System.Collections.Generic;

public interface ITrick {
    List<(Card Card, IPlayer Player)> Plays { get; }
    bool IsOver { get; }
    IPlayer CurrentPlayer { get; }
    IPlayer Winner { get; }
    Suit? LeadSuit { get; }
    void PlayCard(Card card, IPlayer player);
}