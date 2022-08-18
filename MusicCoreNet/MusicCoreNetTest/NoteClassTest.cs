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
        TestDifferencePair(NoteClass.A().Natural(), NoteClass.C().Natural(), Intervals.Major().Sixth());
        TestDifferencePair(NoteClass.F().Sharp(), NoteClass.C().Natural(), Intervals.Augmented().Fourth());
        TestDifferencePair(NoteClass.G().Sharp(), NoteClass.B().Flat(), Intervals.Augmented().Sixth());
        TestDifferencePair(NoteClass.B().Sharp(), NoteClass.D().Flat(), Intervals.Augmented(2).Sixth());
        TestDifferencePair(NoteClass.E().Natural(), NoteClass.G().Sharp(), Intervals.Minor().Sixth());
    }
    
    private static void TestDifferencePair(NoteClass lhs, NoteClass rhs, SimpleIntervalBase expectedResult)
    {
        Assert.AreEqual(expectedResult, lhs - rhs);
        Assert.AreEqual(expectedResult.Inversion(), rhs - lhs);
    }
}
