public static class BiddingMessages {
    // Openings
    public static string OpeningPass(int HCP)
        => string.Format("Apertura {0} {1}, no tienes suficientes HCP para abrir", WithHCP(HCP), HCPRange(0, 11));
    public static string OpeningOneNT(int HCP)
        => string.Format("Apertura {0} {1} con {2}", WithHCP(HCP), HCPRange(15, 17), BalancedHand);

    // Interventions

    // 1NT
    // Responses
    public static string OneNTResponsePass(int HCP)
        => string.Format("{0} {1} {2}, no alcanza a {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(0, 7), GameRange);  // Pass

    public static string OneNTResponse2Clubs(int hCP)
        => string.Format("Stayman. {0} {1} {2} y {3}", ResponseTo(1, Strain.NoTrump), WithHCP(hCP), MoreThan(8), FourMajorCards()); // 2 Clubs

    public static string OneNTResponse2NT(int HCP)
        => string.Format("{0} {1} {2} {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2 NT

    public static string OneNTResponse3NT(int HCP)
        => string.Format("{0} {1} {2} alcanza a {3}", ResponseTo(1, Strain.NoTrump), WithHCP(HCP), MoreThan(10), GameRange); // 3 NT
    
    // Stayman
    // 2D
    public static string OneNTStayman2DResponse2NT(int HCP)
        => string.Format("Stayman. Respuesta a {0} {1} {2} {3}", ResponseTo(2, Strain.Diamonds), WithHCP(HCP), HCPRange(8, 9), CanReachGame(8, 9, HCP)); // 2NT

    public static string OneNTStayman2DResponse3NT(int HCP)
        => string.Format("Stayman. Respuesta a {0} {1} {2} alcanza a {3}", ResponseTo(2, Strain.Diamonds), WithHCP(HCP), MoreThan(10), GameRange); // 3 NT
    
    // 2H
    public static string OneNTStayman2HResponse2NT(int HCP)
        => string.Format("Stayman. Respuesta a {0} {1}. {2}. No tienes 4 corazones, pero sí 4+ picas", ResponseTo(2, Strain.Hearts), WithHCP(HCP), CanReachGame(8, 9, HCP)); // 2NT
    
    public static string OneNTStayman2HResponse3H(int totalPoints)
        => string.Format("Stayman. Respuesta a {0} {1} {2}, {3} corazones. no alcanza a {4}", ResponseTo(2, Strain.Hearts), WithTP(totalPoints), HCPRange(8, 9), MoreThan(4), GameRange); // 3H
    
    public static string OneNTStayman2HResponse3NT(int HCP)
        => string.Format("Stayman. Respuesta a {0} {1}. {2}. No tienes corazones, pero sí 4+ picas y {3}", ResponseTo(2, Strain.Hearts), WithHCP(HCP), HCPRange(10, 15), GameRange); // 3NT

    public static string OneNTStayman2HResponse4H(int totalPoints)
        => string.Format("Stayman. Respuesta a {0} {1} {2}, {3} corazones y alcanza a {4}", ResponseTo(2, Strain.Hearts), WithTP(totalPoints), HCPRange(10, 15), MoreThan(4), GameRange); // 4H

    // 2S
    public static string OneNTStayman2SResponse3S(int totalPoints)
        => string.Format("Stayman. Respuesta a {0} {1} {2}, {3} picas. Invitación", ResponseTo(2, Strain.Spades), WithTP(totalPoints), HCPRange(8, 9), MoreThan(4)); // 3S

    public static string OneNTStayman2SResponse4S(int totalPoints)
        => string.Format("Stayman. Respuesta a {0} {1} {2}, {3} picas y alcanza a {4}", ResponseTo(2, Strain.Spades), WithTP(totalPoints), HCPRange(10, 15), MoreThan(4), GameRange); // 4S


    // Rebid
    public static string OneNTRebidNotEnoughHCP
        => string.Format("Compañero Pass {0}, no alcanza a {1}", HCPRange(0, 7), GameRange); // Pass
    
    public static string OneNTRebid2Diamonds
        => string.Format("Stayman. Compañero 2C, no tienes {0}", FourMajorCards()); // 2D

    public static string OneNTRebid2Hearts
        => string.Format("Stayman. Compañero 2C, tienes {0}", FourMajorCards()); // 2H
    
    public static string OneNTRebid2Spades
        => string.Format("Stayman. Compañero 2C, tienes {0} picas y no {1} corazones", MoreThan(4), MoreThan(4)); // 2S
    
    public static string OneNTRebidPass(int HCP)
        => string.Format("Compañero 2NT {0} {1}, no alcanza a {2}", HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass
    
    public static string OneNTRebidGame(int HCP)
        => string.Format("Compañero 3NT {0} {1} {2}", MoreThan(10), WithHCP(HCP), GameRange); // Pass


    private static string CanReachGame(int minHCP, int maxHCP, int currentHCP) {
        const int HCPNeeded = 25;
        int allyMaxHCP = HCPNeeded - minHCP;
        string allyHCPRange;
        if (currentHCP != minHCP)
            allyHCPRange = string.Format("{0}-{1}", HCPNeeded - currentHCP, allyMaxHCP);
        else
            allyHCPRange = allyMaxHCP.ToString();
        return string.Format("Puede llegar a {0} si el compañero tiene {1} HCP", GameRange, allyHCPRange);
    }
    
    public static readonly string Unknown = "Subasta desconocida";
    public static readonly string AuctionConcludedPass = "Se ha llegado a un acuerdo.";
    private static readonly string BalancedHand = string.Format("mano equilibrada");
    private static readonly string GameRange = string.Format("manga (25-32)");
    private static string HCPRange(int minHCP, int maxHCP) => string.Format("({0}-{1})", minHCP, maxHCP);
    private static string WithHCP(int HCP) => string.Format("con {0} HCP", HCP);
    private static string WithTP(int totalPoints) => string.Format("con {0} TP", totalPoints);
    private static string ResponseTo(int level, Strain strain) => string.Format("Respuesta a {0}{1}", level, strain.ToSymbol());
    private static string MoreThan(int HCP) => string.Format("({0}+)", HCP);
    private static object FourMajorCards() => string.Format("4+ cartas mayores (picas/corazones)");
}