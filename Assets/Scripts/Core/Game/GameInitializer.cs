using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameInitializer : MonoBehaviour {
    [SerializeField] private GameConfiguration _gameConfig;
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
            computerPlayers.Add(new ComputerPlayer(comPos));
        }
        
        // Deal cards

        Deck deck = new();
        IHandGenerator handGenerator = new HandGenerator();

        List<Card> humanHand = handGenerator.Generate(deck, _gameConfig.BalancedHand, _gameConfig.MinHCP, _gameConfig.MaxHCP);
        humanPlayer.ReceiveCards(humanHand);

        foreach (IPlayer player in computerPlayers) {
            player.ReceiveCards(deck.DealCards(13));
            players.Add(player);
        }

        players.OrderBy(player => player.Position);

        IPlayer dealer = players.Where(player => player.Position == _gameConfig.DealerPosition).First();

        // Start auction

        IAuction auction = new Auction(players, dealer);
        
        auction.StartAuction();
    }
}