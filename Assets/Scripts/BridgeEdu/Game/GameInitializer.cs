using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

using BridgeEdu.Utils;
using BridgeEdu.Core;
using BridgeEdu.Game.Players;
using BridgeEdu.UI.Game;
using BridgeEdu.Engines;
using BridgeEdu.Engines.Bidding;
using BridgeEdu.Engines.Bidding.OneNT;

using BiddingStrategy = BridgeEdu.Engines.Bidding;
using PlayingStrategy = BridgeEdu.Engines.Play;
using BridgeEdu.Engines.Play;

namespace BridgeEdu.Game {
    public class GameInitializer : MonoBehaviour {
        [SerializeField] private GameConfiguration _gameConfig;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameScreen _gameScreen;

        private void Awake() {

            List<Position> computerPositions = Enum.GetValues(typeof(Position)).Cast<Position>().ToList();
            computerPositions.Remove(_gameConfig.HumanPlayerPosition);

            // Create human player
            IPlayer humanPlayer = new HumanPlayer(_gameConfig.HumanPlayerPosition);

            // Create computer players
            List<IPlayer> computerPlayers = new();

            foreach (Position comPos in computerPositions) {
                computerPlayers.Add(new ComputerPlayer(comPos, CoroutineRunner.Instance));
            }


            // Create deck
            Deck deck = new();
            deck.Shuffle();

            // Give cards to human player
            IHand humanHand = HandGenerator.Generate(deck, _gameConfig.BalancedHand, _gameConfig.MinHCP, _gameConfig.MaxHCP);
            humanPlayer.ReceiveCards(humanHand.Cards);

            deck.RemoveCards(humanHand.Cards);

            Assert.AreEqual(52 - 13, deck.Cards.Count);

            deck.Shuffle();

            // Identify players
            IPlayer partnerPlayer = PlayerUtils.PartnerOf(humanPlayer, computerPlayers);
            IPlayer rightComputer = PlayerUtils.GetNextPlayer(humanPlayer, computerPlayers);
            IPlayer leftComputer = PlayerUtils.GetNextPlayer(partnerPlayer, computerPlayers);

            List<IPlayer> players = new() { humanPlayer, rightComputer, partnerPlayer, leftComputer };

            // Give cards to computer players
            IHand partnerHand = HandGenerator.Generate(deck, _gameConfig.PartnerBalancedHand, _gameConfig.PartnerMinHCP, _gameConfig.PartnerMaxHCP);
            partnerPlayer.ReceiveCards(partnerHand.Cards);

            deck.RemoveCards(partnerHand.Cards);

            Assert.AreEqual(39 - 13, deck.Cards.Count);

            rightComputer.ReceiveCards(deck.DealCards(13));
            leftComputer.ReceiveCards(deck.DealCards(13));

            // Set dealer
            IPlayer dealer = players.Where(player => player.Position == _gameConfig.DealerPosition).First();

            // Create bidding strategies list
            List<IBiddingStrategy> biddingStrategies = new() {
                new BiddingStrategy.OpenerStrategy(),
                new NoInterventionStrategy(),
                new OneNTResponseStrategy(),
                new OneNTOpenerResponseStrategy(),
                new TransferStrategy(),
                new TransferResponseStrategy(),
                new TransferOpenerResponseStrategy(),
                new StaymanResponseStrategy(),
                new StaymanOpenerResponseStrategy(),
                new StaymanSecondResponseStrategy(),
            };

            // Create bidding engine
            IBiddingEngine biddingEngine = new BiddingEngine(biddingStrategies);

            // Create playing strategies list
            List<IPlayingStrategy> playingStrategies = new() {
                new PlayingStrategy.OpenerStrategy(),
                new OnlyOneCardPossibleStrategy(),
                new NTDiscardStrategy(),
            };

            // Create playing engine
            IPlayingEngine playingEngine = new PlayingEngine(playingStrategies);

            // Initialize game manager
            _gameManager.Initialize(players, dealer, humanPlayer, biddingEngine, playingEngine);

            // Create game view model
            var gameViewModel = new GameViewModel(_gameManager, humanPlayer);

            // Initialize game screen
            _gameScreen.Initialize(gameViewModel);

            // Start game
            _gameManager.StartGame();
        }
    }
}