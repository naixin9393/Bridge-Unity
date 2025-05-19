using System.Collections.ObjectModel;

public interface IPlay {
    ReadOnlyCollection<ITrick> Tricks { get; }
    ReadOnlyCollection<IPlayer> Players { get; }
    IPlayer LeadPlayer { get; }
    IPlayer CurrentPlayer { get; }
    Bid Contract { get; }
    ITrick CurrentTrick { get; }
    bool IsOver { get; }
    int TricksWonByAttackers { get; }
    void PlayCard(Card card, IPlayer player);
}