using BridgeEdu.Game.Bidding;

namespace BridgeEdu.Utils {
    public static class BiddingMessagesUtils {
        public static readonly string Unknown = "Subasta desconocida";
        public static readonly string AuctionConcludedPass = "Se ha llegado a un acuerdo.";
        public static readonly string BalancedHand = string.Format("mano equilibrada");
        public static readonly string GameRange = string.Format("manga (25-32)");
        public static readonly string SlamRange = string.Format("slam (33-36)");
        public static string HCPRange(int minHCP, int maxHCP) => string.Format("({0}-{1})", minHCP, maxHCP);
        public static string WithHCP(int HCP) => string.Format("con {0} HCP", HCP);
        public static string WithTP(int totalPoints) => string.Format("con {0} TP", totalPoints);
        public static string ResponseTo(int level, Strain strain) => string.Format("Respuesta a {0}{1}", level, strain.ToSymbol());
        public static string MoreThan(int HCP) => string.Format("({0}+)", HCP);
        public static object FourMajorCards() => string.Format("4+ cartas mayores (picas/corazones)");

        public static string CanReachGame(int minHCP, int maxHCP, int currentHCP) {
            const int HCPNeeded = 25;
            int allyMaxHCP = HCPNeeded - minHCP;
            string allyHCPRange;
            if (currentHCP != minHCP)
                allyHCPRange = string.Format("{0}-{1}", HCPNeeded - currentHCP, allyMaxHCP);
            else
                allyHCPRange = allyMaxHCP.ToString();
            return string.Format("puede llegar a {0} si el compañero tiene {1} HCP, invitativa a manga", GameRange, allyHCPRange);
        }
    }
}