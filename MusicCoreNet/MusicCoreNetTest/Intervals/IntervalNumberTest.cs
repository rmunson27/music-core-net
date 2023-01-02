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
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(SimpleIntervalNumber.Unison, default(IntervalNumber));
    }

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

    /// <summary>
    /// Tests the <see cref="IntervalNumber.Abbreviation"/> property.
    /// </summary>
    [TestMethod]
    public void TestAbbreviation()
    {
        Assert.AreEqual("Unison", ((IntervalNumber)1).Abbreviation);

        Assert.AreEqual("8ve", IntervalNumber.Octave.Abbreviation);

        foreach (var octaves in new[] { 2, 3, 4, 5, 16 })
        {
            Assert.AreEqual($"{octaves} * 8ve", IntervalNumber.Octaves(octaves).Abbreviation);
        }

        foreach (var number in new[] { 11, 111, 12, 112, 13, 213 })
        {
            Assert.AreEqual($"{number}th", new IntervalNumber(number).Abbreviation);
        }

        foreach (var number in new[] { 3, 33, 53, 103 })
        {
            Assert.AreEqual($"{number}rd", ((IntervalNumber)number).Abbreviation);
        }

        foreach (var number in new[] { 2, 32, 42, 102 })
        {
            Assert.AreEqual($"{number}nd", ((IntervalNumber)number).Abbreviation);
        }

        foreach (var number in new[] { 21, 31, 101 })
        {
            Assert.AreEqual($"{number}st", ((IntervalNumber)number).Abbreviation);
        }

        foreach (var number in new[] { 4, 14, 24, 104, 5, 25, 105, 6, 26, 116, 7, 27, 107 })
        {
            Assert.AreEqual($"{number}th", ((IntervalNumber)number).Abbreviation);
        }
    }
}
