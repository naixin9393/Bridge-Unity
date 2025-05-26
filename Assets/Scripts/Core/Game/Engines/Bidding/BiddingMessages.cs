using System;

public static class BiddingMessages {
    // Openings
    public static string OneNT(int HCP)
        => string.Format("Apertura {0} {1}", With(HCP), HCPRange(15, 17));

    // Interventions
    public const string OneNTInterventionPass = "Pass";

    // Responses
    // 1NT
    public static string OneNTResponsePass(int HCP)
        => string.Format("{0} {1}", ResponseTo(1, Strain.NoTrump), With(HCP));  // Pass

    public static string OneNTResponse2Clubs(int hCP)
        => string.Format("Stayman. {0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), With(hCP), MoreThan(8), FourMajorCards()); // 2 Clubs

    public static string OneNTResponse2NT(int HCP)
        => string.Format("{0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), With(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2 NT

    public static string OneNTResponse3NT(int HCP)
        => string.Format("{0} {1} {2}", ResponseTo(1, Strain.NoTrump), With(HCP), MoreThan(10)); // 3 NT

    // Unknown
    public const string Unknown = "Subasta desconocida: Pass";


    private static string CanReachGame(int minHCP, int maxHCP, int currentHCP) {
        const int HCPNeeded = 25;
        int allyMaxHCP = HCPNeeded - minHCP;
        string allyHCPRange;
        if (currentHCP != minHCP)
            allyHCPRange = string.Format("{0}-{1}", HCPNeeded - currentHCP, allyMaxHCP);
        else
            allyHCPRange = allyMaxHCP.ToString();
        return string.Format("Puede llegar a manga (25-32) si el compañero tiene {0} HCP", allyHCPRange);
    }

    private static string HCPRange(int minHCP, int maxHCP) {
        return string.Format("({0}-{1}).", minHCP, maxHCP);
    }

    private static string With(int HCP) {
        return string.Format("con {0} HCP", HCP);
    }

    private static string ResponseTo(int level, Strain strain) {
        return string.Format("Respuesta a {0}{1}", level, strain.ToSymbol());
    }

    private static string MoreThan(int HCP) {
        return string.Format("({0}+).", HCP);
    }

    private static object FourMajorCards() {
        return string.Format("4+ cartas mayores (picas/corazones)");
    }
}