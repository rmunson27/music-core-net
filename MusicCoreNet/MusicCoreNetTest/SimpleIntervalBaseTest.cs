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
    /// Tests negation of <see cref="SimpleIntervalBase"/> instances.
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

    /// <summary>
    /// Tests addition of <see cref="SimpleIntervalBase"/> instances.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        TestAdditionPair(Intervals.Major().Second(), Intervals.Major().Second(), Intervals.Major().Third());
        TestAdditionPair(Intervals.Perfect().Fourth(), Intervals.Perfect().Fourth(), Intervals.Minor().Seventh());
        TestAdditionPair(Intervals.Augmented(2).Third(), Intervals.Diminished(2).Sixth(), Intervals.Perfect().Unison());
        TestAdditionPair(Intervals.Major().Sixth(), Intervals.Minor().Second(), Intervals.Minor().Seventh());
        TestAdditionPair(Intervals.Major().Seventh(), Intervals.Major().Sixth(), Intervals.Augmented().Fifth());
    }

    private static void TestAdditionPair(
        SimpleIntervalBase first, SimpleIntervalBase second, SimpleIntervalBase expectedResult)
    {
        Assert.AreEqual(expectedResult, first + second);
        Assert.AreEqual(expectedResult, second + first);
    }
}
