using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="Interval"/> struct.
/// </summary>
[TestClass]
public class IntervalTest
{
    /// <summary>
    /// Tests addition of two <see cref="Interval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        TestAdditionPair(Intervals.Perfect().Fourth(), Intervals.Major().Third(), Intervals.Major().Sixth());
        TestAdditionPair(
            Intervals.Perfect().Fourth(), Intervals.Perfect().Fifth(),
            new(Intervals.Perfect().Unison(), AdditionalOctaves: 1));
        TestAdditionPair(
            new(Intervals.Perfect().Fourth(), AdditionalOctaves: 1),
            new(Intervals.Major().Sixth(), AdditionalOctaves: 2),
            new(Intervals.Major().Second(), AdditionalOctaves: 4));
    }
    
    private static void TestAdditionPair(Interval first, Interval second, Interval expectedResult)
    {
        Assert.AreEqual(expectedResult, first + second);
        Assert.AreEqual(expectedResult, second + first);
    }

    /// <summary>
    /// Tests subtraction of two <see cref="Interval"/> instances.
    /// </summary>
    [TestMethod]
    public void TestSubtraction()
    {
        TestSubtractionPair(Intervals.Major().Sixth(), Intervals.Perfect().Fourth(), Intervals.Major().Third());
        TestSubtractionPair(
            new(Intervals.Perfect().Unison(), AdditionalOctaves: 1),
            Intervals.Perfect().Fourth(), Intervals.Perfect().Fifth());
        TestSubtractionPair(
            new(Intervals.Major().Second(), AdditionalOctaves: 4),
            new(Intervals.Perfect().Fourth(), AdditionalOctaves: 1),
            new(Intervals.Major().Sixth(), AdditionalOctaves: 2));

        // Ensure underflows are exceptional
        Assert.ThrowsException<OverflowException>(
            () => (Interval)Intervals.Perfect().Fourth() - Intervals.Perfect().Fifth());
        Assert.ThrowsException<OverflowException>(
            () => Intervals.Perfect().Fourth() - new Interval(Intervals.Perfect().Unison(), 1));
    }

    private static void TestSubtractionPair(Interval lhs, Interval rhs1, Interval rhs2)
    {
        Assert.AreEqual(rhs2, lhs - rhs1);
        Assert.AreEqual(rhs1, lhs - rhs2);
    }
}
