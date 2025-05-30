using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsView : MonoBehaviour {
    [SerializeField] private BiddingHintView _biddingHintView;
    [SerializeField] private PlayHintView _playHintView;
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
    private IPlayer _player1;
    private IPlayer _player2;
    private IPlayer _player3;
    private IPlayer _player4;
    private Dictionary<Position, VisualElement> _playerCallsContainersMap = new();
    private Dictionary<Position, VisualElement> _playerPlaysContainersMap = new();
    private Dictionary<VisualElement, (string call, string message)> _biddingHintsMap = new();
    private Button _hintButton;

    public void Initialize(VisualElement statsContainer, GameViewModel gameViewModel) {
        _statsContainer = statsContainer;
        _gameViewModel = gameViewModel;

        _tabView = _statsContainer.Q<TabView>();

        _callsContainer = _statsContainer.Q<VisualElement>("CallsContainer");
        _playsContainer = _statsContainer.Q<VisualElement>("PlaysContainer");
        
        _callsContainer.RegisterCallback<ClickEvent>(OnPlaysContainerClicked);

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
        
        _biddingHintView.Initialize();
        _playHintView.Initialize();
        
        _hintButton = _statsContainer.Q<Button>("HintButton");
        _hintButton.RegisterCallback<ClickEvent>(OnHintButtonClicked);
    }

    private void OnPlaysContainerClicked(ClickEvent evt) {
        var target = evt.target as VisualElement;
        if (!_biddingHintsMap.ContainsKey(target)) return;
        var call = _biddingHintsMap[target].call;
        var message = _biddingHintsMap[target].message;
        _biddingHintView.Set(call, message);
        _biddingHintView.Show();
    }

    private void OnHintButtonClicked(ClickEvent evt) {
        if (_gameViewModel.Phase == GamePhase.Play) {
            _playHintView.Set(
                card: "A C",
                message: "Test"
            );
            _playHintView.Show();
            return;
        }
        _biddingHintView.Set(
            call: _gameViewModel.BiddingHintCall.Value,
            message: _gameViewModel.BiddingHintMessage.Value
        );
        _biddingHintView.Show();
    }
    
    public void HandleTrickEnded(Component sender, object winner) {
        if (winner is IPlayer) {
            var player = winner as IPlayer;
            var winnerPlaysContainer = _playerPlaysContainersMap[player.Position];
            var lastLabel = winnerPlaysContainer.Query<Label>().Last();
            lastLabel.AddToClassList("underline");
        }
    }

    public void HandlePlayMade(Component sender, object data) {
        if (data is ValueTuple<Card, IPlayer> cardData) {
            var card = cardData.Item1;
            Label label = new(card.ToString());
            label.AddToClassList("header2");

            switch (card.Suit) {
                case Suit.Clubs:
                    label.AddToClassList("clubs");
                    break;
                case Suit.Diamonds:
                    label.AddToClassList("diamonds");
                    break;
                case Suit.Hearts:
                    label.AddToClassList("hearts");
                    break;
                case Suit.Spades:
                    label.AddToClassList("spades");
                    break;
            }

            _playerPlaysContainersMap[cardData.Item2.Position].Add(label);
        }
    }

    public void HandleGamePhaseChanged(Component sender, object phase) {
        if (phase is GamePhase gamePhase) {
            DOTween.To(() => _tabView.selectedTabIndex,
                x => _tabView.selectedTabIndex = x, 1, 2f)
                .SetEase(Ease.OutQuad);
        }
    }

    public void HandleCallMade(Component sender, object call) {
        if (call is not ICall) return;
        var callData = call as ICall;
        var callString = callData switch {
            BidCall bidCall => bidCall.Bid.ToString(),
            _ => callData.ToString()
        };

        Label label = new(callString);

        if (callData.Type == CallType.Bid) {
            BidCall bidCall = callData as BidCall;
            switch (bidCall.Bid.Strain) { 
                case Strain.Clubs:
                    label.AddToClassList("clubs");
                    break;
                case Strain.Diamonds:
                    label.AddToClassList("diamonds");
                    break;
                case Strain.Hearts:
                    label.AddToClassList("hearts");
                    break;
                case Strain.Spades:
                    label.AddToClassList("spades");
                    break;
            }
        }
        
        _biddingHintsMap.Add(label, (callString, _gameViewModel.BiddingHintHistory[_biddingHintsMap.Count].message));

        label.AddToClassList("header2");
        label.AddToClassList("clickable");
        _playerCallsContainersMap[callData.Caller.Position].Add(label);
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