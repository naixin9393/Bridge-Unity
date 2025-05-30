using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Game.Play {
    public readonly struct PlayingContext {
        public readonly List<ITrick> Tricks;
        public readonly List<Card> PossibleCards;
        public readonly IPlayer Dummy;
        public readonly IPlayer Human;
        public readonly Bid Contract;
        public readonly IHand Hand;

        public PlayingContext(List<Card> possibleCards, List<ITrick> tricks, IPlayer dummy, IPlayer human, Bid contract, IHand hand) {
            PossibleCards = possibleCards;
            Tricks = tricks;
            Dummy = dummy;
            Human = human;
            Contract = contract;
            Hand = hand;
        }
    }
}
