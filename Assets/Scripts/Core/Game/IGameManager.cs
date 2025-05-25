using System;
using System.Collections.ObjectModel;

public interface IGameManager {
    ReadOnlyCollection<IPlayer> Players { get; }
    ReadOnlyCollection<ICall> Calls { get; }
    ReadOnlyCollection<ITrick> Tricks { get; }
    IPlayer CurrentPlayer { get; }
    IPlayer Declarer { get; }
    Bid HighestBid { get; }
    Bid Contract { get; }
    Suit? LeadSuit { get; }
    event Action<int> OnTricksWonByAttackersChanged;
    int TricksWonByAttackers { get; }
    event Action<GamePhase> OnPhaseChanged;

    void StartGame();
    void ProcessCall(ICall call);
    void ProcessPlay(Card card, IPlayer player);
    void ProceedNextAction();
}