using System;

namespace BridgeEdu.Game.Play.Exceptions {
    public class GameOverException : Exception {
        public GameOverException() : base("Game is over") {
        }
    }
}