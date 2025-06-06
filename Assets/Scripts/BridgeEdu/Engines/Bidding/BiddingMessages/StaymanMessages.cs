using static BridgeEdu.Utils.BiddingMessagesUtils;
using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Engines.Bidding.BiddingMessages {
    public static class StaymanMessages {
        public static string StaymanResponse2D
            => string.Format("Stayman. Compañero 2C, no tienes {0}", FourMajorCards()); // 2D

        public static string StaymanResponse2H
            => string.Format("Stayman. Compañero 2C, tienes {0}", FourMajorCards()); // 2H

        public static string StaymanResponse2S
            => string.Format("Stayman. Compañero 2C, tienes {0} picas y no {1} corazones", MoreThan(4), MoreThan(4)); // 2S

        // Stayman
        // 2D
        public static string Stayman2DResponse2NT(int HCP)
            => string.Format("Stayman. {0} {1} {2}, {3}", ResponseTo(2, Strain.Diamonds), WithHCP(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2NT

        public static string Stayman2DResponse3NT(int HCP)
            => string.Format("Stayman. {0} {1} {2} alcanza a {3}", ResponseTo(2, Strain.Diamonds), WithHCP(HCP), MoreThan(10), GameRange); // 3 NT

        // 2H
        public static string Stayman2HResponse2NT(int HCP)
            => string.Format("Stayman. {0} {1}. {2}. No tienes {3} corazones, pero sí {4} picas", ResponseTo(2, Strain.Hearts), WithHCP(HCP), HCPRange(8, 9), MoreThan(4), MoreThan(4)); // 2NT

        public static string Stayman2HResponse3H(int totalPoints)
            => string.Format("Stayman. {0} {1} {2}, {3} corazones. no alcanza a {4}", ResponseTo(2, Strain.Hearts), WithTP(totalPoints), HCPRange(8, 9), MoreThan(4), GameRange); // 3H

        public static string Stayman2HResponse3NT(int HCP)
            => string.Format("Stayman. {0} {1}. {2}. No tienes 4 corazones, pero sí {3} picas y {4}", ResponseTo(2, Strain.Hearts), WithHCP(HCP), HCPRange(10, 15), MoreThan(4), GameRange); // 3NT

        public static string Stayman2HResponse4H(int totalPoints)
            => string.Format("Stayman. {0} {1} {2}, {3} corazones y alcanza a {4}", ResponseTo(2, Strain.Hearts), WithTP(totalPoints), HCPRange(10, 15), MoreThan(4), GameRange); // 4H

        // 2S
        public static string Stayman2SResponse3S(int totalPoints)
            => string.Format("Stayman. {0} {1} {2}, {3} picas. Invitación", ResponseTo(2, Strain.Spades), WithTP(totalPoints), HCPRange(8, 9), MoreThan(4)); // 3S

        public static string Stayman2SResponse4S(int totalPoints)
            => string.Format("Stayman. {0} {1} {2}, {3} picas y alcanza a {4}", ResponseTo(2, Strain.Spades), WithTP(totalPoints), HCPRange(10, 15), MoreThan(4), GameRange); // 4S

        public static string Stayman2SResponse2NT(int HCP)
            => string.Format("Stayman. {0} {1} {2}, {3} corazones, {4}", ResponseTo(2, Strain.NoTrump), WithHCP(HCP), HCPRange(8, 9), 4, CanReachGame(8, 9, HCP)); // 2NT

        public static string Stayman2SResponse3NT(int HCP)
            => string.Format("Stayman. {0} {1} {2}, {3} corazones. {4}", ResponseTo(2, Strain.NoTrump), WithHCP(HCP), HCPRange(10, 15), 4, GameRange); // 3NT

        // Stayman second response
        public static string Stayman2D2NTSecondResponsePass(int HCP)
            => string.Format("Stayman 2D. {0} {1} {2}, no alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

        public static string Stayman2D2NTSecondResponse3NT(int HCP)
            => string.Format("Stayman 2D. {0} {1} {2}, alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // 3NT

        public static string Stayman2H3HSecondResponsePass(int TP)
            => string.Format("Stayman 2H. {0} {1} {2}, no alcanza a {3}", ResponseTo(3, Strain.Hearts), HCPRange(8, 9), WithTP(TP), GameRange); // Pass

        public static string Stayman2H3HSecondResponse4H(int TP)
            => string.Format("Stayman 2H. {0} {1} {2}, alcanza a {3}", ResponseTo(3, Strain.Hearts), HCPRange(8, 9), WithTP(TP), GameRange); // 4H

        public static string Stayman2H2NTSecondResponsePass(int HCP)
            => string.Format("Stayman 2H. {0} {1} {2}, no alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

        public static string Stayman2H2NTSecondResponse3NT(int HCP)
            => string.Format("Stayman 2H. {0} {1} {2}, alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // 3NT

        public static string Stayman2H2NTSecondResponse3S(int TP)
            => string.Format("Stayman 2H. {0} {1} {2}, fit picas, no alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithTP(TP), GameRange); // 3S

        public static string Stayman2H2NTSecondResponse4S(int TP)
            => string.Format("Stayman 2H. {0} {1} {2}, fit picas y alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithTP(TP), GameRange); // 4S

        public static string Stayman2H3NTSecondResponsePass(int HCP)
            => string.Format("Stayman 2H. {0} {1} {2}, {3}", ResponseTo(3, Strain.NoTrump), HCPRange(10, 15), WithHCP(HCP), GameRange); // Pass

        public static string Stayman2H3NTSecondResponse4S(int TP)
            => string.Format("Stayman 2H. {0} {1} {2}, fit picas y alcanza a {3}", ResponseTo(3, Strain.NoTrump), HCPRange(10, 15), WithTP(TP), GameRange); // 4S

        public static string Stayman2S3SSecondResponsePass(int TP)
            => string.Format("Stayman 2S. {0} {1} {2}, no alcanza a {3}", ResponseTo(3, Strain.Spades), HCPRange(8, 9), WithTP(TP), GameRange); // Pass

        public static string Stayman2S3SSecondResponse4S(int TP)
            => string.Format("Stayman 2S. {0} {1} {2}, fit picas, alcanza a {3}", ResponseTo(3, Strain.Spades), HCPRange(8, 9), WithTP(TP), GameRange); // 4S

        public static string Stayman2S2NTSecondResponsePass(int HCP)
            => string.Format("Stayman 2S. {0} {1} {2}, no alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

        public static string Stayman2S2NTSecondResponse3NT(int HCP)
            => string.Format("Stayman 2S. {0} {1} {2}, alcanza a {3}", ResponseTo(2, Strain.NoTrump), HCPRange(8, 9), WithHCP(HCP), GameRange); // 3NT
    }
}