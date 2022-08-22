using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    private static readonly ImmutableArray<(NoteClass First, NoteClass Second)> EnharmonicEquivalentPairs
        = ImmutableArray.CreateRange(new[]
        {
            (Notes.A().Natural(), Notes.B().Flat(2)),
            (Notes.A().Natural(), Notes.G().Sharp(2)),
            (Notes.C().Sharp(), Notes.D().Flat()),
            (Notes.G().Natural(), Notes.A().Flat(2)),
        });

    /// <summary>
    /// Tests the <see cref="NoteClass.IsEnharmonicallyEquivalentTo(NoteClass)"/> method.
    /// </summary>
    [TestMethod]
    public void TestEnharmonicEquivalence()
    {
        foreach (var (First, Second) in EnharmonicEquivalentPairs)
        {
            Assert.IsTrue(
                First.IsEnharmonicallyEquivalentTo(First),
                $"{First.ToMusicalNotationString()} was not enharmonically equivalent to itself.");

            Assert.IsTrue(
                Second.IsEnharmonicallyEquivalentTo(Second),
                $"{Second.ToMusicalNotationString()} was not enharmonically equivalent to itself.");

            Assert.IsTrue(
                First.IsEnharmonicallyEquivalentTo(Second),
                $"{First.ToMusicalNotationString()} was not enharmonically equivalent"
                    + $" to {Second.ToMusicalNotationString()}.");

            Assert.IsTrue(
                Second.IsEnharmonicallyEquivalentTo(First),
                $"{Second.ToMusicalNotationString()} was not enharmonically equivalent"
                    + $" to {First.ToMusicalNotationString()}.");
        }
    }

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
