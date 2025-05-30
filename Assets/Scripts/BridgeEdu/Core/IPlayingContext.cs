using System.Collections.Generic;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Core {
    public interface IPlayingContext {
        List<ITrick> Tricks { get; }
        List<Card> PossibleCards { get; }
        ITrick CurrentTrick { get; }
        IPlayer Dummy { get; }
        IPlayer Human { get; }
        Bid Contract { get; }
        IHand Hand { get; }
    }
}