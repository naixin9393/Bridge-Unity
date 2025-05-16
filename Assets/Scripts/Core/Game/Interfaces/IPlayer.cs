using System.Collections.Generic;
using System.Collections.ObjectModel;

public interface IPlayer {
    Position Position { get; }
    ReadOnlyCollection<Card> Hand { get; }
    void ReceiveCards(List<Card> cards);
    void PlayCard(Card card);
}