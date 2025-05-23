public class NotFollowingLeadException : InvalidPlayException {
    public NotFollowingLeadException(IPlayer player, Suit suit) : base($"Player {player} does not follow lead of {suit}") { }
}
    