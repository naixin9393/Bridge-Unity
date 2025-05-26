using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager {
    [SerializeField] private GameEvent OnTrickEnd;
    [SerializeField] private GameEvent OnGamePhaseChanged;
    [SerializeField] private GameEvent OnCallMade;
    [SerializeField] private GameEvent OnPlayMade;
    private List<IPlayer> _players;
    private IPlayer _dealer;
    private IAuction _auction;
    private IPlay _play;
    private ITrick _currentTrick => _play.CurrentTrick;
    public GamePhase Phase { get; private set; }
    public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();
    public IPlayer CurrentPlayer => Phase == GamePhase.Auction ? _auction.CurrentPlayer : _play.CurrentPlayer;
    public ReadOnlyCollection<ICall> Calls => _auction.Calls.AsReadOnly();
    public Bid HighestBid => _auction.HighestBid.Bid;
    public Bid Contract => _auction.FinalContract;
    public IPlayer Declarer => _auction.Declarer;
    public Suit? LeadSuit => _currentTrick.LeadSuit;
    public ReadOnlyCollection<ITrick> Tricks => _play.Tricks;

    public event Action<int> OnTricksWonByAttackersChanged;
    public event Action<GamePhase> OnPhaseChanged;
    public BiddingSuggestion BiddingSuggestion => _auction.BiddingSuggestion;

    public int TricksWonByAttackers {
        get => _play.TricksWonByAttackers;
        set {
            OnTricksWonByAttackersChanged?.Invoke(value);
        }
    }


    public void Initialize(List<IPlayer> players, IPlayer dealer, IBiddingEngine biddingEngine) {
        _players = players;
        _dealer = dealer;
        _auction = new Auction(players: _players, dealer: _dealer, biddingEngine: biddingEngine);
        Phase = GamePhase.Auction;
    }

    public void StartGame() => _auction.RequestPlayerCallDecision();

    public void ProcessCall(ICall call) {
        _auction.MakeCall(call);
        if (_auction.IsOver && _auction.HighestBid == null) {
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
            if (!_auction.IsOver) {
                _auction.RequestPlayerCallDecision();
                return;
            }
            Phase = GamePhase.Play;
            _play = new Play(_auction);
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