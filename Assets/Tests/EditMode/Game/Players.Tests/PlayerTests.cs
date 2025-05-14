using System.Collections.Generic;
using NUnit.Framework;

public class PlayerTests
{
    [Test]
    public void Player_Constructor_SetsPosition() {
        Position position = Position.North;
        var player = new Player(position);
        Assert.AreEqual(position, player.Position);
    }

    [Test]
    public void Player_Hand_StartsEmpty() {
        var player = new Player(Position.North);
        Assert.AreEqual(0, player.Hand.Count);
    }

    [Test]
    public void ReceiveCards_AddsCardsToHand() {
        var player = new Player(Position.North);
        var cards = new List<Card> {
            new(Card.CardRank.Ace, Suit.Clubs)
        };
        player.ReceiveCards(cards);
        Assert.AreEqual(1, player.Hand.Count);
    }

    [Test]
    public void PlayCard_RemovesCardFromHand() {
        var player = new Player(Position.North);
        var card = new Card(Card.CardRank.Ace, Suit.Clubs);
        player.ReceiveCards(new List<Card> { card });
        player.PlayCard(card);
        Assert.AreEqual(0, player.Hand.Count);
    }

    [Test]
    public void PlayCard_DoesNothing_IfCardNotInHand() {
        var player = new Player(Position.North);
        var card = new Card(Card.CardRank.Ace, Suit.Clubs);
        var card2 = new Card(Card.CardRank.Ace, Suit.Spades);
        player.ReceiveCards(new List<Card> { card2 });
        player.PlayCard(card);
        Assert.AreEqual(1, player.Hand.Count);
    }
}
