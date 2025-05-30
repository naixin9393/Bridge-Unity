using System.Collections.Generic;
using System.Linq;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using static BridgeEdu.Engines.Bidding.BiddingMessages.OneNTMessages;
using static BridgeEdu.Utils.BiddingMessagesUtils;

namespace BridgeEdu.Engines.Bidding.OneNT {
    public class OneNTOpenerResponseStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext context) {
            if (context.MatchesBidSequence((1, Strain.NoTrump), (2, Strain.NoTrump))) return true;
            if (context.MatchesBidSequence((1, Strain.NoTrump), (3, Strain.NoTrump))) return true;
            if (context.MatchesBidSequence((1, Strain.NoTrump), (4, Strain.NoTrump))) return true;
            if (context.MatchesBidSequence((1, Strain.NoTrump), (6, Strain.NoTrump))) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            // Responses to 2NT, 4NT and 6NT
            var partnerBidCall = context.Calls.Where(c => c.Type == CallType.Bid).Last() as BidCall;
            var partnerBid = partnerBidCall.Bid;
            List<BiddingSuggestion> suggestions = new();

            // Response to 2NT
            if (context.Hand.HCP >= 15 && context.Hand.HCP <= 16 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(2, Strain.NoTrump)))
                // Pass when 15-16 HCP and balanced hand
                suggestions.Add(
                        new(message: OneNTRebidPass(context.Hand.HCP),
                        call: new Pass(null)));

            if (context.Hand.HCP == 17 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(2, Strain.NoTrump)))
                // 3NT when 17 HCP and balanced hand
                suggestions.Add(
                        new(message: OneNTRebid3NT(context.Hand.HCP),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)));

            // Response to 3NT
            if (partnerBid.Equals(new Bid(3, Strain.NoTrump)))
                suggestions.Add(
                        new(message: AuctionConcludedPass,
                        call: new Pass(null)));

            // Response to 4NT
            if (context.Hand.HCP >= 15 && context.Hand.HCP <= 16 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(4, Strain.NoTrump)))
                // Pass auction concluded
                suggestions.Add(
                        new(message: OneNTRebidPass2(context.Hand.HCP),
                        call: new Pass(null)));

            if (context.Hand.HCP == 17 && context.Hand.IsBalanced && partnerBid.Equals(new Bid(4, Strain.NoTrump)))
                // 6NT when 17 HCP and balanced hand
                suggestions.Add(
                        new(message: OneNTRebid6NT(context.Hand.HCP),
                        call: new BidCall(new Bid(6, Strain.NoTrump), null)));

            // Response to 6NT
            if (context.Hand.IsBalanced && partnerBid.Equals(new Bid(6, Strain.NoTrump)))
                // Pass auction concluded
                suggestions.Add(
                        new(message: AuctionConcludedPass,
                        call: new Pass(null)));

            return suggestions;
        }
    }
}