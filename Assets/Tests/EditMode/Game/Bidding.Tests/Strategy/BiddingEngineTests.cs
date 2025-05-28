using NUnit.Framework;
using UnityEngine;

public class OpeningStrategyTests{
    [Test]
    public void GetSuggestions_OpeningBid_Returns1NT_For15To17HCP_Balanced() {
        var biddingContext = new BiddingContextBuilder()
            .WithHand(HandGenerator.Generate(new Deck(), true, 15, 17))
            .Build();
        
        var strategy = new OpeningBidStrategy();
        var suggestions = strategy.GetSuggestions(biddingContext);
        
        Assert.AreEqual(1, suggestions.Count);

        var suggestion = suggestions[0];
        var call = suggestion.Call;
        
        Debug.Log(suggestion.Message);

        Assert.AreEqual(CallType.Bid, call.Type);

        var bidCall = call as BidCall;
        var bid = bidCall.Bid;

        Assert.AreEqual(new Bid(1, Strain.NoTrump), bid);
    }
}