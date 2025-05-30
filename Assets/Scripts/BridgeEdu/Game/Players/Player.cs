using System;
using System.Collections.Generic;

using BridgeEdu.Core;
using BridgeEdu.Game.Play;
using BridgeEdu.Game.Play.Exceptions;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Engines.Play;

namespace BridgeEdu.Game.Players {
    public abstract class Player : IPlayer {
        private readonly IHand _hand = new Hand();

        public Position Position { get; private set; }
        public virtual event Action<Card, IPlayer> OnCardChosen = delegate { };
        public virtual event Action<ICall> OnCallChosen = delegate { };
        public IHand Hand => _hand;
        public void ReceiveCards(List<Card> cards) => _hand.AddCards(cards);

        public Player(Position position) {
            Position = position;
        }

        public void PlayCard(Card card) {
            if (!_hand.HasCard(card))
                throw new CardNotInHandException(card, this);
            _hand.RemoveCard(card);
        }

        public override string ToString() => $"{Position}";
        public virtual void RequestPlayerPlayDecision(PlayingContext _, List<PlayingSuggestion> __) { }
        public virtual void RequestPlayerCallDecision(List<BiddingSuggestion> _) { }
    }
}