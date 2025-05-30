using System.Collections.Generic;

using BridgeEdu.Core;

namespace BridgeEdu.Game.Play {
    public readonly struct PlayingContext {
        public readonly List<ITrick> Tricks;
        public readonly List<Card> PossibleCards;
        public readonly IPlayer Dummy;
        public readonly IPlayer Human;

        public PlayingContext(List<Card> possibleCards, List<ITrick> tricks, IPlayer dummy, IPlayer human) {
            PossibleCards = possibleCards;
            Tricks = tricks;
            Dummy = dummy;
            Human = human;
        }
    }
}
