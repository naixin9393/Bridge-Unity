using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayView : MonoBehaviour {
    [SerializeField] private GameObject cardPrefab;

    // Place holders for card slots
    [SerializeField] private GameObject bottomCardSlot;
    [SerializeField] private GameObject topCardSlot;
    [SerializeField] private GameObject leftCardSlot;
    [SerializeField] private GameObject rightCardSlot;

    // Place holders for card spawns
    [SerializeField] private GameObject bottomCardSpawn;
    [SerializeField] private GameObject topCardSpawn;
    [SerializeField] private GameObject leftCardSpawn;
    [SerializeField] private GameObject rightCardSpawn;

    // Place holders for card win slots
    [SerializeField] private GameObject bottomCardWinSlot;
    [SerializeField] private GameObject topCardWinSlot;
    [SerializeField] private GameObject leftCardWinSlot;
    [SerializeField] private GameObject rightCardWinSlot;


    private VisualElement _playContainer;
    private GameViewModel _gameViewModel;
    private IPlayer _bottomPlayer;
    private IPlayer _leftPlayer;
    private IPlayer _topPlayer;
    private IPlayer _rightPlayer;
    private VisualElement _bottomHandContainer;

    // Container where played cards are shown
    private readonly List<GameObject> _playedCards = new();
    private readonly List<CardView> _bottomPlayerCardViews = new();

    // GameObjects that are place holders for origin and target positions
    private readonly Dictionary<IPlayer, (GameObject, GameObject, GameObject)> _playerCardSlotSpawnMap = new();
    private int _animationsInProgress = 0;


    public void Initialize(VisualElement playContainer, GameViewModel gameViewModel, IPlayer humanPlayer) {
        _playContainer = playContainer;
        _gameViewModel = gameViewModel;

        var contractLabel = _playContainer.Q<Label>("Contract");
        contractLabel.text = _gameViewModel.Contract.ToString();

        _bottomPlayer = humanPlayer;
        _leftPlayer = PlayerUtils.GetNextPlayer(_bottomPlayer, _gameViewModel.Players.ToList());
        _topPlayer = PlayerUtils.GetNextPlayer(_leftPlayer, _gameViewModel.Players.ToList());
        _rightPlayer = PlayerUtils.GetNextPlayer(_topPlayer, _gameViewModel.Players.ToList());

        _bottomHandContainer = _playContainer.Q<VisualElement>("BottomHandContainer");

        _playerCardSlotSpawnMap.Add(_bottomPlayer, (bottomCardSlot, bottomCardSpawn, bottomCardWinSlot));
        _playerCardSlotSpawnMap.Add(_leftPlayer, (leftCardSlot, leftCardSpawn, leftCardWinSlot));
        _playerCardSlotSpawnMap.Add(_topPlayer, (topCardSlot, topCardSpawn, topCardWinSlot));
        _playerCardSlotSpawnMap.Add(_rightPlayer, (rightCardSlot, rightCardSpawn, rightCardWinSlot));

        PopulatePlayerHandContainer();
        UpdateHandsUI();
    }

    public void HandleTrickEnded(Component sender, object winner) {
        // Disable hand temporarily
        _animationsInProgress++;
        DisableHand();
        var sequence = DOTween.Sequence();
        foreach (GameObject cardObject in _playedCards) {
            sequence.Join(
                cardObject.transform.DOMove(_playerCardSlotSpawnMap[winner as IPlayer].Item3.transform.position, 1.0f)
                    .SetEase(Ease.OutQuad));
        }
        sequence.SetDelay(1)
            .OnComplete(() => {
                _playedCards.Clear();
                _animationsInProgress--;
                UpdateHandsUI();
                HandleAnimationComplete();
            });
    }

    private void DisableHand() {
        foreach (CardView cardView in _bottomPlayerCardViews) {
            cardView.SetEnabled(false);
        }
    }

    public void HandlePlayMade(Component sender, object data) {
        DisableHand();
        if (data is ValueTuple<Card, IPlayer> cardData) {
            AnimateCardToPlayArea(cardData.Item1, cardData.Item2, HandleAnimationComplete);
        }
    }

    private void HandleAnimationComplete() {
        _gameViewModel.HandleAnimationComplete();
    }

    private void UpdateHandsUI() {
        if (_gameViewModel.CurrentPlayer != _bottomPlayer || _animationsInProgress > 0) {
            DisableHand();
            return;
        }
        foreach (CardView cardView in _bottomPlayerCardViews) {
            bool shouldBeEnabled =
                !_gameViewModel.LeadSuit.HasValue ||
                !HasCardsOfSameSuit(_bottomPlayer, _gameViewModel.LeadSuit.Value) ||
                cardView.Card.Suit == _gameViewModel.LeadSuit;
            cardView.SetEnabled(shouldBeEnabled);
        }
    }

    private void PopulatePlayerHandContainer() {
        // Show card in order Spades, Hearts, Diamonds, Clubs
        var playerHand = _bottomPlayer.Hand.ToList()
            .OrderBy(card => card, new BridgeCardComparer(Suit.Spades, Strain.NoTrump))
            .Reverse();
        foreach (var card in playerHand) {
            var cardView = new CardView(card);
            cardView.RegisterCallback<ClickEvent>(evt => {
                _gameViewModel.HandlePlayerCardChosen(card, _bottomPlayer);
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

    private void OnDestroy() {
        foreach (GameObject cardObject in _playedCards) {
            Destroy(cardObject);
        }
    }

    private bool HasCardsOfSameSuit(IPlayer player, Suit leadSuit) {
        return player.Hand.Any(card => card.Suit == leadSuit);
    }

    private void AnimateCardToPlayArea(Card card, IPlayer player, Action animationCompleteCallback) {
        var (targetContainer, originContainer, _) = _playerCardSlotSpawnMap[player];

        // Create card object at origin
        GameObject cardObject = Instantiate(cardPrefab, originContainer.transform.position, Quaternion.identity);
        _playedCards.Add(cardObject);

        // Initialize card UI
        CardUI cardUI = cardObject.GetComponent<CardUI>();
        cardUI.Initialize(card);

        _animationsInProgress++;
        // Animate card to target position
        cardObject.transform.DOMove(targetContainer.transform.position, 1.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                animationCompleteCallback?.Invoke();
                _animationsInProgress--;
                UpdateHandsUI();
            })
            .SetDelay(0.5f);
    }
}
