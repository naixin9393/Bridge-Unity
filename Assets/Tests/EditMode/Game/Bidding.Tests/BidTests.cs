using NUnit.Framework;

namespace BridgeEdu.Game.Bidding.Tests {
    public class BidTests {
        [TestCase(1, Strain.Clubs, 1, Strain.Diamonds, true, TestName = "SameLevel_bid1LowerStrain")]
        [TestCase(1, Strain.Hearts, 1, Strain.Diamonds, false, TestName = "SameLevel_bid1HigherStrain")]
        [TestCase(2, Strain.Diamonds, 2, Strain.Diamonds, false, TestName = "SameLevel_SameStrain")]
        [TestCase(1, Strain.Diamonds, 2, Strain.Diamonds, true, TestName = "SameStrain_bid1LowerLevel")]
        [TestCase(2, Strain.Hearts, 1, Strain.Hearts, false, TestName = "SameStrain_bid1HigherLevel")]
        [TestCase(3, Strain.Clubs, 7, Strain.Diamonds, true, TestName = "Bid1LowerLevel")]
        [TestCase(5, Strain.Clubs, 3, Strain.Diamonds, false, TestName = "Bid1HigherLevel")]
        public void Bid_LessThanOperator_ShouldReturnExpected(int level1, Strain strain1, int level2, Strain strain2, bool expected) {
            var bid1 = new Bid(level1, strain1);
            var bid2 = new Bid(level2, strain2);
            Assert.AreEqual(expected, bid1 < bid2);
        }

        [TestCase(1, Strain.Clubs, 1, Strain.Diamonds, false, TestName = "SameLevel_bid1LowerStrain")]
        [TestCase(1, Strain.Hearts, 1, Strain.Diamonds, true, TestName = "SameLevel_bid1HigherStrain")]
        [TestCase(2, Strain.Diamonds, 2, Strain.Diamonds, false, TestName = "SameLevel_SameStrain")]
        [TestCase(1, Strain.Diamonds, 2, Strain.Diamonds, false, TestName = "SameStrain_bid1LowerLevel")]
        [TestCase(2, Strain.Hearts, 1, Strain.Hearts, true, TestName = "SameStrain_bid1HigherLevel")]
        [TestCase(3, Strain.Clubs, 7, Strain.Diamonds, false, TestName = "Bid1LowerLevel")]
        [TestCase(5, Strain.Clubs, 3, Strain.Diamonds, true, TestName = "Bid1HigherLevel")]
        public void Bid_GreaterThanOperator_ShouldReturnExpected(int level1, Strain strain1, int level2, Strain strain2, bool expected) {
            var bid1 = new Bid(level1, strain1);
            var bid2 = new Bid(level2, strain2);
            Assert.AreEqual(expected, bid1 > bid2);
        }
    }
}