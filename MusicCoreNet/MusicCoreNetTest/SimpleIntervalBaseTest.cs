using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    /// A series of pairs of <see cref="SimpleIntervalBase"/> instances and their associated perfect-unison-based
    /// circle of fifths indexes.
    /// </summary>
    private static readonly ImmutableArray<(SimpleIntervalBase Interval, int Index)> CircleOfFifthsIndexPairs
        = ImmutableArray.CreateRange(new (SimpleIntervalBase, int)[]
        {
            (SimpleIntervalBase.PerfectUnison, 0),
            (Intervals.Minor().Second(), -5),
            (Intervals.Major().Second(), 2),
            (Intervals.Augmented(2).Sixth(), 17),
            (Intervals.Diminished().Third(), -10),
        });

    /// <summary>
    /// Tests the <see cref="SimpleIntervalBase.CircleOfFifthsIndex"/> property.
    /// </summary>
    [TestMethod]
    public void TestPerfectUnisonBasedIndex()
    {
        foreach (var (Interval, Index) in CircleOfFifthsIndexPairs)
        {
            Assert.AreEqual(
                Index, Interval.CircleOfFifthsIndex,
                $"Invalid '{Interval}' {nameof(SimpleIntervalBase.CircleOfFifthsIndex)} value.");
        }
    }

    /// <summary>
    /// Tests the <see cref="SimpleIntervalBase.FromCircleOfFifthsIndex(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromCircleOfFifthsIndex()
    {
        foreach (var (Interval, Index) in CircleOfFifthsIndexPairs)
        {
            Assert.AreEqual(
                Interval, SimpleIntervalBase.FromCircleOfFifthsIndex(Index),
                $"Invalid {nameof(TestFromCircleOfFifthsIndex)}({Index}) result.");
        }
    }

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
