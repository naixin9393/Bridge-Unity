namespace BridgeEdu.Game.Bidding {
    public class Bid {
        public int Level { get; private set; }
        public Strain Strain { get; private set; }

        public Bid(int level, Strain strain) {
            if (level > 7)
                throw new InvalidCallException("Level must be between 1 and 7");
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

        public override bool Equals(object other) {
            if (other == null || GetType() != other.GetType()) return false;
            Bid otherBid = (Bid)other;
            return Level == otherBid.Level && Strain == otherBid.Strain;
        }

        public override int GetHashCode() {
            return 5 * Level.GetHashCode() + 17 * Strain.GetHashCode();
        }

        public override string ToString() {
            return $"{Level}{Strain.ToSymbol()}";
        }
    }
}