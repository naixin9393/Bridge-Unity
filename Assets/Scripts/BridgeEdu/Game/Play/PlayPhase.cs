using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using BridgeEdu.Core;
using BridgeEdu.Engines;
using BridgeEdu.Engines.Play;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Play.Exceptions;
using BridgeEdu.Utils;

namespace BridgeEdu.Game.Play {
    public class PlayPhase : IPlay {
        internal List<ITrick> _tricks = new();
        internal int _currentTrickIndex;
        internal int _tricksWonByAttackers = 0;
        internal int _tricksWonByDefenders = 0;
        private readonly List<IPlayer> _players = new();
        private readonly IPlayer _dummy;
        private readonly IPlayer _human;
        private List<IPlayer> _attackers = new();
        private List<IPlayer> _defenders = new();
        private IPlayingEngine _playingEngine;
        public ITrick CurrentTrick => _tricks[_currentTrickIndex];
        public ReadOnlyCollection<ITrick> Tricks => new(_tricks);
        public ReadOnlyCollection<IPlayer> Players => new(_players);
        public IPlayer LeadPlayer { get; private set; }
        public IPlayer CurrentPlayer { get; private set; }
        public int TricksWonByAttackers => _tricksWonByAttackers;
        public Bid Contract { get; private set; }
        public bool IsOver { get; private set; }
        public bool ContractIsMade => TricksWonByAttackers >= Contract.Level + 6;

        public int TricksWonByDefenders => _tricksWonByDefenders;

        public List<PlayingSuggestion> PlayingSuggestions => _playingEngine.GetSuggestions(new PlayingContext(
            possibleCards: CalculatePossibleCards(),
            tricks: _tricks.ToList(),
            dummy: _dummy,
            human: _human,
            contract: Contract,
            hand: CurrentPlayer.Hand,
            currentTrick: CurrentTrick
        ));

        public PlayPhase(IBidding bidding) {
            Contract = bidding.FinalContract;
            _players = new List<IPlayer>(bidding.Players);
            _dummy = bidding.Dummy;
            _human = bidding.Human;
            DetermineLeadPlayer(bidding);
            DetermineTeams(LeadPlayer);
            _tricks.Add(new Trick(_players, Contract.Strain, CurrentPlayer));
            _playingEngine = new NullPlayingEngine();
        }

        public PlayPhase(IBidding bidding, IPlayingEngine playingEngine) : this(bidding) {
            _playingEngine = playingEngine;
        }

        public void PlayCard(Card card, IPlayer player) {
            if (IsOver)
                throw new GameOverException();
            if (player != CurrentPlayer)
                throw new NotPlayersTurnException(player, CurrentPlayer);
            if (DoesNotFollowLead(player, card))
                throw new NotFollowingLeadException(player, card.Suit);
            CurrentTrick.PlayCard(card, player);
            player.PlayCard(card);

            if (!CurrentTrick.IsOver) {
                CurrentPlayer = PlayerUtils.GetNextPlayer(CurrentPlayer, _players);
                return;
            }
            if (IsAttacker(CurrentTrick.Winner))
                _tricksWonByAttackers++;
            else
                _tricksWonByDefenders++;
            CurrentPlayer = CurrentTrick.Winner;
            if (Tricks.Count == 13)
                EndGame();
        }

        private bool DoesNotFollowLead(IPlayer player, Card cardToPlay) {
            if (CurrentTrick.Plays.Count == 0) return false;
            var leadSuit = CurrentTrick.Plays[0].Card.Suit;
            if (leadSuit == cardToPlay.Suit) return false;
            return HasCardsOfSameSuit(player, leadSuit);
        }

        private bool HasCardsOfSameSuit(IPlayer player, Suit leadSuit) => player.Hand.HasCardOfSuit(leadSuit);
        private void EndGame() => IsOver = true;

        private void DetermineLeadPlayer(IBidding bidding) {
            LeadPlayer = PlayerUtils.GetNextPlayer(bidding.Declarer, _players);
            CurrentPlayer = LeadPlayer;
        }

        private void DetermineTeams(IPlayer leadPlayer) {
            _defenders = new List<IPlayer> {
            leadPlayer,
            PlayerUtils.PartnerOf(leadPlayer, _players)
        };
            _attackers = _players.Where(player => !_defenders.Contains(player)).ToList();
        }

        private bool IsAttacker(IPlayer player) => _attackers.Contains(player);

        public void StartNewTrick() {
            _tricks.Add(new Trick(_players, Contract.Strain, CurrentTrick.Winner));
            _currentTrickIndex++;
        }

        public void RequestPlayerPlayDecision() {
            List<Card> possibleCards = CalculatePossibleCards();
            var context = new PlayingContext(
                possibleCards: possibleCards,
                tricks: _tricks.ToList(),
                dummy: _dummy,
                human: _human,
                contract: Contract,
                currentTrick: CurrentTrick,
                hand: CurrentPlayer.Hand);
            var playingSuggestions = _playingEngine.GetSuggestions(context);
            CurrentPlayer.RequestPlayerPlayDecision(context, playingSuggestions);
        }

        private List<Card> CalculatePossibleCards() {
            if (CurrentTrick.Plays.Count == 0 || !HasCardsOfSameSuit(CurrentPlayer, CurrentTrick.Plays[0].Card.Suit)) return CurrentPlayer.Hand.Cards;
            return CurrentPlayer.Hand.GetCardsOfSuit(CurrentTrick.Plays[0].Card.Suit);
        }
    }
}