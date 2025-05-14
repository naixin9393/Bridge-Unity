using System.Collections.Generic;
using NUnit.Framework;

public class HandAnalyzerTests {
    [Test]
    public void CalculateHighCardPoints_Returns0_IfHandIsEmpty() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card>();
        var result = handAnalyzer.CalculateHighCardPoints(hand);
        Assert.AreEqual(0, result);
    }

    [Test]
    public void CalculateHighCardPoints_Returns10_IfHandContainsAceKingQueenJack() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs)
        };
        var result = handAnalyzer.CalculateHighCardPoints(hand);
        Assert.AreEqual(10, result);
    }

    [Test]
    public void CalculateHighCardPoints_Returns8_IfHandContainsAceKingJack() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs)
        };
        var result = handAnalyzer.CalculateHighCardPoints(hand);
        Assert.AreEqual(8, result);
    }

    [Test]
    public void IsBalancedHand_ReturnsFalse_IfHandHasMoreThan13Cards() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Clubs),
            new(Rank.Seven, Suit.Clubs),
            new(Rank.Six, Suit.Clubs),
            new(Rank.Five, Suit.Clubs),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Three, Suit.Clubs),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Ace, Suit.Spades)
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }
    
    [Test]
    public void IsBalancedHand_ReturnsFalse_IfHandHasLessThan13Cards() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void IsBalancedHand_ReturnsTrue_IfHand_Is5332() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }
    
    [Test]
    public void IsBalancedHand_ReturnsTrue_IfHand_Is4432() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }

    [Test]
    public void IsBalancedHand_ReturnsTrue_IfHand_Is4333() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Spades),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts),
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }

    [Test] 
    public void IsBalancedHand_ReturnsFalse_IfHand_Is5431() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts)
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void IsBalancedHand_ReturnsFalse_IfHand_Is6421() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts)
        };

        var result = handAnalyzer.IsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void ContainsBalancedHand_ReturnsFalse_IfHandIsEmpty() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card>();
        var result = handAnalyzer.ContainsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void ContainsBalancedHand_ReturnsTrue_IfHand_Is6332() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts)
        };

        var result = handAnalyzer.ContainsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }

    [Test]
    public void ContainsBalancedHand_ReturnsTrue_IfHand_Is5532() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts)
        };

        var result = handAnalyzer.ContainsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }

    [Test]
    public void ContainsBalancedHand_ReturnsTrue_IfHand_Is4443() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts)
        };

        var result = handAnalyzer.ContainsBalancedHand(hand);
        Assert.AreEqual(true, result);
    }

    [Test]
    public void ContainsBalancedHand_ReturnsFalse_IfHand_Is6422() {
        var handAnalyzer = new HandAnalyzer();
        var hand = new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Ace, Suit.Hearts)
        };

        var result = handAnalyzer.ContainsBalancedHand(hand);
        Assert.AreEqual(false, result);
    }
}