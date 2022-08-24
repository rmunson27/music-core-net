using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static PerfectableSimpleIntervalNumber;
using static NonPerfectableSimpleIntervalNumber;

/// <summary>
/// Tests of the simple interval number types included in the library.
/// </summary>
[TestClass]
public class SimpleIntervalNumberTest
{
    /// <summary>
    /// Tests the <see cref="SimpleIntervalNumber.FromValue(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromValue()
    {
        Assert.AreEqual(Unison, SimpleIntervalNumber.FromValue(1));
        Assert.AreEqual(Second, SimpleIntervalNumber.FromValue(2));
        Assert.AreEqual(Third, SimpleIntervalNumber.FromValue(3));
        Assert.AreEqual(Fourth, SimpleIntervalNumber.FromValue(4));
        Assert.AreEqual(Fifth, SimpleIntervalNumber.FromValue(5));
        Assert.AreEqual(Sixth, SimpleIntervalNumber.FromValue(6));
        Assert.AreEqual(Seventh, SimpleIntervalNumber.FromValue(7));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => SimpleIntervalNumber.FromValue(8));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => SimpleIntervalNumber.FromValue(0));
    }

    /// <summary>
    /// Ensures that the default instance of <see cref="SimpleIntervalNumber"/> is as advertised in the documentation.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        // First make sure they compare as equal
        Assert.AreEqual(SimpleIntervalNumber.Unison, default(SimpleIntervalNumber));

        // Second ensure that the default actually stores the expected values
        Assert.IsTrue(default(SimpleIntervalNumber).IsPerfectable(out var defaultPNumber));
        Assert.AreEqual(SimpleIntervalNumber.Unison, defaultPNumber);
    }
}
