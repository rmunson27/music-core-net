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
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(Note.C().Natural().WithOctave(0), default);
    }

    /// <summary>
    /// Tests the <see cref="Note.SimplifyAccidental"/> method.
    /// </summary>
    [TestMethod]
    public void TestSimplifyAccidental()
    {
        // Normal cases (described by behavior of NoteClass method)
        Assert.AreEqual(Note.A().Flat().WithOctave(4), Note.B().Flat(3).WithOctave(4).SimplifyAccidental());
        Assert.AreEqual(Note.F().WithOctave(4), Note.B().Flat(6).WithOctave(4).SimplifyAccidental());

        // Octave change cases
        Assert.AreEqual(Note.C().Sharp().WithOctave(4), Note.A().Sharp(4).WithOctave(3).SimplifyAccidental());
        Assert.AreEqual(Note.B().Flat().WithOctave(3), Note.E().Flat(6).WithOctave(4).SimplifyAccidental());
        Assert.AreEqual(Note.C().WithOctave(3), Note.C().Sharp(24).WithOctave(1).SimplifyAccidental());
    }

    /// <summary>
    /// A series of tuples containing a pair of notes and an interval difference between the first and the second.
    /// </summary>
    private static readonly ImmutableArray<(Note First, Note Second, Interval Difference)> Differences
        = ImmutableArray.CreateRange(new[]
        {
            (Note.F().Sharp().WithOctave(4), Note.C().Sharp().WithOctave(3),
             Interval.Perfect().Fourth().WithAdditionalOctaves(1)),

            (Note.B().WithOctave(2), Note.F().WithOctave(2), Interval.Augmented().Fourth()),

            (Note.C().WithOctave(4), Note.E().Flat().WithOctave(2),
             Interval.Major().Sixth().WithAdditionalOctaves(1)),

            (Note.B().Flat().WithOctave(5), Note.F().WithOctave(5), Interval.Perfect().Fourth()),

            (Note.C().WithOctave(3), Note.C().WithOctave(2), Interval.PerfectOctave),

            (Note.C().WithOctave(3), Note.C().WithOctave(3), SimpleIntervalBase.PerfectUnison),
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
                $"Invalid {Second.ToASCIIMusicalNotationString()} + {Difference} result.");

            SignedInterval signedDifference = Difference;
            Assert.AreEqual(
                First, Second + signedDifference,
                $"Invalid {Second.ToASCIIMusicalNotationString()} + {signedDifference} result.");
            Assert.AreEqual(
                Second, First + (-signedDifference),
                $"Invalid {First.ToASCIIMusicalNotationString()} + {-signedDifference} result.");
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
                $"Invalid {First.ToASCIIMusicalNotationString()} - {Difference} result.");

            SignedInterval signedDifference = Difference;
            Assert.AreEqual(
                Second, First - signedDifference,
                $"Invalid {First.ToASCIIMusicalNotationString()} - {signedDifference} result.");
            Assert.AreEqual(
                First, Second - (-signedDifference),
                $"Invalid {First.ToASCIIMusicalNotationString()} - {-signedDifference} result.");
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
                $"Unexpected {Left.ToASCIIMusicalNotationString()} - {Right.ToASCIIMusicalNotationString()} result.");
            Assert.AreEqual(
                -(SignedInterval)Difference, Right - Left,
                $"Unexpected {Right.ToASCIIMusicalNotationString()} - {Left.ToASCIIMusicalNotationString()} result.");
        }
    }

    private static readonly ImmutableArray<(Note First, Note Second)> EnharmonicEquivalentPairs
        = ImmutableArray.CreateRange(new[]
        {
            (Note.A().WithOctave(2), Note.B().Flat(2).WithOctave(2)),
            (Note.A().WithOctave(3), Note.G().Sharp(2).WithOctave(3)),
            (Note.C().Sharp().WithOctave(4), Note.D().Flat().WithOctave(4)),
            (Note.G().WithOctave(1), Note.A().Flat(2).WithOctave(1)),

            (Note.C().WithOctave(3), Note.B().Sharp().WithOctave(2)),
            (Note.B().WithOctave(2), Note.C().Flat().WithOctave(3)),
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
                $"{First.ToASCIIMusicalNotationString()} was not enharmonically equivalent to itself.");

            Assert.IsTrue(
                Second.IsEnharmonicallyEquivalentTo(Second),
                $"{Second.ToASCIIMusicalNotationString()} was not enharmonically equivalent to itself.");

            Assert.IsTrue(
                First.IsEnharmonicallyEquivalentTo(Second),
                $"{First.ToASCIIMusicalNotationString()} was not enharmonically equivalent"
                    + $" to {Second.ToASCIIMusicalNotationString()}.");

            Assert.IsTrue(
                Second.IsEnharmonicallyEquivalentTo(First),
                $"{Second.ToASCIIMusicalNotationString()} was not enharmonically equivalent"
                    + $" to {First.ToASCIIMusicalNotationString()}.");
        }
    }

    /// <summary>
    /// Tests the <see cref="Note.Pitch"/> property.
    /// </summary>
    [TestMethod]
    public void TestPitch()
    {
        Assert.AreEqual(new(NotePitchClass.A, 4), Note.A().WithOctave(4).Pitch);
        Assert.AreEqual(new(NotePitchClass.DE, 2), Note.C().Sharp(3).WithOctave(2).Pitch);
        Assert.AreEqual(new(NotePitchClass.B, 3), Note.C().Flat().WithOctave(4).Pitch);
        Assert.AreEqual(new(NotePitchClass.C, 4), Note.B().Sharp().WithOctave(3).Pitch);
    }
}
