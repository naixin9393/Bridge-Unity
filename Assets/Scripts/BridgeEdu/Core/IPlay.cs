using System.Collections.ObjectModel;

using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IPlay {
        ReadOnlyCollection<ITrick> Tricks { get; }
        ReadOnlyCollection<IPlayer> Players { get; }
        IPlayer LeadPlayer { get; }
        IPlayer CurrentPlayer { get; }
        Bid Contract { get; }
        ITrick CurrentTrick { get; }
        bool IsOver { get; }
        int TricksWonByAttackers { get; }
        int TricksWonByDefenders { get; }
        void StartNewTrick();
        void PlayCard(Card card, IPlayer player);
        void RequestPlayerPlayDecision();
    }
}