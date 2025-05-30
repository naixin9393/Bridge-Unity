using System.Collections.Generic;
using System.Linq;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

using static BridgeEdu.Engines.Bidding.BiddingMessages.OneNTMessages;

namespace BridgeEdu.Engines.Bidding.OneNT {
    public class TransferStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext context) {
            if (context.Calls.Count < 2) return false;
            if (context.Calls.Where(c => c.Type == CallType.Bid).Count() != 1) return false;
            var bidCall = context.Calls.First(c => c.Type == CallType.Bid) as BidCall;
            if (!bidCall.Bid.Equals(new Bid(1, Strain.NoTrump))) return false;
            if (context.Hand.NumberOfCardsOfSuit(Suit.Hearts) >= 5) return true;
            if (context.Hand.NumberOfCardsOfSuit(Suit.Spades) >= 5) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new();
            var hand = context.Hand;

            // 2D when 5+ Hearts
            if (hand.NumberOfCardsOfSuit(Suit.Hearts) >= 5)
                suggestions.Add(new BiddingSuggestion(
                    OneNTTransfer2D,
                    new BidCall(new Bid(2, Strain.Diamonds), null)));

            // 2H when 5+ Spades
            if (hand.NumberOfCardsOfSuit(Suit.Spades) >= 5)
                suggestions.Add(new BiddingSuggestion(
                    OneNTTransfer2H,
                    new BidCall(new Bid(2, Strain.Hearts), null)));


            return suggestions;
        }
    }
}