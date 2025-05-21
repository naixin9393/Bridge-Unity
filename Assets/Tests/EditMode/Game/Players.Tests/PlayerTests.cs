using System.Collections.Generic;
using NUnit.Framework;

public class PlayerTests
{
    [Test]
    public void Player_Constructor_SetsPosition() {
        Position position = Position.North;
        IPlayer player = new ComputerPlayer(position);
        Assert.AreEqual(position, player.Position);
    }

    [Test]
    public void Player_Hand_StartsEmpty() {
        IPlayer player = new ComputerPlayer(Position.North);
        Assert.AreEqual(0, player.Hand.Count);
    }

    [Test]
    public void ReceiveCards_AddsCardsToHand() {
        IPlayer player = new ComputerPlayer(Position.North);
        var cards = new List<Card> {
            new(Rank.Ace, Suit.Clubs)
        };
        player.ReceiveCards(cards);
        Assert.AreEqual(1, player.Hand.Count);
    }

    [Test]
    public void PlayCard_RemovesCardFromHand() {
        var player = new ComputerPlayer(Position.North);
        var card = new Card(Rank.Ace, Suit.Clubs);
        player.ReceiveCards(new List<Card> { card });
        player.PlayCard(card);
        Assert.AreEqual(0, player.Hand.Count);
    }

    [Test]
    public void PlayCard_ThrowsException_IfCardIsNotInHand() {
        var player = new ComputerPlayer(Position.North);
        var card = new Card(Rank.Ace, Suit.Clubs);
        var card2 = new Card(Rank.Ace, Suit.Spades);
        player.ReceiveCards(new List<Card> { card2 });
        Assert.Catch<CardNotInHandException>(() => player.PlayCard(card));
    }
}
