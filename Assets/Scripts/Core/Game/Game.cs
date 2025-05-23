using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Game : IGame, IDisposable {
    private List<IPlayer> _players;
    private IPlayer _dealer;
    private IAuction _auction;
    private IPlay _play;
    private IPlayer Dealer => _dealer;

    public ReadOnlyCollection<ICall> Calls => _auction.Calls.AsReadOnly();
    public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();

    public IPlayer CurrentPlayer => Phase == GamePhase.Auction ? _auction.CurrentPlayer : _play.CurrentPlayer;

    public Bid HighestBid => _auction.HighestBid.Bid;

    public GamePhase Phase;

    public event Action<ICall> OnCallMade;
    public event Action<GamePhase> OnGamePhaseChanged;

    public Game(List<IPlayer> players, IPlayer dealer) {
        _players = SortPlayersByTurnOrder(players, dealer);
        _dealer = dealer;
        _auction = new Auction(_players, _dealer);
        //_auction.OnCallMade += HandleCallMade;
        //_auction.OnAuctionEnd += HandleAuctionEnd;
        Phase = GamePhase.Auction;
    }

    private void HandleCallMade(ICall call) {
        OnCallMade?.Invoke(call);
        if (!_auction.IsOver)
            _auction.RequestPlayerCallDecision();
    }

    private List<IPlayer> SortPlayersByTurnOrder(List<IPlayer> players, IPlayer dealer) {
        var sortedPlayers = new List<IPlayer>();
        var currentPlayer = dealer;
        for (int i = 0; i < players.Count; i++) {
            sortedPlayers.Add(currentPlayer);
            currentPlayer = players.Where(p => p.Position == NextPlayerPosition(currentPlayer)).First();
        }
        return sortedPlayers;
    }

    private Position NextPlayerPosition(IPlayer currentPlayer) {
        return currentPlayer.Position switch {
            Position.North => Position.East,
            Position.East => Position.South,
            Position.South => Position.West,
            Position.West => Position.North,
            _ => throw new NotImplementedException(),
        };
    }

    public void StartGame() => _auction.RequestPlayerCallDecision();

    public void Dispose() {
        return;
    }

    public void ProcessCall(ICall call) {
        _auction.MakeCall(call);
        OnCallMade?.Invoke(call);
        if (!_auction.IsOver)
            _auction.RequestPlayerCallDecision();
        else {
            Phase = GamePhase.Play;
            OnGamePhaseChanged?.Invoke(Phase);
        }
    }
}