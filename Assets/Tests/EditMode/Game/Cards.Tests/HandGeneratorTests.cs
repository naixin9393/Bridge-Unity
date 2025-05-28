using System.Collections.Generic;
using NUnit.Framework;

public class HandGeneratorTests {
    [Test]
    public void Generate_BalancedHands_IsBalanced() {
        Deck deck = new();
        deck.Shuffle();

        IHand hand = HandGenerator.Generate(deck, true);

        Assert.IsTrue(HandUtils.IsBalancedHand(hand));
    }

    [Test]
    public void Generate_BalancedHands_DeckHas13CardsLess() {
        Deck deck = new();
        int numberOfCards = deck.Cards.Count;
        deck.Shuffle();

        HandGenerator.Generate(deck, true);

        Assert.AreEqual(numberOfCards - 13, deck.Cards.Count);
    }

    [Test]
    public void Generate_BalancedHandsWith15HCP_IsBalanced_AndHas15HCP() {
        Deck deck = new();
        deck.Shuffle();

        IHand hand = HandGenerator.Generate(deck, true, 15, 15);

        Assert.IsTrue(HandUtils.IsBalancedHand(hand));
        Assert.AreEqual(15, hand.HCP);
    }
}
