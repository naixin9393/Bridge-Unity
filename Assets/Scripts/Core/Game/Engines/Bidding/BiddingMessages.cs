public static class BiddingMessages {
    // Openings
    public static string OpeningPass(int HCP)
        => string.Format("Apertura {0} {1}, no tienes suficientes HCP para abrir", With(HCP), HCPRange(0, 11));
    public static string OpeningOneNT(int HCP)
        => string.Format("Apertura {0} {1}", With(HCP), HCPRange(15, 17));

    // Interventions
    public const string OneNTInterventionPass = "Pass";

    // 1NT
    // Responses
    public static string OneNTResponsePass(int HCP)
        => string.Format("{0} {1}, no alcanza a {2}", ResponseTo(1, Strain.NoTrump), With(HCP), GameRange());  // Pass

    public static string OneNTResponse2Clubs(int hCP)
        => string.Format("Stayman. {0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), With(hCP), MoreThan(8), FourMajorCards()); // 2 Clubs

    public static string OneNTResponse2NT(int HCP)
        => string.Format("{0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), With(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2 NT

    public static string OneNTResponse3NT(int HCP)
        => string.Format("{0} {1} {2} alcanza a {3}", ResponseTo(1, Strain.NoTrump), With(HCP), MoreThan(10), GameRange()); // 3 NT

    // Unknown
    public const string Unknown = "Subasta desconocida: Pass";

    // Rebid
    public static string OneNTRebidNotEnoughHCP
        => string.Format("Compañero Pass {0}, no alcanza a {1}", HCPRange(0, 7), GameRange()); // Pass
    
    public static string OneNTRebid2Diamonds
        => string.Format("Stayman. Compañero 2C, no tienes {0}", FourMajorCards()); // 2D

    public static string OneNTRebid2Hearts
        => string.Format("Stayman. Compañero 2C, tienes {0}", FourMajorCards()); // 2H
    
    public static string OneNTRebid2Spades
        => string.Format("Stayman. Compañero 2C, tienes {0} picas y no {1} corazones", MoreThan(4), MoreThan(4)); // 2S
    
    public static string OneNTRebidPass(int HCP)
        => string.Format("Compañero 2NT {0} {1}, no alcanza a {1}", HCPRange(8, 9), With(HCP), MoreThan(8), GameRange()); // Pass
    
    public static string OneNTRebidGame(int HCP)
        => string.Format("Compañero 3NT {0} {1} {2}", MoreThan(10), With(HCP), GameRange()); // Pass


    private static string CanReachGame(int minHCP, int maxHCP, int currentHCP) {
        const int HCPNeeded = 25;
        int allyMaxHCP = HCPNeeded - minHCP;
        string allyHCPRange;
        if (currentHCP != minHCP)
            allyHCPRange = string.Format("{0}-{1}", HCPNeeded - currentHCP, allyMaxHCP);
        else
            allyHCPRange = allyMaxHCP.ToString();
        return string.Format("Puede llegar a {0} si el compañero tiene {1} HCP", GameRange(), allyHCPRange);
    }

    private static string GameRange() {
        return string.Format("manga (25-32)");
    }

    private static string HCPRange(int minHCP, int maxHCP) {
        return string.Format("({0}-{1})", minHCP, maxHCP);
    }

    private static string With(int HCP) {
        return string.Format("con {0} HCP", HCP);
    }

    private static string ResponseTo(int level, Strain strain) {
        return string.Format("Respuesta a {0}{1}", level, strain.ToSymbol());
    }

    private static string MoreThan(int HCP) {
        return string.Format("({0}+)", HCP);
    }

    private static object FourMajorCards() {
        return string.Format("4+ cartas mayores (picas/corazones)");
    }
}