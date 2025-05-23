using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Game : IGame {
    private readonly List<IPlayer> _players;
    private readonly IPlayer _dealer;
    private readonly IAuction _auction;
    private IPlay _play;
    private ITrick _currentTrick => _play.CurrentTrick;
    public Suit LeadSuit => _currentTrick.LeadSuit;

    public ReadOnlyCollection<ICall> Calls => _auction.Calls.AsReadOnly();
    public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();

    public IPlayer CurrentPlayer => Phase == GamePhase.Auction ? _auction.CurrentPlayer : _play.CurrentPlayer;

    public Bid HighestBid => _auction.HighestBid.Bid;

    public event Action OnTrickEnded;

    public GamePhase Phase;

    public event Action<ICall> OnCallMade;
    public event Action<Card, IPlayer> OnPlayMade;
    public event Action<GamePhase> OnGamePhaseChanged;

    public Game(List<IPlayer> players, IPlayer dealer) {
        _players = players;
        _dealer = dealer;
        _auction = new Auction(_players, _dealer);
        Phase = GamePhase.Auction;
    }

    public void StartGame() => _auction.RequestPlayerCallDecision();

    public void ProcessCall(ICall call) {
        _auction.MakeCall(call);
        OnCallMade?.Invoke(call);
    }

    public void ProcessPlay(Card card, IPlayer player) {
        _play.PlayCard(card, player);
        OnPlayMade?.Invoke(card, player);
    }

    public void ProceedNextAction() {
        if (Phase == GamePhase.Auction) {
            if (!_auction.IsOver) {
                _auction.RequestPlayerCallDecision();
                return;
            }
            Phase = GamePhase.Play;
            _play = new Play(_auction);
            OnGamePhaseChanged?.Invoke(Phase);
            return;
        }
        if (Phase == GamePhase.Play) {
            if (!_play.IsOver) {
                if (_currentTrick.IsOver) {
                    _play.StartNewTrick();
                    OnTrickEnded?.Invoke();
                    return;
                }
                _play.RequestPlayerPlayDecision();
                return;
            }
            Phase = GamePhase.End;
            OnGamePhaseChanged?.Invoke(Phase);
            return;
        }
    }
}