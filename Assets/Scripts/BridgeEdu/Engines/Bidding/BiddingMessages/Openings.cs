using static BridgeEdu.Utils.BiddingMessagesUtils;

namespace BridgeEdu.Engines.Bidding.BiddingMessages {
    public static class OpeningMessages {
        public static string OpeningPass(int HCP)
            => string.Format("Apertura {0} {1}, no tienes suficientes HCP para abrir", WithHCP(HCP), HCPRange(0, 11));
        public static string OpeningOneNT(int HCP)
            => string.Format("Apertura {0} {1} con {2}", WithHCP(HCP), HCPRange(15, 17), BalancedHand);
    }
}