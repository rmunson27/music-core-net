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
        TestAdditionPair(Interval.Perfect().Fourth(), Interval.Major().Third(), Interval.Major().Sixth());
        TestAdditionPair(
            Interval.Perfect().Fourth(), Interval.Perfect().Fifth(),
            new(Interval.Perfect().Unison(), AdditionalOctaves: 1));
        TestAdditionPair(
            new(Interval.Perfect().Fourth(), AdditionalOctaves: 1),
            new(Interval.Major().Sixth(), AdditionalOctaves: 2),
            new(Interval.Major().Second(), AdditionalOctaves: 4));
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
        TestDifferencePair(Interval.Major().Sixth(), Interval.Perfect().Fourth(), Interval.Major().Third());
        TestDifferencePair(
            new(Interval.Perfect().Unison(), AdditionalOctaves: 1),
            Interval.Perfect().Fourth(), Interval.Perfect().Fifth());
        TestDifferencePair(
            new(Interval.Major().Second(), AdditionalOctaves: 4),
            new(Interval.Perfect().Fourth(), AdditionalOctaves: 1),
            new(Interval.Major().Sixth(), AdditionalOctaves: 2));

        // Ensure underflows are exceptional
        Assert.ThrowsException<OverflowException>(
            () => (Interval)Interval.Perfect().Fourth() - Interval.Perfect().Fifth());
        Assert.ThrowsException<OverflowException>(
            () => Interval.Perfect().Fourth() - new Interval(Interval.Perfect().Unison(), 1));
    }

    private static void TestDifferencePair(Interval lhs, Interval rhs1, Interval rhs2)
    {
        Assert.AreEqual(rhs2, lhs - rhs1);
        Assert.AreEqual(rhs1, lhs - rhs2);
    }
}
