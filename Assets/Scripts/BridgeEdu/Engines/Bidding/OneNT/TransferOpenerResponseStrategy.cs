using System.Collections.Generic;
using System.Linq;
using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;

using static BridgeEdu.Engines.Bidding.BiddingMessages.OneNTMessages;

namespace BridgeEdu.Engines.Bidding.OneNT {
    public class TransferOpenerResponseStrategy : IBiddingStrategy {
        public bool IsApplicable(BiddingContext biddingContext) {
            if (biddingContext.Calls.Count < 3) return false;
            if (biddingContext.Calls.Where(c => c.Type == CallType.Bid).Count() != 3) return false;
            if (biddingContext.MatchesBidSequence(Transfer2HSequence)) return true;
            if (biddingContext.MatchesBidSequence(Transfer2SSequence)) return true;
            return false;
        }

        public List<BiddingSuggestion> GetSuggestions(BiddingContext context) {
            List<BiddingSuggestion> suggestions = new();

            bool hasFiveHearts = context.Hand.NumberOfCardsOfSuit(Suit.Hearts) == 5;
            bool hasFiveSpades = context.Hand.NumberOfCardsOfSuit(Suit.Spades) == 5;
            bool hasSixHearts = context.Hand.NumberOfCardsOfSuit(Suit.Hearts) == 6;
            bool hasSixSpades = context.Hand.NumberOfCardsOfSuit(Suit.Spades) == 6;

            var hcp = context.Hand.HCP;
            if (hcp <= 7)
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponsePass(hcp), new Pass(null)));

            if (hcp >= 8 && hcp <= 9 && (hasFiveHearts || hasFiveSpades))
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse2NT(hcp), new BidCall(new Bid(2, Strain.NoTrump), null)));

            if (hcp >= 10 && (hasFiveHearts || hasFiveSpades))
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse3NT(hcp), new BidCall(new Bid(3, Strain.NoTrump), null)));

            if (hcp >= 8 && hcp <= 9 && hasSixHearts)
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse3H(hcp), new BidCall(new Bid(3, Strain.Hearts), null)));

            if (hcp >= 8 && hcp <= 9 && hasSixSpades)
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse3S(hcp), new BidCall(new Bid(3, Strain.Spades), null)));

            if (hcp >= 10 && hasSixHearts)
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse4H(hcp), new BidCall(new Bid(4, Strain.Hearts), null)));

            if (hcp >= 10 && hasSixSpades)
                suggestions.Add(new BiddingSuggestion(TransferOpenerResponse4S(hcp), new BidCall(new Bid(4, Strain.Spades), null)));

            return suggestions;
        }


        private (int level, Strain strain)[] Transfer2SSequence = new (int, Strain)[] {
            (1, Strain.NoTrump),
            (2, Strain.Hearts),
            (2, Strain.Spades)
        };

        private (int level, Strain strain)[] Transfer2HSequence = new (int, Strain)[] {
            (1, Strain.NoTrump),
            (2, Strain.Diamonds),
            (2, Strain.Hearts)
        };
    }
}