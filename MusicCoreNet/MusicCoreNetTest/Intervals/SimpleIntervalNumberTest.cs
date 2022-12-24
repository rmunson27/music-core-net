using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static PerfectableSimpleIntervalNumber;
using static ImperfectableSimpleIntervalNumber;

/// <summary>
/// Tests of the simple interval number types included in the library.
/// </summary>
[TestClass]
public class SimpleIntervalNumberTest
{
    /// <summary>
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(SimpleIntervalNumber.Unison, default);
    }

    /// <summary>
    /// Tests the <see cref="SimpleIntervalNumber.FromNumericalValue(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromValue()
    {
        Assert.AreEqual(Unison, SimpleIntervalNumber.FromNumericalValue(1));
        Assert.AreEqual(Second, SimpleIntervalNumber.FromNumericalValue(2));
        Assert.AreEqual(Third, SimpleIntervalNumber.FromNumericalValue(3));
        Assert.AreEqual(Fourth, SimpleIntervalNumber.FromNumericalValue(4));
        Assert.AreEqual(Fifth, SimpleIntervalNumber.FromNumericalValue(5));
        Assert.AreEqual(Sixth, SimpleIntervalNumber.FromNumericalValue(6));
        Assert.AreEqual(Seventh, SimpleIntervalNumber.FromNumericalValue(7));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => SimpleIntervalNumber.FromNumericalValue(8));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => SimpleIntervalNumber.FromNumericalValue(0));
    }

    /// <summary>
    /// Tests casting from an <see cref="int"/> to a <see cref="SimpleIntervalNumber"/>.
    /// </summary>
    [TestMethod]
    public void TestIntCast()
    {
        Assert.AreEqual(Third, (SimpleIntervalNumber)3);
        Assert.AreEqual(Fifth, (SimpleIntervalNumber)5);
        Assert.ThrowsException<InvalidCastException>(() => (SimpleIntervalNumber)8);
        Assert.ThrowsException<InvalidCastException>(() => (SimpleIntervalNumber)0);
    }
}
