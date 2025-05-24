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
    public Suit? LeadSuit => _currentTrick.LeadSuit;

    public ReadOnlyCollection<ICall> Calls => _auction.Calls.AsReadOnly();
    public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();

    public IPlayer CurrentPlayer => Phase == GamePhase.Auction ? _auction.CurrentPlayer : _play.CurrentPlayer;

    public Bid HighestBid => _auction.HighestBid.Bid;
    public Bid Contract => _auction.FinalContract;
    public GamePhase Phase;
    

    public void Initialize(List<IPlayer> players, IPlayer dealer) {
        _players = players;
        _dealer = dealer;
        _auction = new Auction(_players, _dealer);
        Phase = GamePhase.Auction;
    }

    public void StartGame() => _auction.RequestPlayerCallDecision();

    public void ProcessCall(ICall call) {
        _auction.MakeCall(call);
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
            return;
        }
        if (Phase == GamePhase.Play) {
            if (!_play.IsOver) {
                if (_currentTrick.IsOver) {
                    var winner = _currentTrick.Winner;
                    _play.StartNewTrick();
                    OnTrickEnd.Raise(this, winner);
                    return;
                }
                _play.RequestPlayerPlayDecision();
                return;
            }
            Phase = GamePhase.End;
            OnGamePhaseChanged.Raise(this, Phase);
            return;
        }
    }
}