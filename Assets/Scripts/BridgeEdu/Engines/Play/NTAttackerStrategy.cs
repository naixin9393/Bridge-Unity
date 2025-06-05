namespace BridgeEdu.Engines.Play {
    using System.Collections.Generic;
    using BridgeEdu.Core;
    using BridgeEdu.Game.Bidding;
    using BridgeEdu.Game.Play;
    using BridgeEdu.Utils;
    public class NTAttackerStrategy : IPlayingStrategy {
        public bool IsApplicable(IPlayingContext playingContext) {
            // Check if the context is suitable for NT Attacker strategy
            if (!playingContext.IsAttackerTurn) return false;
            // Only apply if there are multiple possible cards to play
            if (playingContext.PossibleCards.Count <= 1) return false;
            if (playingContext.Contract.Strain != Strain.NoTrump) return false;
            return true;
        }

        public List<PlayingSuggestion> GetSuggestions(IPlayingContext context) {
            // Generate suggestions based on the NT Attacker strategy
            var suggestions = new List<PlayingSuggestion>();
            if (context.PossibleCards.Count == 0) {
                return new List<PlayingSuggestion>();
            }

            // All possible cards are same suit and consecutive play lowest
            var cards = context.PossibleCards;

            if (HandUtils.AllSameSuit(cards) && HandUtils.AreConsecutive(cards)) {
                // Sort cards by rank
                cards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(
                    new PlayingSuggestion(
                        message: PlayingMessagesUtils.PlayLowestCardConsecutive(cards),
                        card: cards[0]
                    )
                );
            }

            if (context.CurrentTrick == null || context.CurrentTrick.Plays == null)
                return suggestions;

            switch (context.CurrentTrick.Plays.Count) {
                case 0:
                    suggestions.AddRange(AttackerFirstTurn(context));
                    break;
                case 1:
                    suggestions.AddRange(AttackerSecondTurn(context));
                    break;
                case 2:
                    suggestions.AddRange(AttackerThirdTurn(context));
                    break;
                case 3:
                    suggestions.AddRange(AttackerFourthTurn(context));
                    break;
                default:
                    // No specific strategy for more than 4 plays
                    break;
            }
            return suggestions;
        }

        private List<PlayingSuggestion> AttackerFirstTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();

            // Heuristic: Play card of longest suit
            // If partner has the highest card, play the lowest card
            var partner = PlayerUtils.PartnerOf(context.CurrentPlayer, context.Players);

            var longestSuit = context.DeclarerLongSuit;
            var highestCard = HandUtils.GetHighestCardOfSuit(context.Hand, longestSuit);
            var highestPartnerCard = HandUtils.GetHighestCardOfSuit(partner.Hand, longestSuit);

            if (highestCard != null) {
                if (highestPartnerCard == null) {
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerFirstTurnPlayLongSuitHighestCard,
                        card: highestCard
                    ));
                }
                else if (highestCard.Rank > highestPartnerCard.Rank) {
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerFirstTurnPlayLongSuitHighestCard,
                        card: highestCard
                    ));
                }
                else {
                    // If partner has the highest card, play the lowest card in the suit
                    var lowestCard = HandUtils.GetLowestCardOfSuit(context.Hand, longestSuit);
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerFirstTurnPlayLongSuitLowestCard,
                        card: lowestCard
                    ));
                }
            }
            else {
                // If partner has the highest card, play the lowest card in the suit
                var possibleCards = context.PossibleCards;
                possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerFirstTurnPlayLongSuitLowestCard,
                    card: possibleCards[0]
                ));
            }

            return suggestions;
        }

        private List<PlayingSuggestion> AttackerSecondTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();

            // Heuristic: If contains bigger card, play it
            var leadCard = context.CurrentTrick.Plays[0].Card;
            var possibleCards = context.PossibleCards;

            if (HandUtils.ContainsBiggerCard(possibleCards, leadCard)) {
                var biggerCard = HandUtils.GetBiggerCard(possibleCards, leadCard);
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerPlayBiggerCard,
                    card: biggerCard
                ));
            }
            else {
                // play the lowest card in the suit
                possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerSecondTurnPlayLowestCard,
                    card: possibleCards[0]
                ));
            }

            return suggestions;
        }

        private List<PlayingSuggestion> AttackerThirdTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();
            // Heuristic: Play card of longest suit
            // If partner has the highest card, play the lowest card
            var partner = PlayerUtils.PartnerOf(context.CurrentPlayer, context.Players);

            var longestSuit = context.DeclarerLongSuit;
            var highestCardInHand = HandUtils.GetHighestCardOfSuit(context.Hand, longestSuit);

            // If partner card is highest, play lowest
            var sortedCards = new List<Card>() {
                context.CurrentTrick.Plays[0].Card,
                context.CurrentTrick.Plays[1].Card,
            };

            var comparer = new BridgeCardComparer(leadSuit: context.CurrentTrick.LeadSuit.Value, contractStrain: context.Contract.Strain);

            sortedCards.Sort((a, b) => comparer.Compare(a, b));
            var highestCardInTrick = sortedCards[1];
            var partnerCard = context.CurrentTrick.Plays[0].Card;

            if (highestCardInHand != null &&
                comparer.Compare(highestCardInHand, partnerCard) == 1 &&
                context.PossibleCards.Contains(highestCardInHand)) {
                // If we have the highest card in the suit, play it
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerThirdTurnPlayLongSuitHighestCard,
                    card: highestCardInHand
                ));
            }
            // If partner card is highest, play lowest
            if (highestCardInTrick.Equals(partnerCard)) {
                var possibleCards = context.PossibleCards;
                possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerFourthTurnPlayLowestCardPartnerHighest,
                    card: possibleCards[0]
                ));
            }
            else {
                // If partner card is not highest, play highest if can win
                if (HandUtils.ContainsBiggerCard(context.PossibleCards, highestCardInTrick)) {
                    var biggerCard = HandUtils.GetBiggerCard(context.PossibleCards, highestCardInTrick);
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerThirdTurnPlayHigherCard,
                        card: biggerCard
                    ));
                }
                else {
                    // If cannot win, play lowest
                    var possibleCards = context.PossibleCards;
                    possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerThirdTurnPlayLowestCardCantWin,
                        card: possibleCards[0]
                    ));
                }
            }
            return suggestions;
        }

        private List<PlayingSuggestion> AttackerFourthTurn(IPlayingContext context) {
            List<PlayingSuggestion> suggestions = new();

            // If partner card is highest, play lowest
            var sortedCards = new List<Card>() {
                context.CurrentTrick.Plays[0].Card,
                context.CurrentTrick.Plays[1].Card,
                context.CurrentTrick.Plays[2].Card,
            };

            var comparer = new BridgeCardComparer(leadSuit: context.CurrentTrick.LeadSuit.Value, contractStrain: context.Contract.Strain);

            sortedCards.Sort((a, b) => comparer.Compare(a, b));
            var highestCard = sortedCards[2];
            var partnerCard = context.CurrentTrick.Plays[1].Card;

            // If partner card is highest, play lowest
            if (highestCard.Equals(partnerCard)) {
                var possibleCards = context.PossibleCards;
                possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                suggestions.Add(new PlayingSuggestion(
                    message: PlayingMessagesUtils.AttackerFourthTurnPlayLowestCardPartnerHighest,
                    card: possibleCards[0]
                ));
            }
            else {
                // If partner card is not highest, play highest if can win
                if (HandUtils.ContainsBiggerCard(context.PossibleCards, highestCard)) {
                    var biggerCard = HandUtils.GetBiggerCard(context.PossibleCards, highestCard);
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerFourthTurnPlayHigherCard,
                        card: biggerCard
                    ));
                }
                else {
                    // If cannot win, play lowest
                    var possibleCards = context.PossibleCards;
                    possibleCards.Sort((a, b) => a.Rank.CompareTo(b.Rank));
                    suggestions.Add(new PlayingSuggestion(
                        message: PlayingMessagesUtils.AttackerFourthTurnPlayLowestCardCantWin,
                        card: possibleCards[0]
                    ));
                }
            }
            return suggestions;
        }
    }
}