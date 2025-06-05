using System.Collections.Generic;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IPlayingContext {
        List<IPlayer> Players { get; }
        List<ITrick> Tricks { get; }
        List<Card> PossibleCards { get; }
        ITrick CurrentTrick { get; }
        IPlayer Dummy { get; }
        IPlayer CurrentPlayer { get; }
        IPlayer Human { get; }
        Bid Contract { get; }
        IHand Hand { get; }
        bool IsAttackerTurn { get; }
        Suit DeclarerLongSuit { get; }
    }
}