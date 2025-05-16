public class NotPlayersTurnException : InvalidPlayException { 
    public NotPlayersTurnException(IPlayer player, IPlayer expectedPlayer) : base($"It's not {player.Position} player's turn, but {expectedPlayer.Position} player's turn") {}
}