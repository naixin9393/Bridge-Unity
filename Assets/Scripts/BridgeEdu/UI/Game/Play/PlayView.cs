using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

using BridgeEdu.Core;
using BridgeEdu.Game;
using BridgeEdu.Game.Play;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Players;
using BridgeEdu.Utils;

using PlayerPosition = BridgeEdu.Game.Players.Position;

namespace BridgeEdu.UI.Game {
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
        private IPlayer _dummyPlayer;
        private IPlayer _humanPlayer;
        private IPlayer _bottomPlayer;
        private IPlayer _leftPlayer;
        private IPlayer _topPlayer;
        private IPlayer _rightPlayer;

        private VisualElement _topHandContainer;
        private VisualElement _leftHandContainer;
        private VisualElement _bottomHandContainer;
        private VisualElement _rightHandContainer;
        private VisualElement _dummyHandContainer;


        // Container where played cards are shown
        private readonly List<GameObject> _playedCards = new();
        private readonly List<CardView> _humanPlayerCardViews = new();
        private readonly List<CardView> _dummyPlayerCardViews = new();

        // GameObjects that are place holders for origin and target positions
        private readonly Dictionary<IPlayer, (GameObject, GameObject, GameObject)> _playerCardSlotSpawnMap = new();

        // Dictionary that map position to the hand container
        private readonly Dictionary<PlayerPosition, VisualElement> _positionHandContainersMap = new();
        private int _animationsInProgress = 0;
        private Label _wonTricksLabel;

        private BindableProperty<DisplayStyle> _dummyHandDisplayStyle => BindableProperty<DisplayStyle>.Bind(() => {
            var ShowDummyHand = _gameViewModel.ShowDummyHand.Value;
            return ShowDummyHand ? DisplayStyle.Flex : DisplayStyle.None;
        });
        public void Initialize(VisualElement playContainer, GameViewModel gameViewModel, IPlayer humanPlayer) {
            _playContainer = playContainer;
            _gameViewModel = gameViewModel;

            var contractLabel = _playContainer.Q<Label>("Contract");
            contractLabel.text = _gameViewModel.Contract.ToString();

            // Assign players to position
            _humanPlayer = humanPlayer;
            _bottomPlayer = humanPlayer;
            _leftPlayer = PlayerUtils.GetNextPlayer(_bottomPlayer, _gameViewModel.Players.ToList());
            _topPlayer = PlayerUtils.GetNextPlayer(_leftPlayer, _gameViewModel.Players.ToList());
            _rightPlayer = PlayerUtils.GetNextPlayer(_topPlayer, _gameViewModel.Players.ToList());

            // Query hand containers
            _bottomHandContainer = _playContainer.Q<VisualElement>("BottomHandContainer");
            _leftHandContainer = _playContainer.Q<VisualElement>("LeftHandContainer");
            _topHandContainer = _playContainer.Q<VisualElement>("TopHandContainer");
            _rightHandContainer = _playContainer.Q<VisualElement>("RightHandContainer");

            // Assign hand container to position
            _positionHandContainersMap.Add(_bottomPlayer.Position, _bottomHandContainer);
            _positionHandContainersMap.Add(_leftPlayer.Position, _leftHandContainer);
            _positionHandContainersMap.Add(_topPlayer.Position, _topHandContainer);
            _positionHandContainersMap.Add(_rightPlayer.Position, _rightHandContainer);

            _playerCardSlotSpawnMap.Add(_bottomPlayer, (bottomCardSlot, bottomCardSpawn, bottomCardWinSlot));
            _playerCardSlotSpawnMap.Add(_leftPlayer, (leftCardSlot, leftCardSpawn, leftCardWinSlot));
            _playerCardSlotSpawnMap.Add(_topPlayer, (topCardSlot, topCardSpawn, topCardWinSlot));
            _playerCardSlotSpawnMap.Add(_rightPlayer, (rightCardSlot, rightCardSpawn, rightCardWinSlot));

            var bottomPositionLabel = _playContainer.Q<Label>("BottomPosition");
            bottomPositionLabel.text = _bottomPlayer.Position.ToString();

            var leftPositionLabel = _playContainer.Q<Label>("LeftPosition");
            leftPositionLabel.text = _leftPlayer.Position.ToString();

            var topPositionLabel = _playContainer.Q<Label>("TopPosition");
            topPositionLabel.text = _topPlayer.Position.ToString();

            var rightPositionLabel = _playContainer.Q<Label>("RightPosition");
            rightPositionLabel.text = _rightPlayer.Position.ToString();

            _wonTricksLabel = _playContainer.Q<Label>("WonTricks");

            _wonTricksLabel.dataSource = _gameViewModel.WonTricksCount;
            _wonTricksLabel.SetBinding(nameof(Label.text), new DataBinding {
                dataSourcePath = new PropertyPath(nameof(_gameViewModel.WonTricksCount.Value)),
                bindingMode = BindingMode.ToTarget
            });

            if (_humanPlayer == _gameViewModel.Dummy) {
                _dummyPlayer = PlayerUtils.PartnerOf(_humanPlayer, _gameViewModel.Players.ToList());
            }
            else {
                _dummyPlayer = _gameViewModel.Dummy;
            }
            _dummyHandContainer = _positionHandContainersMap[_dummyPlayer.Position];

            _dummyHandContainer.dataSource = _dummyHandDisplayStyle;
            _dummyHandContainer.SetBinding("style.display", new DataBinding {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<DisplayStyle>.Value)),
                bindingMode = BindingMode.ToTarget
            });

            PopulatePlayerHandContainer();
            UpdateHandsUI();
        }

        public void HandleTrickEnded(Component sender, object winner) {
            // Disable hand temporarily
            _animationsInProgress++;
            DisableHands();
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

        private void DisableHands() {
            DisableHand(_humanPlayerCardViews);
            DisableHand(_dummyPlayerCardViews);
        }

        private void DisableHand(List<CardView> playerCardViews) {
            foreach (CardView cardView in playerCardViews) {
                cardView.SetEnabled(false);
                cardView.RemoveFromClassList("clickable");
            }
        }

        public void HandlePlayMade(Component sender, object data) {
            DisableHands();
            if (data is ValueTuple<Card, IPlayer> cardData) {
                var (card, player) = cardData;
                if (player == _humanPlayer || player == _dummyPlayer) {
                    var playerCardViews = player == _humanPlayer ? _humanPlayerCardViews : _dummyPlayerCardViews;
                    var cardToRemove = playerCardViews.First(cardView => cardView.Card == card);
                    cardToRemove.style.display = DisplayStyle.None;
                }
                AnimateCardToPlayArea(card, player, HandleAnimationComplete);
            }
        }

        private void HandleAnimationComplete() {
            _gameViewModel.HandleAnimationComplete();
        }

        private void UpdateHandsUI() {
            UpdateHandUI(_humanPlayer, _humanPlayerCardViews);
            UpdateHandUI(_dummyPlayer, _dummyPlayerCardViews);
        }

        private void UpdateHandUI(IPlayer player, List<CardView> playerCardViews) {
            if (_gameViewModel.CurrentPlayer != player
                || _animationsInProgress > 0) {
                DisableHand(playerCardViews);
                return;
            }
            if (player == _humanPlayer ||
                player == _dummyPlayer && !PlayerUtils.OnDifferentTeam(_dummyPlayer, _humanPlayer) ||
                player == _gameViewModel.Declarer && _humanPlayer == _dummyPlayer) {
                EnablePossibleCardInHand(player, playerCardViews);
                return;
            }
            DisableHand(playerCardViews);
        }

        private void EnablePossibleCardInHand(IPlayer player, List<CardView> playerCardViews) {
            foreach (CardView cardView in playerCardViews) {
                bool shouldBeEnabled =
                    !_gameViewModel.LeadSuit.HasValue ||
                    !HasCardsOfSameSuit(player, _gameViewModel.LeadSuit.Value) ||
                    cardView.Card.Suit == _gameViewModel.LeadSuit;
                cardView.SetEnabled(shouldBeEnabled);
                if (!shouldBeEnabled)
                    cardView.RemoveFromClassList("clickable");
                else
                    cardView.AddToClassList("clickable");
            }
        }

        private void PopulatePlayerHandContainer() {
            // Insert card for human
            PopulateHand(
                hand: _humanPlayer.Hand.Cards,
                handContainer: _bottomHandContainer,
                player: _humanPlayer,
                canChooseCard: true,
                playerCardViews: _humanPlayerCardViews
            );

            PopulateHand(
                hand: _dummyPlayer.Hand.Cards,
                handContainer: _dummyHandContainer,
                player: _dummyPlayer,
                canChooseCard: !PlayerUtils.OnDifferentTeam(_humanPlayer, _dummyPlayer),
                playerCardViews: _dummyPlayerCardViews
            );
        }

        private void PopulateHand(
            List<Card> hand,
            VisualElement handContainer,
            IPlayer player,
            bool canChooseCard,
            List<CardView> playerCardViews
        ) {
            var sortedHand = hand.OrderBy(card => card, new BridgeCardComparer(StrainToSuit(_gameViewModel.Contract.Strain), _gameViewModel.Contract.Strain)).Reverse();
            var spadesContainer = new VisualElement();
            var heartsContainer = new VisualElement();
            var diamondsContainer = new VisualElement();
            var clubsContainer = new VisualElement();

            spadesContainer.AddToClassList("flex-row");
            spadesContainer.AddToClassList("margin-top-bottom10");
            heartsContainer.AddToClassList("flex-row");
            heartsContainer.AddToClassList("margin-top-bottom10");
            diamondsContainer.AddToClassList("flex-row");
            diamondsContainer.AddToClassList("margin-top-bottom10");
            clubsContainer.AddToClassList("flex-row");
            clubsContainer.AddToClassList("margin-top-bottom10");

            foreach (var card in sortedHand) {
                var cardView = new CardView(card);
                if (player == _leftPlayer || player == _rightPlayer) {
                    if (card.Suit == Suit.Spades)
                        spadesContainer.Add(cardView);
                    else if (card.Suit == Suit.Hearts)
                        heartsContainer.Add(cardView);
                    else if (card.Suit == Suit.Diamonds)
                        diamondsContainer.Add(cardView);
                    else if (card.Suit == Suit.Clubs)
                        clubsContainer.Add(cardView);
                    // Make card smaller
                    const float scaleFactor = 0.8f;
                    float currentWidth = cardView.style.width.value.value;
                    float currentHeight = cardView.style.height.value.value;

                    float newWidth = currentWidth * scaleFactor;
                    float newHeight = currentHeight * scaleFactor;

                    cardView.style.width = new StyleLength(newWidth);
                    cardView.style.height = new StyleLength(newHeight);

                }
                if (canChooseCard) {
                    cardView.RegisterCallback<ClickEvent>(evt => _gameViewModel.HandlePlayerCardChosen(card, player));
                    cardView.AddToClassList("clickable");
                }
                if (player == _bottomPlayer || player == _topPlayer)
                    handContainer.Add(cardView);
                playerCardViews.Add(cardView);
            }

            if (player == _leftPlayer || player == _rightPlayer) {
                handContainer.Add(spadesContainer);
                handContainer.Add(heartsContainer);
                handContainer.Add(clubsContainer);
                handContainer.Add(diamondsContainer);
            }
        }

        private Suit StrainToSuit(Strain strain) {
            return strain switch {
                Strain.NoTrump => Suit.Spades,
                Strain.Spades => Suit.Spades,
                Strain.Hearts => Suit.Hearts,
                Strain.Diamonds => Suit.Diamonds,
                Strain.Clubs => Suit.Clubs,
                _ => throw new ArgumentOutOfRangeException(nameof(strain), strain, null)
            };
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
            return player.Hand.HasCardOfSuit(leadSuit);
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
}