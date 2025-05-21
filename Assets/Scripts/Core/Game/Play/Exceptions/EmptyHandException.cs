public class EmptyHandException : InvalidPlayException {
    public EmptyHandException(IPlayer player) : base($"Player {player} has no cards in hand") {
    }
}