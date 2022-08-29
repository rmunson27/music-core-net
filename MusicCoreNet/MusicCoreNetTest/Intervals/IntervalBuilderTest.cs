using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests various interval builder types.
/// </summary>
[TestClass]
public class IntervalBuilderTest
{
    /// <summary>
    /// Tests methods for constructing an interval from a predefined builder and a supplied interval number.
    /// </summary>
    [TestMethod]
    public void TestWithNumber()
    {
        // All should pass
        // The augmented and diminished should work no matter what the perfectability of the number passed in is, as
        // mismatches between perfectabilities is not possible
        Assert.AreEqual(Interval.Augmented().Third(), Interval.Augmented().WithNumber(3));
        Assert.AreEqual(Interval.Augmented().Fourth(), Interval.Augmented().WithNumber(4));
        Assert.AreEqual(Interval.Major().Third(), Interval.Major().WithNumber(3));
        Assert.AreEqual(Interval.Minor().Third(), Interval.Minor().WithNumber(3));
        Assert.AreEqual(Interval.Perfect().Unison(), Interval.Perfect().WithNumber(1));
        Assert.AreEqual(Interval.Diminished().Third(), Interval.Diminished().WithNumber(3));
        Assert.AreEqual(Interval.Diminished().Fourth(), Interval.Diminished().WithNumber(4));

        // These should fail due to mismatches
        Assert.ThrowsException<ArgumentException>(() => Interval.Major().WithNumber(4));
        Assert.ThrowsException<ArgumentException>(() => Interval.Perfect().WithNumber(3));
        Assert.ThrowsException<ArgumentException>(() => Interval.Minor().WithNumber(4));

        // These should fail due to the number being 0 or negative
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Interval.Major().WithNumber(0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Interval.Major().WithNumber(-1));
    }
}
