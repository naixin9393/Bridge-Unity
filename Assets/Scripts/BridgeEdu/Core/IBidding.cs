using System.Collections.Generic;

using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IBidding {
        List<IPlayer> Players { get; }
        IPlayer Declarer { get; }
        ICall LastCall { get; }
        Bid FinalContract { get; }
        IPlayer CurrentPlayer { get; }
        IPlayer Dummy { get; }
        IPlayer Dealer { get; }
        IPlayer Human { get; }
        List<ICall> Calls { get; }
        List<BiddingSuggestion> BiddingSuggestions { get; }
        bool IsOver { get; }
        BidCall HighestBid { get; }
        void RequestPlayerCallDecision();
        void MakeCall(ICall call);
    }
}