using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class GameViewModel : IDisposable {
    private readonly IGameManager _game;
    private readonly IPlayer _humanPlayer;

    public ReadOnlyCollection<ICall> Calls => _game.Calls;
    public ReadOnlyCollection<IPlayer> Players => _game.Players;
    public IPlayer CurrentPlayer => _game.CurrentPlayer;
    public Bid HighestBid => _game.HighestBid;
    public Suit? LeadSuit => _game.LeadSuit;
    public Bid Contract => _game.Contract;
    public IPlayer Declarer => _game.Declarer;
    public IPlayer Dummy => PlayerUtils.PartnerOf(_game.Declarer, _game.Players.ToList());
    public bool IsPlayerAttacker;
    public BindableProperty<bool> ShowDummyHand => BindableProperty<bool>.Bind(() => {
        var noPlaysMade = _game.Tricks.Count == 1 && _game.Tricks[0].Plays.Count == 0;

        return !noPlaysMade;
    });
    public GamePhase Phase => _game.Phase;
    public BindableProperty<int> WonTricksCount => BindableProperty<int>.Bind(() => {
        var declarer = _game.Declarer;
        var partnerOfDeclarer = PlayerUtils.PartnerOf(declarer, _game.Players.ToList());

        bool isPlayerAttacker = _humanPlayer == declarer || _humanPlayer == partnerOfDeclarer;
        return isPlayerAttacker ?
            _game.TricksWonByAttackers :
            _game.TricksWonByDefenders;
    });
    
    public BindableProperty<string> BiddingHintCall => BindableProperty<string>.Bind(() => {
        if (_phase == GamePhase.Auction && _game.BiddingSuggestions.Count > 0) {
            var call = _game.BiddingSuggestions[0].Call;
            if (call.Type != CallType.Bid)
                return call.ToString().ToLower();
            var bidCall = call as BidCall;
            return bidCall.Bid.ToString();
        }
        return string.Empty;
    });
    
    public BindableProperty<string> BiddingHintMessage => BindableProperty<string>.Bind(() => {
        if (_phase == GamePhase.Auction && _game.BiddingSuggestions.Count > 0) {
            Debug.Log(_game.BiddingSuggestions[0].Message);
            return _game.BiddingSuggestions[0].Message;
        }
        return string.Empty;
    });

    public int TricksNeededToWin;
    public ReadOnlyCollection<(string call, string message)> BiddingHintHistory => _biddingHintHistory.AsReadOnly();
    private readonly List<(string call, string message)> _biddingHintHistory = new();
    private GamePhase _phase = GamePhase.Auction;

    public GameViewModel(IGameManager game, IPlayer humanPlayer) {
        _game = game;
        _game.OnPhaseChanged += HandleGamePhaseChanged;
        _humanPlayer = humanPlayer;

        foreach (IPlayer player in _game.Players) {
            player.OnCallChosen += HandlePlayerCallChosen;
            player.OnCardChosen += HandlePlayerCardChosen;
        }
        
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        _phase = phase;
        switch (phase) {
            case GamePhase.Play: 
                IsPlayerAttacker = _humanPlayer == _game.Declarer || _humanPlayer == PlayerUtils.PartnerOf(_game.Declarer, _game.Players.ToList());
                int tricksAttackersNeed = 6 + _game.Contract.Level;
                TricksNeededToWin = IsPlayerAttacker ? tricksAttackersNeed : 14 - tricksAttackersNeed;
                break;
        }
    }

    public void HandlePlayerCardChosen(Card card, IPlayer player) {
        _game.ProcessPlay(card, player);
    }

    public void HandlePlayerCallChosen(ICall call) {
        _biddingHintHistory.Add((call.ToString(), _game.BiddingSuggestions[0].Message));
        _game.ProcessCall(call);
    }
    
    public void HandleAnimationComplete() {
        _game.ProceedNextAction();
    }

    public void Dispose() {
        foreach (IPlayer player in _game.Players) {
            player.OnCallChosen -= HandlePlayerCallChosen;
            player.OnCardChosen -= HandlePlayerCardChosen;
        }
        _game.OnPhaseChanged -= HandleGamePhaseChanged;
    }
}