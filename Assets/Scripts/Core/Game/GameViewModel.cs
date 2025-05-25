using System;
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
    public BindableProperty<int> WonTricksCount => BindableProperty<int>.Bind(() => {
        var declarer = _game.Declarer;
        var partnerOfDeclarer = PlayerUtils.PartnerOf(declarer, _game.Players.ToList());

        bool isPlayerAttacker = _humanPlayer == declarer || _humanPlayer == partnerOfDeclarer;

        Debug.Log("Declarer: " + declarer);
        Debug.Log("Partner of declarer: " + partnerOfDeclarer);
        Debug.Log("Is player attacker: " + isPlayerAttacker);

        return isPlayerAttacker ?
            _game.TricksWonByAttackers :
            _game.Tricks.Count - 1 - _game.TricksWonByAttackers;
    });

    public int TricksNeededToWin;

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