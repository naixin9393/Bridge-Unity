namespace BridgeEdu.Utils {
    using BridgeEdu.Core;

    public static class CardUtils {
        public static bool IsHonor(Card card) {
            return card != null &&
                   (card.Rank == Rank.Ace ||
                    card.Rank == Rank.King ||
                    card.Rank == Rank.Queen ||
                    card.Rank == Rank.Jack);
        }
    }
}