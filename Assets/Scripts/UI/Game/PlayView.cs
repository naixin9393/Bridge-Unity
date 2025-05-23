using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayView : IDisposable {
    private readonly VisualElement _playContainer;
    private readonly GameViewModel _gameViewModel;
    private readonly IPlayer _player;
    private readonly VisualElement _bottomHandContainer;
    private readonly VisualElement _topCardContainer;
    private readonly VisualElement _rightCardContainer;
    private readonly VisualElement _bottomCardContainer;
    private readonly VisualElement _leftCardContainer;
    private readonly List<CardView> _bottomPlayerCardViews = new();

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
        UpdateHandsUI();
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
        cardContainer.Add(new CardView(card));
        UpdateHandsUI();
        _gameViewModel.ProceedNextAction();
    }

    private void UpdateHandsUI() {
        if (_gameViewModel.CurrentPlayer != _player) {
            foreach (CardView cardView in _bottomPlayerCardViews) {
                cardView.SetEnabled(false);
            }
            return;
        }
        foreach (CardView cardView in _bottomPlayerCardViews) {
            bool shouldBeEnabled = !HasCardsOfSameSuit(_player, _gameViewModel.LeadSuit) ||
                cardView.Card.Suit == _gameViewModel.LeadSuit;
            cardView.SetEnabled(shouldBeEnabled);
        }
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

    private void PopulatePlayerHandContainer() {
        // Show card in order Spades, Hearts, Diamonds, Clubs
        var playerHand = _player.Hand.ToList()
            .OrderBy(card => card, new BridgeCardComparer(Suit.Spades, Strain.NoTrump))
            .Reverse();
        foreach (var card in playerHand) {
            var cardView = new CardView(card);
            cardView.RegisterCallback<ClickEvent>(evt => {
                _gameViewModel.HandlePlayerCardChosen(card, _player);
                _bottomHandContainer.Remove(cardView);
            });
            _bottomHandContainer.Add(cardView);
            _bottomPlayerCardViews.Add(cardView);
        }
    }

    private class CardView : VisualElement {
        public readonly Card Card;
        public CardView(Card card) {
            Card = card;
            style.width = 86;
            style.height = 125;
            style.backgroundImage = LoadImage(card);
            style.marginLeft = 5;
            style.marginRight = 5;
        }
    }

    private static StyleBackground LoadImage(Card card) {
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

    private bool HasCardsOfSameSuit(IPlayer player, Suit leadSuit) {
        return player.Hand.Any(card => card.Suit == leadSuit);
    }
}
