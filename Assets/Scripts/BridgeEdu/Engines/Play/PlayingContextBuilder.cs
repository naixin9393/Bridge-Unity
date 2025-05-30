using System.Collections.Generic;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Play;

namespace BridgeEdu.Engines.Play {
    public class PlayingContextBuilder {
        private IHand _hand;
        private IList<ITrick> _tricks = new List<ITrick>();
        private IList<Card> _possibleCards = new List<Card>();
        private IPlayer _dummy;
        private IPlayer _human;
        private Bid _contract;
        private ITrick _currentTrick;
        private bool _isAttackerTurn;

        public PlayingContextBuilder WithHand(IHand hand) {
            _hand = hand;
            return this;
        }

        public PlayingContextBuilder WithCurrentTrick(ITrick currentTrick) {
            _currentTrick = currentTrick;
            return this;
        }

        public PlayingContextBuilder WithTricks(IList<ITrick> tricks) {
            _tricks = tricks;
            return this;
        }

        public PlayingContextBuilder WithPossibleCards(IList<Card> possibleCards) {
            _possibleCards = possibleCards;
            return this;
        }

        public PlayingContextBuilder WithIsAttackerTurn(bool isAttackerTurn) {
            _isAttackerTurn = isAttackerTurn;
            return this;
        }

        public PlayingContextBuilder WithDummy(IPlayer dummy) {
            _dummy = dummy;
            return this;
        }

        public PlayingContextBuilder WithHuman(IPlayer human) {
            _human = human;
            return this;
        }

        public PlayingContextBuilder WithContract(Bid contract) {
            _contract = contract;
            return this;
        }

        public PlayingContext Build() {
            return new PlayingContext(
                hand: _hand,
                tricks: new List<ITrick>(_tricks),
                currentTrick: _currentTrick,
                possibleCards: new List<Card>(_possibleCards),
                dummy: _dummy,
                human: _human,
                isAttackerTurn: _isAttackerTurn,
                contract: _contract
            );
        }
    }
}