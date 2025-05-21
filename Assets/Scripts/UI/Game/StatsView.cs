using System;
using UnityEngine.UIElements;

public class StatsView {
    private GameViewModel _gameViewModel;
    private VisualElement _statsContainer;
    private VisualElement _player1ActionsContainer;
    private VisualElement _player2ActionsContainer;
    private VisualElement _player3ActionsContainer;
    private VisualElement _player4ActionsContainer;
    public StatsView(VisualElement statsContainer, GameViewModel gameViewModel) {
        _statsContainer = statsContainer;
        _gameViewModel = gameViewModel;
        _gameViewModel.OnCallMade += OnCallMade;
        SetPlayerLabel();

        _player1ActionsContainer = _statsContainer.Q<VisualElement>("Player1Actions");
        _player2ActionsContainer = _statsContainer.Q<VisualElement>("Player2Actions");
        _player3ActionsContainer = _statsContainer.Q<VisualElement>("Player3Actions");
        _player4ActionsContainer = _statsContainer.Q<VisualElement>("Player4Actions");
    }

    private void OnCallMade(ICall call) {
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
            0 => _player1ActionsContainer,
            1 => _player2ActionsContainer,
            2 => _player3ActionsContainer,
            3 => _player4ActionsContainer,
            _ => throw new NotImplementedException()
        };
    }

    private void ClearCallTable() {
        _player1ActionsContainer.Clear();
        _player2ActionsContainer.Clear();
        _player3ActionsContainer.Clear();
        _player4ActionsContainer.Clear();
    }

    private void SetPlayerLabel() {
        var player1Label = _statsContainer.Q<Label>("Player1");
        var player2Label = _statsContainer.Q<Label>("Player2");
        var player3Label = _statsContainer.Q<Label>("Player3");
        var player4Label = _statsContainer.Q<Label>("Player4");

        player1Label.text = _gameViewModel.Players[0].ToString();
        player2Label.text = _gameViewModel.Players[1].ToString();
        player3Label.text = _gameViewModel.Players[2].ToString();
        player4Label.text = _gameViewModel.Players[3].ToString();
    }
}
/*
        player4Label.dataSource = _gameViewModel.CallCount;
        player4Label.SetBinding(nameof(Label.text), new DataBinding {
            dataSourcePath = new PropertyPath(nameof(BindableProperty<int>.Value)),
            bindingMode = BindingMode.ToTarget
        });
        */