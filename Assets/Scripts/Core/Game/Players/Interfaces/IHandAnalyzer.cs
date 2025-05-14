using System.Collections.Generic;

public interface IHandAnalyzer {
    public int CalculateHighCardPoints(List<Card> hand);
    public bool ContainsBalancedHand(List<Card> hand);
    public bool IsBalancedHand(List<Card> hand);
}