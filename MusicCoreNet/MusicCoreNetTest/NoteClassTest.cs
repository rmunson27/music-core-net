using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="NoteClass"/> struct.
/// </summary>
[TestClass]
public class NoteClassTest
{
    /// <summary>
    /// Tests of the <see cref="NoteClass.operator -(NoteClass, NoteClass)"/> method.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        TestDifferencePair(Notes.A().Natural(), Notes.C().Natural(), Intervals.Major().Sixth());
        TestDifferencePair(Notes.F().Sharp(), Notes.C().Natural(), Intervals.Augmented().Fourth());
        TestDifferencePair(Notes.G().Sharp(), Notes.B().Flat(), Intervals.Augmented().Sixth());
        TestDifferencePair(Notes.B().Sharp(), Notes.D().Flat(), Intervals.Augmented(2).Sixth());
        TestDifferencePair(Notes.E().Natural(), Notes.G().Sharp(), Intervals.Minor().Sixth());
    }
    
    private static void TestDifferencePair(NoteClass lhs, NoteClass rhs, SimpleIntervalBase expectedResult)
    {
        Assert.AreEqual(expectedResult, lhs - rhs);
        Assert.AreEqual(expectedResult.Inversion(), rhs - lhs);
    }
}
