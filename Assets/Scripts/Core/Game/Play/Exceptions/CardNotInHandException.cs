public class CardNotInHandException : InvalidPlayException {
    public CardNotInHandException(Card card, IPlayer player) : base($"Card {card} is not in {player.Position}'s hand") { }
}