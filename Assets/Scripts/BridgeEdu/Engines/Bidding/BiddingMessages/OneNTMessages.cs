using static BridgeEdu.Utils.BiddingMessagesUtils;

using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Engines.Bidding.BiddingMessages {
    public static class OneNTMessages {
        // Interventions
        public static string NoIntervention => "No intervención";

        // 1NT
        // Responses
        public static string OneNTResponsePass(int HCP)
            => string.Format("{0} {1} {2} {3}, no alcanza a {4}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(0, 7), BalancedHand, GameRange);  // Pass

        public static string OneNTResponse2Clubs(int hCP)
            => string.Format("Stayman. {0} {1} {2} y {3}", ResponseTo(1, Strain.NoTrump), WithHCP(hCP), MoreThan(8), FourMajorCards()); // 2 Clubs

        public static string OneNTResponse2NT(int HCP)
            => string.Format("{0} {1} {2}, {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2 NT

        public static string OneNTResponse3NT(int HCP)
            => string.Format("{0} {1} {2} alcanza a {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(10, 15), GameRange); // 3 NT

        public static string OneNTResponse4NT(int HCP)
            => string.Format("{0} {1} {2} cuantitativa", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(16, 17)); // 4 NT

        public static string OneNTResponse6NT(int HCP)
            => string.Format("{0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), MoreThan(18), SlamRange); // 6 NT

        // Rebid
        public static string OneNTRebidNotEnoughHCP
            => string.Format("Compañero Pass {0}, no alcanza a {1}", HCPRange(0, 7), GameRange); // Pass


        public static string OneNTRebidPass(int HCP)
            => string.Format("Compañero 2NT {0} {1}, no alcanza a {2}", HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

        public static string OneNTRebidPass2(int HCP)
            => string.Format("{0} {1} {2}, no alcanza a {3}", ResponseTo(4, Strain.NoTrump), WithHCP(HCP), HCPRange(15, 16), SlamRange);  // Pass

        public static string OneNTRebid3NT(int HCP)
            => string.Format("Compañero 2NT {0} {1} {2}", HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

        public static string OneNTRebid6NT(int HCP)
            => string.Format("Compañero 4NT {0} {1} {2}", HCPRange(16, 17), WithHCP(HCP), SlamRange); // Pass

        // Transfer
        public static string OneNTTransfer2D = "Transfer, 5+ corazones";
        public static string OneNTTransfer2H = "Transfer, 5+ picas";

        public static string Transfer2DResponse2H = "Transfer, responder a 2D";
        public static string Transfer2HResponse2S = "Transfer, responder a 2H";

        public static string TransferOpenerResponsePass(int HCP)
            => string.Format("Transfer, {0} {1}", WithHCP(HCP), HCPRange(0, 7));

        public static string TransferOpenerResponse2NT(int HCP)
            => string.Format("Transfer, {0} {1} 5 cartas mayores", WithHCP(HCP), HCPRange(8, 9));

        public static string TransferOpenerResponse3NT(int HCP)
            => string.Format("Transfer, {0} {1} 5 cartas mayores", WithHCP(HCP), MoreThan(10));

        public static string TransferOpenerResponse3H(int HCP)
            => string.Format("Transfer, {0} {1}. 5 corazones", WithHCP(HCP), HCPRange(8, 9));

        public static string TransferOpenerResponse3S(int HCP)
            => string.Format("Transfer, {0} {1}. 5 picas", WithHCP(HCP), HCPRange(8, 9));

        public static string TransferOpenerResponse4H(int HCP)
            => string.Format("Transfer, {0} {1}. 6 corazones", WithHCP(HCP), MoreThan(10));

        public static string TransferOpenerResponse4S(int HCP)
            => string.Format("Transfer, {0} {1} 6 picas", WithHCP(HCP), MoreThan(10));
    }
}