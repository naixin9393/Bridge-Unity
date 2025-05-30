using System.Collections.Generic;
using System.Linq;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Utils;
using static BridgeEdu.Engines.Bidding.BiddingMessages.StaymanMessages;

namespace BridgeEdu.Engines.Bidding.OneNT {
    public class StaymanOpenerResponseStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext biddingContext) {
            if (biddingContext.Calls.Count < 6) return false;
            var partnerCall = biddingContext.Calls[^2];
            if (partnerCall.Type != CallType.Bid) return false;
            if (biddingContext.MatchesBidSequence((1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Diamonds))) return true;
            if (biddingContext.MatchesBidSequence((1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Hearts))) return true;
            if (biddingContext.MatchesBidSequence((1, Strain.NoTrump), (2, Strain.Clubs), (2, Strain.Spades))) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new();
            int hcp = context.Hand.HCP;
            int totalPointsHearts = HandUtils.CalculateTotalPoints(context.Hand, Suit.Hearts);
            int totalPointsSpades = HandUtils.CalculateTotalPoints(context.Hand, Suit.Spades);
            var hand = context.Hand;
            var partnerBid = context.Calls.Where(c => c.Type == CallType.Bid).Last() as BidCall;
            bool partnerBidIs2Diamonds = partnerBid.Bid.Equals(new Bid(2, Strain.Diamonds));
            bool partnerBidIs2Hearts = partnerBid.Bid.Equals(new Bid(2, Strain.Hearts));
            bool partnerBidIs2Spades = partnerBid.Bid.Equals(new Bid(2, Strain.Spades));

            // Response to 2D
            if (hcp >= 8 && hcp <= 9 && partnerBidIs2Diamonds)
                // 2NT when 8-9 HCP
                suggestions.Add(
                        new(message: Stayman2DResponse2NT(hcp),
                        call: new BidCall(new Bid(2, Strain.NoTrump), null)));

            if (hcp >= 10 && hcp <= 15 && partnerBidIs2Diamonds)
                // 3NT when 10-15 HCP
                suggestions.Add(
                        new(message: Stayman2DResponse3NT(hcp),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)));

            // Response to 2H
            if (totalPointsHearts >= 8 && totalPointsHearts <= 9 && HandUtils.Contains4Hearts(hand) && partnerBidIs2Hearts)
                // 3H when 8-9 TP and 4 hearts
                suggestions.Add(
                        new(message: Stayman2HResponse3H(totalPointsHearts),
                        call: new BidCall(new Bid(3, Strain.Hearts), null)));

            if (totalPointsHearts >= 10 && totalPointsHearts <= 15 && HandUtils.Contains4Hearts(hand) && partnerBidIs2Hearts)
                // 4H when 10-15 TP and 4 hearts
                suggestions.Add(
                        new(message: Stayman2HResponse4H(totalPointsHearts),
                        call: new BidCall(new Bid(4, Strain.Hearts), null)));

            if (hcp >= 8 && hcp <= 9 && HandUtils.Contains4Spades(hand) && !HandUtils.Contains4Hearts(hand) && partnerBidIs2Hearts)
                // 2NT when 8-9 HCP and 4 spades no 4 hearts
                suggestions.Add(
                        new(message: Stayman2HResponse2NT(hcp),
                        call: new BidCall(new Bid(2, Strain.NoTrump), null)));

            if (hcp >= 10 && hcp <= 15 && partnerBidIs2Hearts)
                // 3NT when 10-15 HCP
                suggestions.Add(
                        new(message: Stayman2HResponse3NT(hcp),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)));

            // Response to 2S
            if (totalPointsSpades >= 8 && totalPointsSpades <= 9 && HandUtils.Contains4Spades(hand) && partnerBidIs2Spades)
                // 3S when 8-9 TP and 4 spades
                suggestions.Add(
                        new(message: Stayman2SResponse3S(totalPointsSpades),
                        call: new BidCall(new Bid(3, Strain.Spades), null)));

            if (totalPointsSpades >= 10 && HandUtils.Contains4Spades(hand) && partnerBidIs2Spades)
                // 4S when 10+ and 4 spades
                suggestions.Add(
                        new(message: Stayman2SResponse4S(totalPointsSpades),
                        call: new BidCall(new Bid(4, Strain.Spades), null)));

            if (hcp >= 8 && hcp <= 9 && HandUtils.Contains4Hearts(hand) && !HandUtils.Contains4Spades(hand) && partnerBidIs2Spades)
                // 2NT when 8-9 HCP and 4 hearts no 4 spades
                suggestions.Add(
                        new(message: Stayman2SResponse2NT(hcp),
                        call: new BidCall(new Bid(2, Strain.NoTrump), null)));

            if (hcp >= 10 && hcp <= 15 && HandUtils.Contains4Hearts(hand) && !HandUtils.Contains4Spades(hand) && partnerBidIs2Spades)
                // 3NT when 10-15 HCP and 4 hearts no 4 spades
                suggestions.Add(
                        new(message: Stayman2SResponse3NT(hcp),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)));

            return suggestions;
        }
    }
}