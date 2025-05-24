using System;
using System.Collections.ObjectModel;

public class GameViewModel : IDisposable {
    private readonly IGameManager _game;
    public ReadOnlyCollection<ICall> Calls => _game.Calls;
    public ReadOnlyCollection<IPlayer> Players => _game.Players;
    public BindableProperty<int> CallCount => BindableProperty<int>.Bind(() => _game.Calls.Count);
    public IPlayer CurrentPlayer => _game.CurrentPlayer;
    public Bid HighestBid => _game.HighestBid;
    public Suit? LeadSuit => _game.LeadSuit;
    public Bid Contract => _game.Contract;
    
    public GameViewModel(IGameManager game) {
        _game = game;

        foreach (IPlayer player in _game.Players) {
            player.OnCallChosen += HandlePlayerCallChosen;
            player.OnCardChosen += HandlePlayerCardChosen;
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
    }
}