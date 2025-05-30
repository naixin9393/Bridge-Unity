using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Players;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Engines.Bidding {
    public class BiddingContextBuilder {
        private IHand _hand;
        private List<ICall> _calls = new();
        private Position _currentPosition;

        public BiddingContextBuilder WithHand(IHand hand) {
            _hand = hand;
            return this;
        }

        public BiddingContextBuilder WithCalls(List<ICall> calls) {
            _calls = calls;
            return this;
        }

        public BiddingContextBuilder WithCurrentPosition(Position position) {
            _currentPosition = position;
            return this;
        }

        public BiddingContext Build() {
            return new BiddingContext(_calls, _hand, _currentPosition);
        }
    }
}