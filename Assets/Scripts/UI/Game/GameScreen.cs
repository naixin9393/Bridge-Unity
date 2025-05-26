using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour {
    // Visual trees
    [SerializeField] private AuctionView _auctionView;
    [SerializeField] private StatsView _statsView;
    [SerializeField] private PlayView _playView;
    [SerializeField] private EndView _endView;
    [SerializeField] private GameAbortedView _gameAbortedView;
    [SerializeField] private GameSettingsView _gameSettingsView;
    [SerializeField] private VisualTreeAsset _gameVisualTree;

    private UIDocument _document;

    // Containers
    private VisualElement _auctionContainer;
    private VisualElement _playContainer;
    private VisualElement _statsContainer;
    private Button _settingsButton;
    private IPlayer _humanPlayer;
    private GameViewModel _gameViewModel;

    private void Awake() {
        // Load visual tree
        _document = GetComponent<UIDocument>();
        VisualElement root = _document.rootVisualElement;
        root.Clear();

        VisualElement gameScreenUI = _gameVisualTree.Instantiate();
        gameScreenUI.style.flexGrow = 1;
        root.Add(gameScreenUI);
        _auctionContainer = gameScreenUI.Q<VisualElement>("AuctionTemplateContainer");
        _playContainer = gameScreenUI.Q<VisualElement>("PlayTemplateContainer");
        _playContainer.style.display = DisplayStyle.None;
        _statsContainer = gameScreenUI.Q<VisualElement>("StatsContainer");

        _gameSettingsView.Initialize();

        _settingsButton = gameScreenUI.Q<Button>("SettingsButton");
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
    }

    private void OnSettingsButtonClicked(ClickEvent evt) {
        _gameSettingsView.Show();
    }

    public void Initialize(GameViewModel gameViewModel) {
        if (_auctionContainer == null)
            Debug.LogError("Auction container not found");

        // Assume human is always south
        _humanPlayer = gameViewModel.Players.First(p => p.Position == Position.South);
        _auctionView.Initialize(_auctionContainer, gameViewModel, _humanPlayer);
        _statsView.Initialize(_statsContainer, gameViewModel);

        _gameViewModel = gameViewModel;
    }

    public void HandleGamePhaseChanged(Component sender, object phase) {
        if (phase is GamePhase) {
            switch (phase) {
                case GamePhase.Auction:
                    break;
                case GamePhase.Play:
                    Debug.Log("Play phase started");
                    _auctionContainer.style.opacity = 1.0f;
                    DOTween.To(() => _auctionContainer.style.opacity.value,
                        x => _auctionContainer.style.opacity = x, 0.0f, 1.0f)
                        .OnComplete(() => {
                            _auctionContainer.style.display = DisplayStyle.None;
                            _playContainer.style.opacity = 0.0f;
                            _playContainer.style.display = DisplayStyle.Flex;
                            _playView.Initialize(_playContainer, _gameViewModel, _humanPlayer);
                            DOTween.To(() => _playContainer.style.opacity.value,
                                x => _playContainer.style.opacity = x, 1.0f, 1.0f)
                                .OnComplete(() => _gameViewModel.HandleAnimationComplete());
                        });
                    break;
                case GamePhase.End:
                    Debug.Log("Game ended");
                    if (_gameViewModel.Declarer == null) {
                        _gameAbortedView.Initialize();
                        return;
                    }
                    _endView.Initialize(_gameViewModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }
    }
}