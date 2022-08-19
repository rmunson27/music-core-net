using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests for the <see cref="SignedInterval"/> class.
/// </summary>
[TestClass]
public class SignedIntervalTest
{
    /// <summary>
    /// Tests <see cref="SignedInterval"/> factory methods.
    /// </summary>
    [TestMethod]
    public void TestFactories()
    {
        Interval unison = Intervals.Diminished().Unison();
        Interval nonUnison = Intervals.Augmented().Fourth();

        var posNonUnison = SignedInterval.Positive(nonUnison);
        Assert.AreEqual(1, posNonUnison.Sign);
        Assert.AreEqual(nonUnison, posNonUnison.Interval);

        var negNonUnison = SignedInterval.Negative(nonUnison);
        Assert.AreEqual(-1, negNonUnison.Sign);
        Assert.AreEqual(nonUnison, negNonUnison.Interval);

        var posUnison = SignedInterval.Positive(unison);
        Assert.AreEqual(1, posUnison.Sign);
        Assert.AreEqual(unison, posUnison.Interval);

        // The negative unison should be simplified in the constructor to be positive to avoid ambiguity
        var negUnison = SignedInterval.Negative(unison);
        Assert.AreEqual(1, negUnison.Sign);
        Assert.AreEqual(unison with { Base = -unison.Base }, negUnison.Interval);
    }

    /// <summary>
    /// Tests negation of <see cref="SignedInterval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestNegate()
    {
        TestNegatePair(Intervals.Perfect().Fifth());
        TestNegatePair(Intervals.Diminished().Unison());
    }

    private static void TestNegatePair(Interval interval)
    {
        var pos = SignedInterval.Positive(interval);
        var neg = SignedInterval.Negative(interval);
        Assert.AreEqual(pos, -neg);
        Assert.AreEqual(neg, -pos);
    }

    /// <summary>
    /// Tests addition of <see cref="SignedInterval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        TestAdditionPair(Intervals.Major().Sixth(), Intervals.Minor().Third(), Interval.PerfectOctave);
        TestAdditionPair(
            new Interval(Intervals.Major().Second(), AdditionalOctaves: 1),
            SignedInterval.Negative(Intervals.Perfect().Fourth()),
            Intervals.Major().Sixth());
        TestAdditionPair(
            Intervals.Minor().Second(),
            SignedInterval.Negative(Intervals.Perfect().Fifth()),
            SignedInterval.Negative(Intervals.Augmented().Fourth()));
        TestAdditionPair(
            Intervals.Perfect().Fourth(),
            SignedInterval.Negative(new(Intervals.Perfect().Fourth(), AdditionalOctaves: 1)),
            SignedInterval.Negative(Interval.PerfectOctave));
    }

    private static void TestAdditionPair(SignedInterval first, SignedInterval second, SignedInterval expectedResult)
    {
        Assert.AreEqual(expectedResult, first + second);
        Assert.AreEqual(expectedResult, second + first);
    }

    /// <summary>
    /// Tests subtraction of <see cref="SignedInterval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestSubtraction()
    {
        TestDifferencePair(Interval.PerfectOctave, Intervals.Major().Sixth(), Intervals.Minor().Third());
        TestDifferencePair(
            Intervals.Major().Sixth(),
            new Interval(Intervals.Major().Second(), AdditionalOctaves: 1),
            SignedInterval.Negative(Intervals.Perfect().Fourth()));
        TestDifferencePair(
            SignedInterval.Negative(Intervals.Augmented().Fourth()),
            Intervals.Minor().Second(),
            SignedInterval.Negative(Intervals.Perfect().Fifth()));
        TestDifferencePair(
            SignedInterval.Negative(Interval.PerfectOctave),
            Intervals.Perfect().Fourth(),
            SignedInterval.Negative(new(Intervals.Perfect().Fourth(), AdditionalOctaves: 1)));
    }

    private static void TestDifferencePair(SignedInterval lhs, SignedInterval rhs1, SignedInterval rhs2)
    {
        Assert.AreEqual(rhs1, lhs - rhs2);
        Assert.AreEqual(rhs2, lhs - rhs1);
    }
}
