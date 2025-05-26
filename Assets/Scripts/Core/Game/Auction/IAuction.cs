using System.Collections.Generic;

public interface IAuction {
    List<IPlayer> Players { get; }
    IPlayer Declarer { get; }
    ICall LastCall { get; }
    Bid FinalContract { get; }
    IPlayer CurrentPlayer { get; }
    IPlayer Dummy { get; }
    IPlayer Dealer { get; }
    List<ICall> Calls { get; }
    string BiddingSuggestion { get; }
    bool IsOver { get; }
    BidCall HighestBid { get; }
    void RequestPlayerCallDecision();
    void MakeCall(ICall call);
}