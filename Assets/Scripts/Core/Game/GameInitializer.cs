using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInitializer : MonoBehaviour {
    [SerializeField] private GameConfiguration _gameConfig;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameScreen _gameScreen;

    private void Awake() {

        List<Position> computerPositions = Enum.GetValues(typeof(Position)).Cast<Position>().ToList();
        computerPositions.Remove(_gameConfig.HumanPlayerPosition);

        // Create human player
        IPlayer humanPlayer = new HumanPlayer(_gameConfig.HumanPlayerPosition);

        List<IPlayer> players = new() {
            humanPlayer
        };

        // Create computer players
        List<IPlayer> computerPlayers = new();
        
        foreach (Position comPos in computerPositions) {
            computerPlayers.Add(new ComputerPlayer(comPos, CoroutineRunner.Instance));
        }
        
        // Deal cards

        Deck deck = new();
        deck.Shuffle();

        List<Card> humanHand = HandGenerator.Generate(deck, _gameConfig.BalancedHand, _gameConfig.MinHCP, _gameConfig.MaxHCP);
        humanPlayer.ReceiveCards(humanHand);

        deck.Shuffle();

        foreach (IPlayer player in computerPlayers) {
            player.ReceiveCards(deck.DealCards(13));
            players.Add(player);
        }

        IPlayer dealer = players.Where(player => player.Position == _gameConfig.DealerPosition).First();
        
        // Create bidding engine
        IBiddingEngine biddingEngine = new BiddingEngine();

        _gameManager.Initialize(players, dealer, biddingEngine);

        // Create game view model
        var gameViewModel = new GameViewModel(_gameManager, humanPlayer);

        _gameScreen.Initialize(gameViewModel);
        
        _gameManager.StartGame();
    }
}