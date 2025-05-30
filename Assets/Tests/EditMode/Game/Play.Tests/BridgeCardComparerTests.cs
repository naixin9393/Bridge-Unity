using NUnit.Framework;

using BridgeEdu.Core;
using BridgeEdu.Game.Bidding;
using BridgeEdu.Game.Play;

namespace BridgeEdu.Game.Cards.Tests {
    public class BridgeCardComparerTests {
        [TestCase(Rank.Ace, Suit.Diamonds, Rank.Two, Suit.Diamonds, Suit.Diamonds, Strain.NoTrump, 1, TestName = "NoTrump_BothCardsFollowLead_CompareRank")]
        [TestCase(Rank.Four, Suit.Diamonds, Rank.King, Suit.Diamonds, Suit.Diamonds, Strain.NoTrump, -1, TestName = "NoTrump_BothCardsFollowLead_CompareRank")]
        [TestCase(Rank.Ace, Suit.Diamonds, Rank.Ace, Suit.Diamonds, Suit.Diamonds, Strain.NoTrump, 0, TestName = "NoTrump_BothCardsFollowLead_CompareRank")]

        [TestCase(Rank.Two, Suit.Clubs, Rank.Four, Suit.Diamonds, Suit.Clubs, Strain.NoTrump, 1, TestName = "NoTrump_OneCardFollowLead_CardWithFollowLeadIsHigher")]
        [TestCase(Rank.Two, Suit.Clubs, Rank.Four, Suit.Spades, Suit.Spades, Strain.NoTrump, -1, TestName = "NoTrump_OneCardFollowLead_CardWithFollowLeadIsHigher")]

        [TestCase(Rank.Three, Suit.Diamonds, Rank.Four, Suit.Diamonds, Suit.Clubs, Strain.NoTrump, -1, TestName = "NoTrump_NoCardFollowLead_BothSameSuit_CompareRank")]
        [TestCase(Rank.Four, Suit.Diamonds, Rank.Four, Suit.Diamonds, Suit.Clubs, Strain.NoTrump, 0, TestName = "NoTrump_NoCardFollowLead_BothSameSuit_CompareRank")]
        [TestCase(Rank.Queen, Suit.Diamonds, Rank.Four, Suit.Diamonds, Suit.Clubs, Strain.NoTrump, 1, TestName = "NoTrump_NoCardFollowLead_BothSameSuit_CompareRank")]

        [TestCase(Rank.Three, Suit.Clubs, Rank.Four, Suit.Spades, Suit.Hearts, Strain.NoTrump, -1, TestName = "NoTrump_NoCardFollowLead_BothDifferentSuit_CompareSuit")]
        [TestCase(Rank.Three, Suit.Clubs, Rank.Four, Suit.Diamonds, Suit.Hearts, Strain.NoTrump, 1, TestName = "NoTrump_NoCardFollowLead_BothDifferentSuit_CompareSuit")]

        [TestCase(Rank.Three, Suit.Hearts, Rank.Four, Suit.Hearts, Suit.Hearts, Strain.Hearts, -1, TestName = "Trump_BothCardsTrump_CompareRank")]
        [TestCase(Rank.Seven, Suit.Hearts, Rank.Four, Suit.Hearts, Suit.Hearts, Strain.Hearts, 1, TestName = "Trump_BothCardsTrump_CompareRank")]
        [TestCase(Rank.Four, Suit.Hearts, Rank.Four, Suit.Hearts, Suit.Hearts, Strain.Hearts, 0, TestName = "Trump_BothCardsTrump_CompareRank")]

        [TestCase(Rank.Three, Suit.Spades, Rank.Four, Suit.Hearts, Suit.Hearts, Strain.Hearts, -1, TestName = "Trump_OneCardTrump_TrumpCardIsHigher")]
        [TestCase(Rank.Three, Suit.Hearts, Rank.Four, Suit.Clubs, Suit.Hearts, Strain.Hearts, 1, TestName = "Trump_OneCardTrump_TrumpCardIsHigher")]

        [TestCase(Rank.Ace, Suit.Spades, Rank.Five, Suit.Hearts, Suit.Hearts, Strain.Clubs, -1, TestName = "Trump_NoCardsTrump_OneFollowsLead_LeadCardIsHigher")]
        [TestCase(Rank.Four, Suit.Diamonds, Rank.Four, Suit.Clubs, Suit.Clubs, Strain.Hearts, -1, TestName = "Trump_NoCardsTrump_OneFollowsLead_CompareSuit")]

        [TestCase(Rank.Three, Suit.Clubs, Rank.Four, Suit.Clubs, Suit.Clubs, Strain.Hearts, -1, TestName = "Trump_NoCardsTrump_BothSameSuit_CompareRank")]
        [TestCase(Rank.Seven, Suit.Clubs, Rank.Four, Suit.Clubs, Suit.Clubs, Strain.Hearts, 1, TestName = "Trump_NoCardsTrump_BothSameSuit_CompareRank")]
        [TestCase(Rank.Four, Suit.Diamonds, Rank.Four, Suit.Spades, Suit.Clubs, Strain.Hearts, -1, TestName = "Trump_NoCardsTrump_BothDifferentSuit_CompareSuit")]
        public void Compare_ShouldReturnExpectedResult(Rank rank1, Suit suit1, Rank rank2, Suit suit2, Suit leadSuit, Strain contractStrain, int expectedResult) {
            var bridgeCardComparer = new BridgeCardComparer(leadSuit, contractStrain);
            int result = bridgeCardComparer.Compare(new Card(rank1, suit1), new Card(rank2, suit2));
            Assert.AreEqual(expectedResult, result);
        }
    }
}