using System;

public class GameOverException : Exception {
    public GameOverException() : base("Game is over") {
    }
}