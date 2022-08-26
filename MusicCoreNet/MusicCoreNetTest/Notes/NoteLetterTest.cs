using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static NoteLetter;

/// <summary>
/// Tests of the <see cref="NoteLetter"/> enum static and extension functionality.
/// </summary>
[TestClass]
public class NoteLetterTest
{
    /// <summary>
    /// Tests the <see cref="NoteLetters.Plus(NoteLetter, SimpleIntervalNumber)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestPlus()
    {
        Assert.AreEqual(A, A.Plus(SimpleIntervalNumber.Unison));
        Assert.AreEqual(B, A.Plus(SimpleIntervalNumber.Second));
        Assert.AreEqual(B, G.Plus(SimpleIntervalNumber.Third));
    }

    /// <summary>
    /// Tests the <see cref="NoteLetters.Minus(NoteLetter, SimpleIntervalNumber)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestMinus()
    {
        Assert.AreEqual(A, A.Minus(SimpleIntervalNumber.Unison));
        Assert.AreEqual(A, B.Minus(SimpleIntervalNumber.Second));
        Assert.AreEqual(G, B.Minus(SimpleIntervalNumber.Third));
    }

    /// <summary>
    /// Tests the <see cref="NoteLetters.Minus(NoteLetter, NoteLetter)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        Assert.AreEqual(Intervals.Minor().Seventh(), A.Minus(B));
        Assert.AreEqual(Intervals.Major().Third(), E.Minus(C));

        // Edge cases where null is returned from internal method since the tritone is ambiguous
        Assert.AreEqual(Intervals.Augmented().Fourth(), B.Minus(F));
        Assert.AreEqual(Intervals.Diminished().Fifth(), F.Minus(B));
    }
}
