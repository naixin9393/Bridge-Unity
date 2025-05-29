public static class BiddingMessages {
    // Openings
    public static string OpeningPass(int HCP)
        => string.Format("Apertura {0} {1}, no tienes suficientes HCP para abrir", WithHCP(HCP), HCPRange(0, 11));
    public static string OpeningOneNT(int HCP)
        => string.Format("Apertura {0} {1} con {2}", WithHCP(HCP), HCPRange(15, 17), BalancedHand);

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

    // Rebid
    public static string OneNTRebidNotEnoughHCP
        => string.Format("Compañero Pass {0}, no alcanza a {1}", HCPRange(0, 7), GameRange); // Pass
    
    public static string StaymanResponse2D
        => string.Format("Stayman. Compañero 2C, no tienes {0}", FourMajorCards()); // 2D

    public static string StaymanResponse2H
        => string.Format("Stayman. Compañero 2C, tienes {0}", FourMajorCards()); // 2H
    
    public static string StaymanResponse2S
        => string.Format("Stayman. Compañero 2C, tienes {0} picas y no {1} corazones", MoreThan(4), MoreThan(4)); // 2S
    
    public static string OneNTRebidPass(int HCP)
        => string.Format("Compañero 2NT {0} {1}, no alcanza a {2}", HCPRange(8, 9), WithHCP(HCP), GameRange); // Pass

    public static string OneNTRebidPass2(int HCP)
        => string.Format("{0} {1} {2}, no alcanza a {3}", ResponseTo(4, Strain.NoTrump), WithHCP(HCP), HCPRange(15, 16), SlamRange);  // Pass
    
    public static string OneNTRebid3NT(int HCP)
        => string.Format("Compañero 2NT {0} {1} {2}",HCPRange(8,9), WithHCP(HCP), GameRange); // Pass
    
    public static string OneNTRebid6NT(int HCP)
        => string.Format("Compañero 4NT {0} {1} {2}", HCPRange(16, 17), WithHCP(HCP), SlamRange); // Pass

    private static string CanReachGame(int minHCP, int maxHCP, int currentHCP) {
        const int HCPNeeded = 25;
        int allyMaxHCP = HCPNeeded - minHCP;
        string allyHCPRange;
        if (currentHCP != minHCP)
            allyHCPRange = string.Format("{0}-{1}", HCPNeeded - currentHCP, allyMaxHCP);
        else
            allyHCPRange = allyMaxHCP.ToString();
        return string.Format("puede llegar a {0} si el compañero tiene {1} HCP, invitativa a manga", GameRange, allyHCPRange);
    }
    
    public static readonly string Unknown = "Subasta desconocida";
    public static readonly string AuctionConcludedPass = "Se ha llegado a un acuerdo.";
    private static readonly string BalancedHand = string.Format("mano equilibrada");
    private static readonly string GameRange = string.Format("manga (25-32)");
    private static readonly string SlamRange = string.Format("slam (33-36)");
    private static string HCPRange(int minHCP, int maxHCP) => string.Format("({0}-{1})", minHCP, maxHCP);
    private static string WithHCP(int HCP) => string.Format("con {0} HCP", HCP);
    private static string WithTP(int totalPoints) => string.Format("con {0} TP", totalPoints);
    private static string ResponseTo(int level, Strain strain) => string.Format("Respuesta a {0}{1}", level, strain.ToSymbol());
    private static string MoreThan(int HCP) => string.Format("({0}+)", HCP);
    private static object FourMajorCards() => string.Format("4+ cartas mayores (picas/corazones)");
}