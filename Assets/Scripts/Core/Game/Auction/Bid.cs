public class Bid {
    public int Level { get; private set; }
    public Strain Strain { get; private set; }

    public Bid(int level, Strain strain) {
        Level = level;
        Strain = strain;
    }
    
    public static bool operator >(Bid bid1, Bid bid2) {
        if (bid1.Level == bid2.Level) {
            return bid1.Strain > bid2.Strain;
        }
        return bid1.Level > bid2.Level;
    }
    
    public static bool operator <(Bid bid1, Bid bid2) {
        if (bid1.Level == bid2.Level) {
            return bid1.Strain < bid2.Strain;
        }
        return bid1.Level < bid2.Level;
    }
}
