using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="SimpleInterval"/> struct.
/// </summary>
[TestClass]
public class SimpleIntervalTest
{
    /// <summary>
    /// Tests the <see cref="SimpleInterval.Create(IntervalQuality, SimpleIntervalNumber)"/> method.
    /// </summary>
    [TestMethod]
    public void TestCreate()
    {
        // This should not fail
        Assert.AreEqual(
            Interval.Augmented().Fifth,
            SimpleInterval.Create(IntervalQuality.Augmented(), SimpleIntervalNumber.Fifth));

        Assert.AreEqual(
            Interval.Minor.Third,
            SimpleInterval.Create(IntervalQuality.Minor, SimpleIntervalNumber.Third));

        // These should fail (as they are mismatched)
        Assert.ThrowsException<ArgumentException>(
            () => SimpleInterval.Create(IntervalQuality.Major, SimpleIntervalNumber.Fourth));
        Assert.ThrowsException<ArgumentException>(
            () => SimpleInterval.Create(IntervalQuality.Perfect, SimpleIntervalNumber.Third));
    }

    /// <summary>
    /// A series of pairs of <see cref="SimpleInterval"/> instances and their associated perfect-unison-based
    /// circle of fifths indexes.
    /// </summary>
    private static readonly ImmutableArray<(SimpleInterval Interval, int Index)> CircleOfFifthsIndexPairs
        = ImmutableArray.CreateRange(new (SimpleInterval, int)[]
        {
            (SimpleInterval.PerfectUnison, 0),
            (Interval.Minor.Second, -5),
            (Interval.Major.Second, 2),
            (Interval.Augmented(2).Sixth, 17),
            (Interval.Diminished().Third, -10),
        });

    /// <summary>
    /// Tests the <see cref="SimpleInterval.CircleOfFifthsIndex"/> property.
    /// </summary>
    [TestMethod]
    public void TestPerfectUnisonBasedIndex()
    {
        foreach (var (Interval, Index) in CircleOfFifthsIndexPairs)
        {
            Assert.AreEqual(
                Index, Interval.CircleOfFifthsIndex,
                $"Invalid '{Interval}' {nameof(SimpleInterval.CircleOfFifthsIndex)} value.");
        }
    }

    /// <summary>
    /// Tests the <see cref="SimpleInterval.FromCircleOfFifthsIndex(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromCircleOfFifthsIndex()
    {
        foreach (var (Interval, Index) in CircleOfFifthsIndexPairs)
        {
            Assert.AreEqual(
                Interval, SimpleInterval.FromCircleOfFifthsIndex(Index),
                $"Invalid {nameof(TestFromCircleOfFifthsIndex)}({Index}) result.");
        }
    }

    /// <summary>
    /// Tests negation of <see cref="SimpleInterval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestNegation()
    {
        TestInversionPair(Interval.Perfect.Fourth, Interval.Perfect.Fifth);
        TestInversionPair(Interval.Augmented(3).Second, Interval.Diminished(3).Seventh);
        TestInversionPair(Interval.Minor.Sixth, Interval.Major.Third);
        TestInversionPair(Interval.Augmented().Fourth, Interval.Diminished().Fifth);
    }

    private static void TestInversionPair(SimpleInterval first, SimpleInterval second)
    {
        Assert.AreEqual(first, -second);
        Assert.AreEqual(second, -first);
    }

    /// <summary>
    /// Tests addition of <see cref="SimpleInterval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        TestAdditionPair(Interval.Major.Second, Interval.Major.Second, Interval.Major.Third);
        TestAdditionPair(Interval.Perfect.Fourth, Interval.Perfect.Fourth, Interval.Minor.Seventh);
        TestAdditionPair(Interval.Augmented(2).Third, Interval.Diminished(2).Sixth, Interval.Perfect.Unison);
        TestAdditionPair(Interval.Major.Sixth, Interval.Minor.Second, Interval.Minor.Seventh);
        TestAdditionPair(Interval.Major.Seventh, Interval.Major.Sixth, Interval.Augmented().Fifth);
    }

    private static void TestAdditionPair(
        SimpleInterval first, SimpleInterval second, SimpleInterval expectedResult)
    {
        Assert.AreEqual(expectedResult, first + second);
        Assert.AreEqual(expectedResult, second + first);
    }
}
