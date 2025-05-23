using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class StatsView {
    private readonly GameViewModel _gameViewModel;
    private readonly VisualElement _statsContainer;
    private readonly VisualElement _player1CallsContainer;
    private readonly VisualElement _player2CallsContainer;
    private readonly VisualElement _player3CallsContainer;
    private readonly VisualElement _player4CallsContainer;
    private readonly VisualElement _player1PlaysContainer;
    private readonly VisualElement _player2PlaysContainer;
    private readonly VisualElement _player3PlaysContainer;
    private readonly VisualElement _player4PlaysContainer;
    private readonly TabView _tabView;
    private readonly VisualElement _callsContainer;
    private readonly VisualElement _playsContainer;
    private IPlayer _player1;
    private IPlayer _player2;
    private IPlayer _player3;
    private IPlayer _player4;
    private Dictionary<Position, VisualElement> _playerCallsContainersMap = new();
    private Dictionary<Position, VisualElement> _playerPlaysContainersMap = new();

    public StatsView(VisualElement statsContainer, GameViewModel gameViewModel) {
        _statsContainer = statsContainer;
        _gameViewModel = gameViewModel;
        _gameViewModel.OnCallMade += HandleCallMade;
        _gameViewModel.OnPlayMade += HandlePlayMade;
        _gameViewModel.OnGamePhaseChanged += HandleGamePhaseChanged;

        _tabView = _statsContainer.Q<TabView>();

        _callsContainer = _statsContainer.Q<VisualElement>("CallsContainer");
        _playsContainer = _statsContainer.Q<VisualElement>("PlaysContainer");

        SetPlayersLabel();

        _player1CallsContainer = _callsContainer.Q<VisualElement>("Player1Calls");
        _player2CallsContainer = _callsContainer.Q<VisualElement>("Player2Calls");
        _player3CallsContainer = _callsContainer.Q<VisualElement>("Player3Calls");
        _player4CallsContainer = _callsContainer.Q<VisualElement>("Player4Calls");

        _player1PlaysContainer = _playsContainer.Q<VisualElement>("Player1Plays");
        _player2PlaysContainer = _playsContainer.Q<VisualElement>("Player2Plays");
        _player3PlaysContainer = _playsContainer.Q<VisualElement>("Player3Plays");
        _player4PlaysContainer = _playsContainer.Q<VisualElement>("Player4Plays");
        
        _playerCallsContainersMap.Add(_player1.Position, _player1CallsContainer);
        _playerCallsContainersMap.Add(_player2.Position, _player2CallsContainer);
        _playerCallsContainersMap.Add(_player3.Position, _player3CallsContainer);
        _playerCallsContainersMap.Add(_player4.Position, _player4CallsContainer);

        _playerPlaysContainersMap.Add(_player1.Position, _player1PlaysContainer);
        _playerPlaysContainersMap.Add(_player2.Position, _player2PlaysContainer);
        _playerPlaysContainersMap.Add(_player3.Position, _player3PlaysContainer);        
        _playerPlaysContainersMap.Add(_player4.Position, _player4PlaysContainer);
    }

    private void HandlePlayMade(Card card, IPlayer player) {
        Label label = new(card.ToString());
        label.AddToClassList("header2");
        _playerPlaysContainersMap[player.Position].Add(label);
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        _tabView.selectedTabIndex = 1;
    }

    private void HandleCallMade(ICall call) {
        Label label = new(call switch {
            BidCall bidCall => bidCall.Bid.ToString(),
            _ => call.ToString()
        });
        label.AddToClassList("header2");
        _playerCallsContainersMap[call.Caller.Position].Add(label);
    }

    private void SetPlayersLabel() {

        var player1LabelCalls = _callsContainer.Q<Label>("Player1");
        var player2LabelCalls = _callsContainer.Q<Label>("Player2");
        var player3LabelCalls = _callsContainer.Q<Label>("Player3");
        var player4LabelCalls = _callsContainer.Q<Label>("Player4");
        var player1LabelPlays = _playsContainer.Q<Label>("Player1");
        var player2LabelPlays = _playsContainer.Q<Label>("Player2");
        var player3LabelPlays = _playsContainer.Q<Label>("Player3");
        var player4LabelPlays = _playsContainer.Q<Label>("Player4");

        _player1 = _gameViewModel.CurrentPlayer;
        _player2 = PlayerUtils.GetNextPlayer(_player1, _gameViewModel.Players.ToList());
        _player3 = PlayerUtils.GetNextPlayer(_player2, _gameViewModel.Players.ToList());
        _player4 = PlayerUtils.GetNextPlayer(_player3, _gameViewModel.Players.ToList());

        player1LabelCalls.text = _player1.ToString();
        player2LabelCalls.text = _player2.ToString();
        player3LabelCalls.text = _player3.ToString();
        player4LabelCalls.text = _player4.ToString();
        player1LabelPlays.text = _player1.ToString();
        player2LabelPlays.text = _player2.ToString();
        player3LabelPlays.text = _player3.ToString();
        player4LabelPlays.text = _player4.ToString();
    }
}