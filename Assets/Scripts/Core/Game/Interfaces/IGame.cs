using System;
using System.Collections.ObjectModel;

public interface IGame {
    ReadOnlyCollection<IPlayer> Players { get; }
    ReadOnlyCollection<ICall> Calls { get; }
    IPlayer CurrentPlayer { get; }
    Bid HighestBid { get; }
    void StartGame();
    void ProcessCall(ICall call);
    void ProcessPlay(Card card, IPlayer player);
    void ProceedNextAction();

    event Action<ICall> OnCallMade;
    event Action<Card, IPlayer> OnPlayMade;
    event Action<GamePhase> OnGamePhaseChanged;
    event Action OnTrickEnded;
}