/*
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BiddingEngineTests {
    [Test]
    public void GetBiddingSuggestion_OpeningBid_Returns1NT_For15To17HCP_Balanced() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), true, 15, 17))
            .Build();

        var biddingEngine = new BiddingEngine();
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(1, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo1NT_With0To7HCP_ReturnsPass() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), false, 0, 7))
            .WithCalls(new List<ICall> {
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Pass, call.Type);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo1NT_With8To9HCP_NoFourMajor_Returns2NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8_9HCP_NoFourMajor())
            .WithCalls(new List<ICall> {
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo1NT_With8To9HCP_NoFourMajor_Returns2NT_Test2() {
        var biddingEngine = new BiddingEngine();
        biddingEngine.UpdateState(new BidCall(new Bid(1, Strain.NoTrump), null));
        biddingEngine.UpdateState(new Pass(null));
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8_9HCP_NoFourMajor())
            .WithCalls(new List<ICall> {
                new Pass(null)}) // No intervention
            .Build();

        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;
        
        Assert.AreEqual(CallType.Bid, call.Type);
        Debug.Log(suggestion.Message);

        var bidCall2 = call as BidCall;
        var bid2 = bidCall2.Bid;

        Assert.AreEqual(new Bid(2, Strain.NoTrump), bid2);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo1NT_WithMoreThan10HCP_NoFourMajor_Returns3NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith10PlusHCP_NoFourMajor())
            .WithCalls(new List<ICall> {
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo1NT_With8PlusHCP_FourMajor_Returns2Clubs() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8PlusHCP_FourMajor())
            .WithCalls(new List<ICall> {
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Clubs), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondTo2NTInvitation_With17HCP_Returns3NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), true, 17, 17))
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.NoTrump), null), // Partner 2NT
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new YouActAfterPartner1NTResponseState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman_WithNoFourMajor_Returns2Diamonds() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith15_17HCP_NoFourMajor())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Clubs), null), // Partner 2C (stayman)
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new YouActAfterPartner1NTResponseState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Diamonds), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman_With_FourHearts_Returns2Hearts() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8PlusHCP_FourHearts())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Clubs), null), // Partner 2C (stayman)
                new Pass(null)}) // No intervention
            .Build();
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new YouActAfterPartner1NTResponseState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Hearts), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman_With_FourSpadesAndNoFourHearts_Returns2Spades() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8PlusHCP_FourSpades())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Clubs), null), // Partner 2C (stayman)
                new Pass(null)}) // No intervention
            .Build();
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new YouActAfterPartner1NTResponseState());
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.Spades), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Diamonds_With_8_9HCP_Returns2NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), false, 8, 9))
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Diamonds), null), // Partner 2D
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());
        
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Diamonds_With_10PlusHCP_Returns3NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), false, 10, 15))
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Diamonds), null), // Partner 2D
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());
        
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;        

        Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Hearts_With_4HeartsAnd8_9TP_Returns3Hearts() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8_9TP_FourHearts())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Hearts), null), // Partner 2H
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());
        
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;        

        Assert.AreEqual(new Bid(3, Strain.Hearts), bid);
    }


    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Hearts_With_4HeartsAnd10_15TP_Returns4Hearts() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith10_15TP_FourHearts())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Hearts), null), // Partner 2H
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());

        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;
        
        Assert.AreEqual(new Bid(4, Strain.Hearts), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Hearts_With_4SpadesAnd8_9HCP_Returns2NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8_9HCP_FourSpades())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Hearts), null), // Partner 2H
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());

        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(2, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Hearts_With_4SpadesAnd10_15HCP_Returns3NT() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith10_15HCP_FourSpades())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Hearts), null), // Partner 2H
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());

        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(3, Strain.NoTrump), bid);
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Spades_With_4SpadesAnd8_9TP_Returns3S() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith8_9TP_FourSpades())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Spades), null), // Partner 2S
                new Pass(null)}) // No intervention
            .Build();

        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());

        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;
        
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(3, Strain.Spades), bid);
    }

    private List<Card> HandWith8_9TP_FourSpades() {
        // 8 HCP
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Nine, Suit.Hearts),
            new(Rank.Eight, Suit.Hearts),
            new(Rank.Seven, Suit.Hearts),
            new(Rank.Six, Suit.Diamonds),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Diamonds),
            new(Rank.Three, Suit.Clubs),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
        };
    }

    [Test]
    public void GetBiddingSuggestion_RespondToStayman2Spades_With_4SpadesAnd10_15TP_Returns4S() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandWith10_15TP_FourSpades())
            .WithCalls(new List<ICall> {
                new BidCall(new Bid(2, Strain.Spades), null), // Partner 2S
                new Pass(null)}) // No intervention
            .Build();
        
        var biddingEngine = new BiddingEngine();
        biddingEngine.SetState(new PartnerRespondTo1NTRebidState());
        
        var suggestion = biddingEngine.GetBiddingSuggestion(biddingContext);
        var call = suggestion.Call;

        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(4, Strain.Spades), bid);
    }

    private List<Card> HandWith10_15TP_FourSpades() {
        // 8 HCP + 2 points (1 Singleton) = 10 TP
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Eight, Suit.Hearts),
            new(Rank.Nine, Suit.Hearts),
            new(Rank.Ten, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Four, Suit.Diamonds),
            new(Rank.Ten, Suit.Diamonds),
            new(Rank.Three, Suit.Diamonds),
            new(Rank.Two, Suit.Clubs),
        };
    }

    private List<Card> HandWith10_15HCP_FourSpades() {
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
            new(Rank.Ace, Suit.Hearts),
        };
    }

    private List<Card> HandWith8_9HCP_FourSpades() {
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
        };
    }

    private List<Card> HandWith10_15TP_FourHearts() {
        // 8 HCP + 1 DP (9Hearts cards) + 2 points (1 Singleton) = 11 TP
        return new List<Card> {
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Five, Suit.Hearts),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Ten, Suit.Spades),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Clubs),
        };
    }

    private List<Card> HandWith8_9TP_FourHearts() {
        return new List<Card> {
            new(Rank.Ace, Suit.Hearts),
            new(Rank.King, Suit.Hearts),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Ten, Suit.Hearts),
            new(Rank.Nine, Suit.Spades),
            new(Rank.Eight, Suit.Spades),
            new(Rank.Seven, Suit.Spades),
            new(Rank.Six, Suit.Clubs),
            new(Rank.Five, Suit.Clubs),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Three, Suit.Diamonds),
            new(Rank.Two, Suit.Diamonds),
            new(Rank.Ten, Suit.Diamonds),
        };
    }

    private List<Card> HandWith8PlusHCP_FourSpades() {
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds)
        };
    }

    private List<Card> HandWith8PlusHCP_FourHearts() {
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds)
        };
    }

    private List<Card> HandWith15_17HCP_NoFourMajor() {
        return new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Queen, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };
    }

    private List<Card> HandWith8PlusHCP_FourMajor() {
        return new List<Card> {
            new(Rank.Ace, Suit.Spades),
            new(Rank.King, Suit.Spades),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Jack, Suit.Spades),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds)
        };
    }

    private List<Card> HandWith10PlusHCP_NoFourMajor() {
        return new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Jack, Suit.Clubs),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Seven, Suit.Diamonds),
            new(Rank.Six, Suit.Spades),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Two, Suit.Hearts)
        };
    }

    private List<Card> HandWith8_9HCP_NoFourMajor() {
        return new List<Card> {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.King, Suit.Clubs),
            new(Rank.Queen, Suit.Clubs),
            new(Rank.Two, Suit.Clubs),
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
    }
}
*/