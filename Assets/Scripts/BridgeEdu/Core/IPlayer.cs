using System;
using System.Collections.Generic;

using BridgeEdu.Game.Play;
using BridgeEdu.Game.Players;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Engines.Play;

namespace BridgeEdu.Core {
    public interface IPlayer {
        Position Position { get; }
        IHand Hand { get; }
        void ReceiveCards(List<Card> cards);
        void RequestPlayerPlayDecision(PlayingContext playingContext, List<PlayingSuggestion> playingSuggestions);
        void RequestPlayerCallDecision(List<BiddingSuggestion> biddingSuggestions);
        void PlayCard(Card card);
        event Action<Card, IPlayer> OnCardChosen;
        event Action<ICall> OnCallChosen;
    }
}