/*
using System;

public class PartnerRespondTo1NTRebidState : IBiddingState {
    public BiddingSuggestion CalculateCall(BiddingContext biddingContext) {
        var calls = biddingContext.Calls;
        if (calls.Count == 0)
            throw new Exception("No calls in bidding context");
        if (calls[^1].Type != CallType.Pass)
            // Opponent intervened, just pass
            return new BiddingSuggestion(
                message: BiddingMessages.Unknown,
                call: new Pass(null)
            );
        
        var partnerCall = calls[^2];
        var hand = biddingContext.Hand;
        int HCP = HandUtils.CalculateHighCardPoints(hand);

        // Partner responds with bid
        if (partnerCall.Type == CallType.Bid) {
            var bidCall = partnerCall as BidCall;
            var bid = bidCall.Bid;

            // Partner bid 2D (no fit)
            if (bid.Equals(new Bid(2, Strain.Diamonds))) {
                // Respond with 2NT if HCP 8-9
                if (HCP <= 9)
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2DResponse2NT(HCP),
                        call: new BidCall(new Bid(2, Strain.NoTrump), null)
                    );
                // Respond with 3NT if HCP 10-15
                return new BiddingSuggestion(
                    message: BiddingMessages.OneNTStayman2DResponse3NT(HCP),
                    call: new BidCall(new Bid(3, Strain.NoTrump), null)
                );
            }

            // Partner bid 2H
            if (bid.Equals(new Bid(2, Strain.Hearts))) {
                // Respond with 2NT if hand does not contain 4 hearts and 8-9 HCP
                if (!HandUtils.Contains4Hearts(hand) && HCP >= 8 && HCP <= 9) {
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2HResponse2NT(HCP),
                        call: new BidCall(new Bid(2, Strain.NoTrump), null)
                    );
                }
                
                // Respond with 3NT if hand does not contain 4 hearts and 10-15 HCP
                if (!HandUtils.Contains4Hearts(hand) && HCP >= 10 && HCP <= 15) {
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2HResponse3NT(HCP),
                        call: new BidCall(new Bid(3, Strain.NoTrump), null)
                    );
                }

                int totalPoints = HandUtils.CalculateTotalPoints(hand, Suit.Hearts);
                // Respond with 3H if TP 8-9
                if (totalPoints > 7 && totalPoints < 10)
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2HResponse3H(totalPoints),
                        call: new BidCall(new Bid(3, Strain.Hearts), null)
                    );
                // Respond with 3S if TP 10-15
                if (totalPoints >= 10 && totalPoints <= 15)
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2HResponse4H(totalPoints),
                        call: new BidCall(new Bid(4, Strain.Hearts), null)
                    );
            }

            // Partner bid 2S
            if (bid.Equals(new Bid(2, Strain.Spades))) {
                int totalPoints = HandUtils.CalculateTotalPoints(hand, Suit.Spades);
                // Respond with 3S if hand contains 4 spades and 8-9 HCPD
                if (HandUtils.Contains4Spades(hand) && totalPoints >= 8 && totalPoints <= 9)
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2SResponse3S(totalPoints),
                        call: new BidCall(new Bid(3, Strain.Spades), null)
                    );
                // Respond with 4S if hand contains 4 spades and 10+ HCPD
                if (HandUtils.Contains4Spades(hand) && totalPoints >= 10)
                    return new BiddingSuggestion(
                        message: BiddingMessages.OneNTStayman2SResponse4S(totalPoints),
                        call: new BidCall(new Bid(4, Strain.Spades), null)
                    );
            }
        }

        return new BiddingSuggestion(
            message: BiddingMessages.Unknown,
            call: new Pass(null)
        );
    }

    public IBiddingState GetNextState(ICall call) {
        return new UnknownState();
    }
}
*/