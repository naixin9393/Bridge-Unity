using System.Collections.ObjectModel;

public interface IGameManager {
    ReadOnlyCollection<IPlayer> Players { get; }
    ReadOnlyCollection<ICall> Calls { get; }
    IPlayer CurrentPlayer { get; }
    Bid HighestBid { get; }
    Bid Contract { get; }
    Suit? LeadSuit { get; }
    void StartGame();
    void ProcessCall(ICall call);
    void ProcessPlay(Card card, IPlayer player);
    void ProceedNextAction();
}