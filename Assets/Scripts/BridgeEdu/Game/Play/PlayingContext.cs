using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Game.Play {
    public readonly struct PlayingContext : IPlayingContext {
        public List<ITrick> Tricks { get; }
        public List<Card> PossibleCards { get; }
        public ITrick CurrentTrick { get; }
        public IPlayer Dummy { get; }
        public IPlayer Human { get; }
        public IPlayer CurrentPlayer { get; }
        public Bid Contract { get; }
        public IHand Hand { get; }
        public bool IsAttackerTurn { get; }
        public Suit DeclarerLongSuit { get; }
        public List<IPlayer> Players { get; }

        public PlayingContext(
            List<Card> possibleCards,
            List<ITrick> tricks,
            IPlayer dummy,
            IPlayer human,
            Bid contract,
            IHand hand,
            ITrick currentTrick,
            bool isAttackerTurn,
            Suit declarerLongSuit,
            IPlayer currentPlayer,
            List<IPlayer> players) {

            PossibleCards = possibleCards;
            CurrentTrick = currentTrick;
            Tricks = tricks;
            Dummy = dummy;
            Human = human;
            Contract = contract;
            Hand = hand;
            IsAttackerTurn = isAttackerTurn;
            DeclarerLongSuit = declarerLongSuit;
            Players = players;
            CurrentPlayer = currentPlayer;
        }
    }
}
