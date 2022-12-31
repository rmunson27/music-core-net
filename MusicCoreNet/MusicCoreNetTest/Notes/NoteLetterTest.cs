using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static NoteLetter;

/// <summary>
/// Tests of the <see cref="NoteLetter"/> struct.
/// </summary>
[TestClass]
public class NoteLetterTest
{
    /// <summary>
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(C, default);
    }

    /// <summary>
    /// Tests addition of a <see cref="SimpleInterval"/> to a <see cref="NoteLetter"/>.
    /// </summary>
    [TestMethod]
    public void TestPlus()
    {
        Assert.AreEqual(A, A + SimpleIntervalNumber.Unison);
        Assert.AreEqual(B, A + SimpleIntervalNumber.Second);
        Assert.AreEqual(B, G + SimpleIntervalNumber.Third);
    }

    /// <summary>
    /// Tests subtraction of a <see cref="SimpleInterval"/> from a <see cref="NoteLetter"/>.
    /// </summary>
    [TestMethod]
    public void TestMinus()
    {
        Assert.AreEqual(A, A - SimpleIntervalNumber.Unison);
        Assert.AreEqual(A, B - SimpleIntervalNumber.Second);
        Assert.AreEqual(G, B - SimpleIntervalNumber.Third);
    }

    /// <summary>
    /// Tests the difference between <see cref="NoteLetter"/> instances.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        Assert.AreEqual(Interval.Minor.Seventh, A - B);
        Assert.AreEqual(Interval.Major.Third, E - C);

        // Edge cases where null is returned from internal method since the tritone is ambiguous
        Assert.AreEqual(Interval.Augmented().Fourth, B - F);
        Assert.AreEqual(Interval.Diminished().Fifth, F - B);
    }

    /// <summary>
    /// Tests the <see cref="NoteLetter.ShiftedBy(int)"/> method.
    /// </summary>
    [TestMethod]
    public void TestShiftedBy()
    {
        Assert.AreEqual(A, B.ShiftedBy(-15));
        Assert.AreEqual(A, B.ShiftedBy(-8));
        Assert.AreEqual(B, B.ShiftedBy(-7));
        Assert.AreEqual(G, B.ShiftedBy(-2));
        Assert.AreEqual(A, B.ShiftedBy(-1));

        Assert.AreEqual(B, B.ShiftedBy(0));

        Assert.AreEqual(C, B.ShiftedBy(1));
        Assert.AreEqual(D, B.ShiftedBy(2));
        Assert.AreEqual(B, B.ShiftedBy(7));
        Assert.AreEqual(C, B.ShiftedBy(8));
        Assert.AreEqual(C, B.ShiftedBy(15));
    }
}
