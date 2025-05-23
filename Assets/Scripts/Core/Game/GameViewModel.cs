using System;
using System.Collections.ObjectModel;

public class GameViewModel : IDisposable {
    private readonly IGame _game;
    public ReadOnlyCollection<ICall> Calls => _game.Calls;
    public ReadOnlyCollection<IPlayer> Players => _game.Players;
    public BindableProperty<int> CallCount => BindableProperty<int>.Bind(() => _game.Calls.Count);
    public IPlayer CurrentPlayer => _game.CurrentPlayer;
    public Bid HighestBid => _game.HighestBid;
    
    public event Action<ICall> OnCallMade;
    public event Action<GamePhase> OnGamePhaseChanged;
    public GameViewModel(IGame game) {
        _game = game;
        _game.OnCallMade += HandleCallMade;
        _game.OnGamePhaseChanged += HandleGamePhaseChanged;

        foreach (IPlayer player in _game.Players) {
            player.OnCallChosen += HandlePlayerCallChosen;
            player.OnCardChosen += HandlePlayerCardChosen;
        }
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        OnGamePhaseChanged?.Invoke(phase);
    }

    private void HandleCallMade(ICall call) {
        OnCallMade?.Invoke(call);
    }

    private void HandlePlayerCardChosen(Card card) {
        // TODO
    }

    public void HandlePlayerCallChosen(ICall call) {
        _game.ProcessCall(call);
    }

    public void Dispose() {
        foreach (IPlayer player in _game.Players) {
            player.OnCallChosen -= HandlePlayerCallChosen;
            player.OnCardChosen -= HandlePlayerCardChosen;
        }
        _game.OnGamePhaseChanged -= HandleGamePhaseChanged;
        _game.OnCallMade -= HandleCallMade;
    }
}