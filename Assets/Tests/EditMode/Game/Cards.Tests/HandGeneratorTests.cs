using System.Collections.Generic;
using NUnit.Framework;

public class HandGeneratorTests {
    [Test]
    public void Generate_BalancedHands_IsBalanced() {
        Deck deck = new();
        deck.Shuffle();

        HandGenerator handGenerator = new();
        List<Card> hand = handGenerator.Generate(deck, true);

        HandAnalyzer handAnalyzer = new();

        Assert.IsTrue(handAnalyzer.IsBalancedHand(hand));
    }

    [Test]
    public void Generate_BalancedHands_DeckHas13CardsLess() {
        Deck deck = new();
        int numberOfCards = deck.Cards.Count;
        deck.Shuffle();

        HandGenerator handGenerator = new();
        handGenerator.Generate(deck, true);

        Assert.AreEqual(numberOfCards - 13, deck.Cards.Count);
    }

    [Test]
    public void Generate_BalancedHandsWith15HCP_IsBalanced_AndHas15HCP() {
        Deck deck = new();
        deck.Shuffle();

        HandGenerator handGenerator = new();
        List<Card> hand = handGenerator.Generate(deck, true, 15, 15);

        HandAnalyzer handAnalyzer = new();

        Assert.IsTrue(handAnalyzer.IsBalancedHand(hand));
        Assert.AreEqual(15, handAnalyzer.CalculateHighCardPoints(hand));
    }
}
