using System;
using System.Collections.ObjectModel;

public interface IGame {
    ReadOnlyCollection<IPlayer> Players { get; }
    ReadOnlyCollection<ICall> Calls { get; }
    IPlayer CurrentPlayer { get; }
    Bid HighestBid { get; }
    void StartGame();
    void ProcessCall(ICall call);
    event Action<ICall> OnCallMade;
}