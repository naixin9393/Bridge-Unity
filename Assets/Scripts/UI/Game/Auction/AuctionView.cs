using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AuctionView : MonoBehaviour {
    private VisualElement _auctionHandContainer;
    private VisualElement _auctionContainer;
    private VisualElement _callsContainer;
    private VisualElement _bidsContainer;
    private GameViewModel _gameViewModel;
    private Button _confirmButton;
    private Button _selectedButton;
    private Button _doubleButton;
    private Button _redoubleButton;
    private IPlayer _player;
    private ICall _selectedCall;
    private Dictionary<Button, Bid> _bidButtonMap = new();

    public void Initialize(VisualElement auctionContainer, GameViewModel gameViewModel, IPlayer player) {
        _auctionContainer = auctionContainer;
        _gameViewModel = gameViewModel;
        _player = player;

        _callsContainer = _auctionContainer.Q<VisualElement>("CallsContainer");
        PopulateCallsContainer();

        _confirmButton = _auctionContainer.Q<Button>("ConfirmButton");
        _confirmButton.SetEnabled(false);
        _confirmButton.clicked += () => _gameViewModel.HandlePlayerCallChosen(_selectedCall);

        // Show player's hand HCP
        _auctionContainer.Q<Label>("HCP").text = _player.Hand.HCP.ToString();

        _auctionHandContainer = _auctionContainer.Q<VisualElement>("AuctionHandContainer");
        PopulatePlayerHandContainer();
    }

    private void PopulatePlayerHandContainer() {
        // Show card in order Spades, Hearts, Diamonds, Clubs
        var playerHand = _player.Hand.Cards
            .OrderBy(card => card, new BridgeCardComparer(Suit.Spades, Strain.NoTrump))
            .Reverse();
        foreach (var card in playerHand) {
            var cardView = CreateCardView(card);
            _auctionHandContainer.Add(cardView);
        }
    }

    private VisualElement CreateCardView(Card card) {
        VisualElement cardView = new();
        cardView.style.width = 86;
        cardView.style.height = 125;
        cardView.style.backgroundImage = LoadImage(card);
        cardView.style.marginLeft = 5;
        cardView.style.marginRight = 5;
        return cardView;
    }

    private StyleBackground LoadImage(Card card) {
        string rank;
        if ((int)card.Rank > 10)
            rank = card.Rank.ToString();
        else
            rank = ((int)card.Rank).ToString();
        string textureName = rank.ToLower() + "_" + card.Suit.ToString().ToLower();
        Texture2D cardTexture = Resources.Load<Texture2D>("Cards/" + textureName);
        return new StyleBackground(cardTexture);
    }

    public void HandleCallMade(Component sender, object call) {
        if (call is not ICall) return;
        var buttons = _auctionContainer.Query<Button>().ToList();
        foreach (var button in buttons) {
            button.SetEnabled(_gameViewModel.CurrentPlayer == _player);
        }
        SetSelectedButton(null, null);
        UpdateUI();
        _gameViewModel.HandleAnimationComplete();
    }

    private void UpdateUI() {
        if (_gameViewModel.CurrentPlayer != _player) return;
        UpdateBidButtons();
        UpdateDoubleRedoubleButtons();
        UpdateConfirmButton();
    }

    private void UpdateConfirmButton() {
        if (_selectedCall == null) _confirmButton.SetEnabled(false);
        _confirmButton.SetEnabled(_gameViewModel.CurrentPlayer == _player);
    }

    private void UpdateDoubleRedoubleButtons() {
        var lastCall = _gameViewModel.Calls[^1];
        if (lastCall.Type == CallType.Bid) {
            _doubleButton.SetEnabled(true);
            _redoubleButton.SetEnabled(false);
            return;
        }
        if (lastCall.Type == CallType.Double) {
            _doubleButton.SetEnabled(false);
            _redoubleButton.SetEnabled(true);
            return;
        }
        _redoubleButton.SetEnabled(false);
    }

    private void UpdateBidButtons() {
        foreach (var button in _bidButtonMap.Keys) {
            button.SetEnabled(_gameViewModel == null ||
                _gameViewModel.HighestBid == null ||
                _gameViewModel.HighestBid < _bidButtonMap[button]);
        }
    }

    private void PopulateCallsContainer() {
        _bidsContainer = new VisualElement();
        for (int level = 1; level < 8; level++) {
            var row = CreateCallRowView();
            foreach (Strain strain in Enum.GetValues(typeof(Strain))) {
                row.Add(CreateBidButtonView(level, strain));
            }
            _bidsContainer.Add(row);
        }

        var passDoubleRedoubleContainer = CreateCallRowView();
        passDoubleRedoubleContainer.style.justifyContent = Justify.SpaceBetween;
        passDoubleRedoubleContainer.Add(CreatePassButtonView());
        _doubleButton = CreateDoubleButtonView();
        _redoubleButton = CreateRedoubleButtonView();
        passDoubleRedoubleContainer.Add(_doubleButton);
        passDoubleRedoubleContainer.Add(_redoubleButton);

        _callsContainer.Add(_bidsContainer);
        _callsContainer.Add(passDoubleRedoubleContainer);
    }

    public VisualElement CreateCallRowView() {
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;
        row.style.justifyContent = Justify.Center;
        return row;
    }

    private Button CreatePassButtonView() {
        Button button = new() {
            text = "Pass"
        };
        button.style.color = Color.white;
        button.style.backgroundColor = Color.green;
        SetBasicStyles(button);
        button.style.width = Length.Percent(30);
        button.clicked += () => SetSelectedButton(button, new Pass(_player));
        return button;
    }

    private Button CreateDoubleButtonView() {
        Button button = new() {
            text = "X"
        };
        button.style.color = Color.white;
        button.style.backgroundColor = Color.red;
        SetBasicStyles(button);
        button.style.width = Length.Percent(30);
        button.clicked += () => SetSelectedButton(button, new Double(_player));
        return button;
    }

    private Button CreateRedoubleButtonView() {
        Button button = new() {
            text = "XX"
        };
        button.style.color = Color.white;
        button.style.backgroundColor = Color.blue;
        SetBasicStyles(button);
        button.style.width = Length.Percent(30);
        button.clicked += () => SetSelectedButton(button, new Redouble(_player));
        return button;
    }

    private Button CreateBidButtonView(int level, Strain strain) {
        Button button = new() {
            text = $"{level}{strain.ToSymbol()}"
        };
        button.style.color = strain switch {
            Strain.NoTrump => Color.black,
            Strain.Spades => new Color(0.0f, 0.2f, 0.5f),
            Strain.Hearts => Color.red,
            Strain.Diamonds => new Color(1.0f, 0.4f, 0.0f),
            Strain.Clubs => new Color(0.0f, 0.5f, 0.2f),
            _ => throw new NotImplementedException(),
        };
        SetBasicStyles(button);
        button.style.width = Length.Percent(18);
        button.AddToClassList("button");
        button.clicked += () => SetSelectedButton(button, new BidCall(new Bid(level, strain), _player));
        _bidButtonMap.Add(button, new Bid(level, strain));
        return button;
    }

    private void SetSelectedButton(Button button, ICall call) {
        _selectedButton?.RemoveFromClassList("selected-button");
        _selectedButton = button;
        _selectedButton?.AddToClassList("selected-button");
        _selectedCall = call;
        _confirmButton.SetEnabled(_selectedCall != null);
    }

    private static void SetBasicStyles(Button button) {
        button.style.fontSize = 20;
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        button.style.borderTopLeftRadius = 20;
        button.style.borderTopRightRadius = 20;
        button.style.borderBottomLeftRadius = 20;
        button.style.borderBottomRightRadius = 20;
    }
}