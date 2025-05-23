using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsView {
    private GameViewModel _gameViewModel;
    private VisualElement _statsContainer;
    private VisualElement _player1CallsContainer;
    private VisualElement _player2CallsContainer;
    private VisualElement _player3CallsContainer;
    private VisualElement _player4CallsContainer;
    private VisualElement _player1PlaysContainer;
    private VisualElement _player2PlaysContainer;
    private VisualElement _player3PlaysContainer;
    private VisualElement _player4PlaysContainer;
    private TabView _tabView;
    private VisualElement _callsContainer;
    private VisualElement _playsContainer;

    public StatsView(VisualElement statsContainer, GameViewModel gameViewModel) {
        _statsContainer = statsContainer;
        _gameViewModel = gameViewModel;
        _gameViewModel.OnCallMade += HandleCallMade;
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
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        _tabView.selectedTabIndex = 1;
        Debug.Log($"StatsView: GamePhaseChanged: {phase}");
    }

    private void HandleCallMade(ICall call) {
        UpdateCallTable();
    }

    private void UpdateCallTable() {
        ClearCallTable();
        foreach (var call in _gameViewModel.Calls) {
            Label label = new Label(call switch {
                BidCall bidCall => bidCall.Bid.ToString(),
                _ => call.ToString()
            });
            label.AddToClassList("header2");
            GetPlayerActionsContainer(call.Caller).Add(label);
        }
    }

    private VisualElement GetPlayerActionsContainer(IPlayer caller) {
        return _gameViewModel.Players.IndexOf(caller) switch {
            0 => _player1CallsContainer,
            1 => _player2CallsContainer,
            2 => _player3CallsContainer,
            3 => _player4CallsContainer,
            _ => throw new NotImplementedException()
        };
    }

    private void ClearCallTable() {
        _player1CallsContainer.Clear();
        _player2CallsContainer.Clear();
        _player3CallsContainer.Clear();
        _player4CallsContainer.Clear();
    }

    private void SetPlayersLabel() {
        var player1LabelPlays = _playsContainer.Q<Label>("Player1");
        var player2LabelPlays = _playsContainer.Q<Label>("Player2");
        var player3LabelPlays = _playsContainer.Q<Label>("Player3");
        var player4LabelPlays = _playsContainer.Q<Label>("Player4");

        var player1LabelCalls = _callsContainer.Q<Label>("Player1");
        var player2LabelCalls = _callsContainer.Q<Label>("Player2");
        var player3LabelCalls = _callsContainer.Q<Label>("Player3");
        var player4LabelCalls = _callsContainer.Q<Label>("Player4");

        player1LabelPlays.text = _gameViewModel.Players[0].ToString();
        player2LabelPlays.text = _gameViewModel.Players[1].ToString();
        player3LabelPlays.text = _gameViewModel.Players[2].ToString();
        player4LabelPlays.text = _gameViewModel.Players[3].ToString();
        
        player1LabelCalls.text = _gameViewModel.Players[0].ToString();
        player2LabelCalls.text = _gameViewModel.Players[1].ToString();
        player3LabelCalls.text = _gameViewModel.Players[2].ToString();
        player4LabelCalls.text = _gameViewModel.Players[3].ToString();
    }
}