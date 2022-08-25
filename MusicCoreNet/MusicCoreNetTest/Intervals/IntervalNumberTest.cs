using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="IntervalNumber"/> struct.
/// </summary>
[TestClass]
public class IntervalNumberTest
{
    /// <summary>
    /// Tests construction of <see cref="IntervalNumber"/> instances.
    /// </summary>
    [TestMethod]
    public void TestConstruction()
    {
        Assert.AreEqual(new IntervalNumber(SimpleIntervalNumber.Seventh), new(7));
        Assert.AreEqual(new IntervalNumber(SimpleIntervalNumber.Unison, AdditionalOctaves: 1), new(8));
        Assert.AreEqual(new IntervalNumber(SimpleIntervalNumber.Third, AdditionalOctaves: 2), new(17));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IntervalNumber(SimpleIntervalNumber.Sixth, -1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IntervalNumber(0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IntervalNumber(-1));
    }
}
