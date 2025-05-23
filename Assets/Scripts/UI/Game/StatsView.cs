using System;
using UnityEngine;
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
    }

    private void HandlePlayMade(Card card, IPlayer player) {
        Label label = new(card.ToString());
        label.AddToClassList("header2");
        GetPlayerPlaysContainer(player).Add(label);
    }

    private VisualElement GetPlayerPlaysContainer(IPlayer player) {
        return _gameViewModel.Players.IndexOf(player) switch {
            0 => _player1PlaysContainer,
            1 => _player2PlaysContainer,
            2 => _player3PlaysContainer,
            3 => _player4PlaysContainer,
            _ => throw new NotImplementedException()
        };
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        _tabView.selectedTabIndex = 1;
        Debug.Log($"StatsView: GamePhaseChanged: {phase}");

        var player1LabelPlays = _playsContainer.Q<Label>("Player1");
        var player2LabelPlays = _playsContainer.Q<Label>("Player2");
        var player3LabelPlays = _playsContainer.Q<Label>("Player3");
        var player4LabelPlays = _playsContainer.Q<Label>("Player4");
        
        

        player1LabelPlays.text = _gameViewModel.Players[0].ToString();
        player2LabelPlays.text = _gameViewModel.Players[1].ToString();
        player3LabelPlays.text = _gameViewModel.Players[2].ToString();
        player4LabelPlays.text = _gameViewModel.Players[3].ToString();
    }

    private void HandleCallMade(ICall call) {
        Label label = new(call switch {
            BidCall bidCall => bidCall.Bid.ToString(),
            _ => call.ToString()
        });
        label.AddToClassList("header2");
        GetPlayerCallsContainer(call.Caller).Add(label);
    }

    private VisualElement GetPlayerCallsContainer(IPlayer caller) {
        return _gameViewModel.Players.IndexOf(caller) switch {
            0 => _player1CallsContainer,
            1 => _player2CallsContainer,
            2 => _player3CallsContainer,
            3 => _player4CallsContainer,
            _ => throw new NotImplementedException()
        };
    }

    private void SetPlayersLabel() {

        var player1LabelCalls = _callsContainer.Q<Label>("Player1");
        var player2LabelCalls = _callsContainer.Q<Label>("Player2");
        var player3LabelCalls = _callsContainer.Q<Label>("Player3");
        var player4LabelCalls = _callsContainer.Q<Label>("Player4");


        player1LabelCalls.text = _gameViewModel.Players[0].ToString();
        player2LabelCalls.text = _gameViewModel.Players[1].ToString();
        player3LabelCalls.text = _gameViewModel.Players[2].ToString();
        player4LabelCalls.text = _gameViewModel.Players[3].ToString();
    }
}