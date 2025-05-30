using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

using BridgeEdu.Core;
using BridgeEdu.Game.Play;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Events;

namespace BridgeEdu.Game {
    public class GameManager : MonoBehaviour, IGameManager {
        [SerializeField] private GameEvent OnTrickEnd;
        [SerializeField] private GameEvent OnGamePhaseChanged;
        [SerializeField] private GameEvent OnCallMade;
        [SerializeField] private GameEvent OnPlayMade;
        private List<IPlayer> _players;
        private IPlayer _dealer;
        private IBidding _bidding;
        private IPlay _play;
        private ITrick _currentTrick => _play.CurrentTrick;
        private IPlayer _human;
        public GamePhase Phase { get; private set; }
        public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();
        public IPlayer CurrentPlayer => Phase == GamePhase.Auction ? _bidding.CurrentPlayer : _play.CurrentPlayer;
        public ReadOnlyCollection<ICall> Calls => _bidding.Calls.AsReadOnly();
        public Bid HighestBid => _bidding.HighestBid.Bid;
        public Bid Contract => _bidding.FinalContract;
        public IPlayer Declarer => _bidding.Declarer;
        public Suit? LeadSuit => _currentTrick.LeadSuit;
        public ReadOnlyCollection<ITrick> Tricks => _play.Tricks;
        public event Action<GamePhase> OnPhaseChanged;
        public List<BiddingSuggestion> BiddingSuggestions => _bidding.BiddingSuggestions;

        public int TricksWonByAttackers => _play.TricksWonByAttackers;
        public int TricksWonByDefenders => _play.TricksWonByDefenders;


        public void Initialize(List<IPlayer> players, IPlayer dealer, IPlayer human, IBiddingEngine biddingEngine) {
            _players = players;
            _dealer = dealer;
            _human = human;
            _bidding = new BiddingPhase(players: _players, dealer: _dealer, human: _human, biddingEngine: biddingEngine);
            Phase = GamePhase.Auction;
        }

        public void StartGame() => _bidding.RequestPlayerCallDecision();

        public void ProcessCall(ICall call) {
            _bidding.MakeCall(call);
            if (_bidding.IsOver && _bidding.HighestBid == null) {
                Phase = GamePhase.End;
                OnGamePhaseChanged.Raise(this, Phase);
                OnPhaseChanged?.Invoke(Phase);
                return;
            }
            OnCallMade.Raise(this, call);
        }

        public void ProcessPlay(Card card, IPlayer player) {
            _play.PlayCard(card, player);
            OnPlayMade.Raise(this, (card, player));
        }

        public void ProceedNextAction() {
            if (Phase == GamePhase.Auction) {
                if (!_bidding.IsOver) {
                    _bidding.RequestPlayerCallDecision();
                    return;
                }
                Phase = GamePhase.Play;
                _play = new PlayPhase(_bidding);
                OnGamePhaseChanged.Raise(this, Phase);
                OnPhaseChanged?.Invoke(Phase);
                return;
            }
            if (Phase == GamePhase.Play) {
                if (!_play.IsOver) {
                    if (_currentTrick.IsOver) {
                        var winner = _currentTrick.Winner;
                        _play.StartNewTrick();
                        Debug.Log("Tricks won by attackers: " + TricksWonByAttackers);
                        OnTrickEnd.Raise(this, winner);
                        return;
                    }
                    _play.RequestPlayerPlayDecision();
                    return;
                }
                Phase = GamePhase.End;
                OnGamePhaseChanged.Raise(this, Phase);
                OnPhaseChanged?.Invoke(Phase);
                return;
            }
        }
    }
}