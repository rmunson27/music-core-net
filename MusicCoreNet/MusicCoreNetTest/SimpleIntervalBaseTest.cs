using Rem.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="SimpleIntervalBase"/> class and subclasses.
/// </summary>
[TestClass]
public class SimpleIntervalBaseTest
{
    /// <summary>
    /// Tests negation of a simple interval base.
    /// </summary>
    [TestMethod]
    public void TestNegation()
    {
        TestInversionPair(Intervals.Perfect().Fourth(), Intervals.Perfect().Fifth());
        TestInversionPair(Intervals.Augmented(3).Second(), Intervals.Diminished(3).Seventh());
        TestInversionPair(Intervals.Minor().Sixth(), Intervals.Major().Third());
        TestInversionPair(Intervals.Augmented().Fourth(), Intervals.Diminished().Fifth());
    }

    private static void TestInversionPair(SimpleIntervalBase first, SimpleIntervalBase second)
    {
        Assert.AreEqual(first, -second);
        Assert.AreEqual(second, -first);
    }
}
