using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour {
    // Visual trees
    [SerializeField] private VisualTreeAsset _gameVisualTree;

    private UIDocument _document;

    // Containers
    private VisualElement _auctionContainer;
    private VisualElement _playContainer;
    private VisualElement _statsContainer;

    private AuctionView _auctionView;
    private StatsView _statsView;
    private GameViewModel _gameViewModel;
    private PlayView _playView;

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
    }

    public void Initialize(GameViewModel gameViewModel) {
        if (_auctionContainer == null)
            Debug.LogError("Auction container not found");

        // Assume human is always south
        var humanPlayer = gameViewModel.Players.First(p => p.Position == Position.South);
        _auctionView = new AuctionView(_auctionContainer, gameViewModel, humanPlayer);
        _playView = new PlayView(_playContainer, gameViewModel, humanPlayer);
        _statsView = new StatsView(_statsContainer, gameViewModel);
        
        _gameViewModel = gameViewModel;

        _gameViewModel.OnGamePhaseChanged += HandleGamePhaseChanged;
    }

    private void HandleGamePhaseChanged(GamePhase phase) {
        switch (phase) {
            case GamePhase.Auction:
                break;
            case GamePhase.Play:
                _auctionView.Dispose();
                _auctionContainer.style.display = DisplayStyle.None;
                _playContainer.style.display = DisplayStyle.Flex;
                _gameViewModel.ProceedNextAction();
                break;
            case GamePhase.End:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
        }
    }
}