using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayView : IDisposable {
    private VisualElement _playContainer;
    private GameViewModel _gameViewModel;
    private IPlayer _player;
    private VisualElement _bottomHandContainer;
    private VisualElement _topCardContainer;
    private VisualElement _rightCardContainer;
    private VisualElement _bottomCardContainer;
    private VisualElement _leftCardContainer;

    public PlayView(VisualElement playContainer, GameViewModel gameViewModel, IPlayer player) {
        _playContainer = playContainer;
        _gameViewModel = gameViewModel;
        _player = player;


        _gameViewModel.OnPlayMade += HandlePlayMade;
        _gameViewModel.OnTrickEnded += HandleTrickEnded;

        _topCardContainer = _playContainer.Q<VisualElement>("TopCardContainer");
        _rightCardContainer = _playContainer.Q<VisualElement>("RightCardContainer");
        _bottomCardContainer = _playContainer.Q<VisualElement>("BottomCardContainer");
        _leftCardContainer = _playContainer.Q<VisualElement>("LeftCardContainer");
        
        _bottomHandContainer = _playContainer.Q<VisualElement>("BottomHandContainer");

        PopulatePlayerHandContainer();
    }

    private void HandleTrickEnded() {
        _topCardContainer.Clear();
        _rightCardContainer.Clear();
        _bottomCardContainer.Clear();
        _leftCardContainer.Clear();
        _gameViewModel.ProceedNextAction();
    }

    private void HandlePlayMade(Card card, IPlayer player) {
        var cardContainer = GetPlayerPlaysContainer(player);
        cardContainer.Clear();
        cardContainer.Add(CreateCardView(card));
        _gameViewModel.ProceedNextAction();
    }

    private VisualElement GetPlayerPlaysContainer(IPlayer player) {
        return player.Position switch {
            Position.North => _topCardContainer,
            Position.East => _rightCardContainer,
            Position.South => _bottomCardContainer,
            Position.West => _leftCardContainer,
            _ => throw new NotImplementedException()
        };
    }

    // Copied from AuctionView
    private void PopulatePlayerHandContainer() {
        // Show card in order Spades, Hearts, Diamonds, Clubs
        var playerHand = _player.Hand.ToList()
            .OrderBy(card => card, new BridgeCardComparer(Suit.Spades, Strain.NoTrump))
            .Reverse();
        foreach (var card in playerHand) {
            var cardView = CreateCardView(card);
            cardView.RegisterCallback<ClickEvent>(evt => {
                _gameViewModel.HandlePlayerCardChosen(card, _player);
                _bottomHandContainer.Remove(cardView);
                });
            _bottomHandContainer.Add(cardView);
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

    public void Dispose() {
        _gameViewModel.OnPlayMade -= HandlePlayMade;
    }
}
