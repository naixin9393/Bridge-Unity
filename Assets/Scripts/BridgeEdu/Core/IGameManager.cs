using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using BridgeEdu.Game.Bidding;
using BridgeEdu.Game;

namespace BridgeEdu.Core {
    public interface IGameManager {
        ReadOnlyCollection<IPlayer> Players { get; }
        ReadOnlyCollection<ICall> Calls { get; }
        ReadOnlyCollection<ITrick> Tricks { get; }
        IPlayer CurrentPlayer { get; }
        IPlayer Declarer { get; }
        Bid HighestBid { get; }
        Bid Contract { get; }
        Suit? LeadSuit { get; }
        List<BiddingSuggestion> BiddingSuggestions { get; }
        int TricksWonByAttackers { get; }
        int TricksWonByDefenders { get; }
        GamePhase Phase { get; }
        event Action<GamePhase> OnPhaseChanged;
        void StartGame();
        void ProcessCall(ICall call);
        void ProcessPlay(Card card, IPlayer player);
        void ProceedNextAction();
    }
}