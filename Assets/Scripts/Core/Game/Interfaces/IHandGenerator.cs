using System.Collections.Generic;

public interface IHandGenerator {
    List<Card> Generate(Deck deck, bool regularHand);
    List<Card> Generate(Deck deck, bool regularHand, int minHCP, int maxHCP);
}