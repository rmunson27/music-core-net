using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="Note"/> struct.
/// </summary>
[TestClass]
public class NoteTest
{
    /// <summary>
    /// Tests the <see cref="Note.SimplifyAccidental"/> method.
    /// </summary>
    [TestMethod]
    public void TestSimplifyAccidental()
    {
        // Normal cases (described by behavior of NoteClass method)
        Assert.AreEqual(Notes.A().Flat().WithOctave(4), Notes.B().Flat(3).WithOctave(4).SimplifyAccidental());
        Assert.AreEqual(Notes.F().Natural().WithOctave(4), Notes.B().Flat(6).WithOctave(4).SimplifyAccidental());

        // Octave change cases
        Assert.AreEqual(Notes.C().Sharp().WithOctave(4), Notes.A().Sharp(4).WithOctave(3).SimplifyAccidental());
        Assert.AreEqual(Notes.B().Flat().WithOctave(3), Notes.E().Flat(6).WithOctave(4).SimplifyAccidental());
        Assert.AreEqual(Notes.C().Natural().WithOctave(3), Notes.C().Sharp(24).WithOctave(1).SimplifyAccidental());
    }

    /// <summary>
    /// A series of tuples containing a pair of notes and an interval difference between the first and the second.
    /// </summary>
    private static readonly ImmutableArray<(Note First, Note Second, Interval Difference)> Differences
        = ImmutableArray.CreateRange(new[]
        {
            (Notes.F().Sharp().WithOctave(4), Notes.C().Sharp().WithOctave(3),
             Intervals.Perfect().Fourth().WithAdditionalOctaves(1)),

            (Notes.B().Natural().WithOctave(2), Notes.F().Natural().WithOctave(2),
             Intervals.Augmented().Fourth()),

            (Notes.C().Natural().WithOctave(4), Notes.E().Flat().WithOctave(2),
             Intervals.Major().Sixth().WithAdditionalOctaves(1)),

            (Notes.B().Flat().WithOctave(5), Notes.F().Natural().WithOctave(5),
             Intervals.Perfect().Fourth()),

            (Notes.C().Natural().WithOctave(3), Notes.C().Natural().WithOctave(2),
             Interval.PerfectOctave),

            (Notes.C().Natural().WithOctave(3), Notes.C().Natural().WithOctave(3),
             SimpleIntervalBase.PerfectUnison),
        });

    /// <summary>
    /// Tests the <see cref="Note"/> interval addition operators.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        foreach (var (First, Second, Difference) in Differences)
        {
            Assert.AreEqual(
                First, Second + Difference,
                $"Invalid {Second.ToMusicalNotationString()} + {Difference} result.");

            SignedInterval signedDifference = Difference;
            Assert.AreEqual(
                First, Second + signedDifference,
                $"Invalid {Second.ToMusicalNotationString()} + {signedDifference} result.");
            Assert.AreEqual(
                Second, First + (-signedDifference),
                $"Invalid {First.ToMusicalNotationString()} + {-signedDifference} result.");
        }
    }

    /// <summary>
    /// Tests the <see cref="Note"/> interval subtraction operators.
    /// </summary>
    [TestMethod]
    public void TestSubtraction()
    {
        foreach (var (First, Second, Difference) in Differences)
        {
            Assert.AreEqual(
                Second, First - Difference,
                $"Invalid {First.ToMusicalNotationString()} - {Difference} result.");

            SignedInterval signedDifference = Difference;
            Assert.AreEqual(
                Second, First - signedDifference,
                $"Invalid {First.ToMusicalNotationString()} - {signedDifference} result.");
            Assert.AreEqual(
                First, Second - (-signedDifference),
                $"Invalid {First.ToMusicalNotationString()} - {-signedDifference} result.");
        }
    }

    /// <summary>
    /// Tests of the <see cref="Note"/> difference operator.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        foreach (var (Left, Right, Difference) in Differences)
        {
            Assert.AreEqual(
                Difference, Left - Right,
                $"Unexpected {Left.ToMusicalNotationString()} - {Right.ToMusicalNotationString()} result.");
            Assert.AreEqual(
                -(SignedInterval)Difference, Right - Left,
                $"Unexpected {Right.ToMusicalNotationString()} - {Left.ToMusicalNotationString()} result.");
        }
    }

    private static readonly ImmutableArray<(Note First, Note Second)> EnharmonicEquivalentPairs
        = ImmutableArray.CreateRange(new[]
        {
            (Notes.A().Natural().WithOctave(2), Notes.B().Flat(2).WithOctave(2)),
            (Notes.A().Natural().WithOctave(3), Notes.G().Sharp(2).WithOctave(3)),
            (Notes.C().Sharp().WithOctave(4), Notes.D().Flat().WithOctave(4)),
            (Notes.G().Natural().WithOctave(1), Notes.A().Flat(2).WithOctave(1)),

            (Notes.C().Natural().WithOctave(3), Notes.B().Sharp().WithOctave(2)),
            (Notes.B().Natural().WithOctave(2), Notes.C().Flat().WithOctave(3)),
        });

    /// <summary>
    /// Tests the <see cref="Note.IsEnharmonicallyEquivalentTo(Note)"/> method.
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
    /// Tests the <see cref="Note.Pitch"/> property.
    /// </summary>
    [TestMethod]
    public void TestPitch()
    {
        Assert.AreEqual(new(NotePitchClass.A, 4), Notes.A().Natural().WithOctave(4).Pitch);
        Assert.AreEqual(new(NotePitchClass.DE, 2), Notes.C().Sharp(3).WithOctave(2).Pitch);
        Assert.AreEqual(new(NotePitchClass.B, 3), Notes.C().Flat().WithOctave(4).Pitch);
        Assert.AreEqual(new(NotePitchClass.C, 4), Notes.B().Sharp().WithOctave(3).Pitch);
    }
}
