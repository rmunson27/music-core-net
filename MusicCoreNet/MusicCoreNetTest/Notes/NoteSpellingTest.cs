using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="NoteSpelling"/> struct.
/// </summary>
[TestClass]
public class NoteSpellingTest
{
    /// <summary>
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(Note.C().Natural(), default);
    }

    /// <summary>
    /// Tests the <see cref="NoteSpelling.HalfStepsAboveC"/> property.
    /// </summary>
    [TestMethod]
    public void TestHalfStepsAboveC()
    {
        Assert.AreEqual(8, Note.A().Flat().HalfStepsAboveC);
        Assert.AreEqual(6, Note.F().Sharp().HalfStepsAboveC);
        Assert.AreEqual(0, Note.C().Natural().HalfStepsAboveC);

        // Should be in the negative range
        Assert.AreEqual(-1, Note.C().Flat().HalfStepsAboveC);
        Assert.AreEqual(-2, Note.D().Flat(4).HalfStepsAboveC);

        // Should be out of range of the octave
        Assert.AreEqual(12, Note.B().Sharp().HalfStepsAboveC);
        Assert.AreEqual(13, Note.A().Sharp(4).HalfStepsAboveC);
    }

    /// <summary>
    /// Tests the <see cref="NoteSpelling.HalfStepsBelowC"/> property.
    /// </summary>
    [TestMethod]
    public void TestHalfStepsBelowC()
    {
        Assert.AreEqual(4, Note.A().Flat().HalfStepsBelowC);
        Assert.AreEqual(6, Note.F().Sharp().HalfStepsBelowC);
        Assert.AreEqual(0, Note.C().Natural().HalfStepsBelowC);

        // Should be out of range of the octave
        Assert.AreEqual(12, Note.E().Flat(4).HalfStepsBelowC);
        Assert.AreEqual(13, Note.D().Flat(3).HalfStepsBelowC);

        // Should be in the negative range
        Assert.AreEqual(-1, Note.B().Sharp(2).HalfStepsBelowC);
        Assert.AreEqual(-2, Note.A().Sharp(5).HalfStepsBelowC);
    }

    /// <summary>
    /// A list of tuples containing two <see cref="NoteSpelling"/> instances and a <see cref="SimpleIntervalBase"/>
    /// representing the difference between the first and the second.
    /// </summary>
    private static readonly ImmutableArray<(NoteSpelling First, NoteSpelling Second, SimpleIntervalBase Difference)> Differences
        = ImmutableArray.CreateRange(new[]
        {
            (Note.A(), Note.C(), Interval.Major().Sixth()),
            (Note.F().Sharp(), Note.C(), Interval.Augmented().Fourth()),
            (Note.G().Sharp(), Note.B().Flat(), Interval.Augmented().Sixth()),
            (Note.B().Sharp(), Note.D().Flat(), Interval.Augmented(2).Sixth()),
            (Note.E(), Note.G().Sharp(), Interval.Minor().Sixth()),
        });

    /// <summary>
    /// Tests the <see cref="NoteSpelling.operator +(NoteSpelling, SimpleIntervalBase)"/> method.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        foreach (var (First, Second, Difference) in Differences)
        {
            Assert.AreEqual(
                First, Second + Difference,
                $"Invalid {Second.ToMusicalNotationString()} + {Difference} result.");
            Assert.AreEqual(
                Second, First + Difference.Inversion(),
                $"Invalid {First.ToMusicalNotationString()} + {Difference.Inversion()} result.");
        }
    }

    /// <summary>
    /// Tests the <see cref="NoteSpelling.operator -(NoteSpelling, SimpleIntervalBase)"/> method.
    /// </summary>
    [TestMethod]
    public void TestSubtraction()
    {
        foreach (var (First, Second, Difference) in Differences)
        {
            Assert.AreEqual(
                Second, First - Difference,
                $"Invalid {First.ToMusicalNotationString()} - {Difference} result.");
            Assert.AreEqual(
                First, Second - Difference.Inversion(),
                $"Invalid {Second.ToMusicalNotationString()} + {Difference.Inversion()} result.");
        }
    }

    private static readonly ImmutableArray<(NoteSpelling First, NoteSpelling Second)> EnharmonicEquivalentPairs
        = ImmutableArray.CreateRange(new[]
        {
            (Note.A(), Note.B().Flat(2)),
            (Note.A(), Note.G().Sharp(2)),
            (Note.C().Sharp(), Note.D().Flat()),
            (Note.G(), Note.A().Flat(2)),
        });

    /// <summary>
    /// Tests the <see cref="NoteSpelling.IsEnharmonicallyEquivalentTo(NoteSpelling)"/> method.
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
    /// Tests the <see cref="NoteSpelling.PitchClass"/> property.
    /// </summary>
    [TestMethod]
    public void TestPitchClass()
    {
        Assert.AreEqual(NotePitchClass.A, Note.A().Natural().PitchClass);
        Assert.AreEqual(NotePitchClass.A, Note.B().Flat(2).PitchClass);
        Assert.AreEqual(NotePitchClass.G, Note.A().Flat(2).PitchClass);
    }

    /// <summary>
    /// Tests the <see cref="NoteSpelling.SimplifyAccidental"/> method.
    /// </summary>
    [TestMethod]
    public void TestSimplifyAccidental()
    {
        Assert.AreEqual(NoteLetter.A, Note.B().Flat(2).SimplifyAccidental());
        Assert.AreEqual(NoteLetter.B, Note.C().Flat().SimplifyAccidental());
        Assert.AreEqual(Note.A().Sharp(), Note.G().Sharp(3).SimplifyAccidental());
        Assert.AreEqual(Note.B().Flat(), Note.C().Flat(2).SimplifyAccidental());
    }

    /// <summary>
    /// Tests the <see cref="NoteSpelling.SimplestWithPitchClass(NotePitchClass, NonNaturalAccidentalType)"/>
    /// factory method.
    /// </summary>
    [TestMethod]
    public void TestSimplestWithPitchClass()
    {
        // Non-ambiguous tests
        TestSimplestWithPitchClassPair(NotePitchClass.C, NoteLetter.C);
        TestSimplestWithPitchClassPair(NotePitchClass.D, NoteLetter.D);
        TestSimplestWithPitchClassPair(NotePitchClass.E, NoteLetter.E);
        TestSimplestWithPitchClassPair(NotePitchClass.F, NoteLetter.F);
        TestSimplestWithPitchClassPair(NotePitchClass.G, NoteLetter.G);
        TestSimplestWithPitchClassPair(NotePitchClass.A, NoteLetter.A);
        TestSimplestWithPitchClassPair(NotePitchClass.B, NoteLetter.B);

        // Ambiguous tests
        TestAmbiguousSimplestWithPitchClassPair(NotePitchClass.CD, NoteLetter.C, NoteLetter.D);
        TestAmbiguousSimplestWithPitchClassPair(NotePitchClass.DE, NoteLetter.D, NoteLetter.E);
        TestAmbiguousSimplestWithPitchClassPair(NotePitchClass.FG, NoteLetter.F, NoteLetter.G);
        TestAmbiguousSimplestWithPitchClassPair(NotePitchClass.GA, NoteLetter.G, NoteLetter.A);
        TestAmbiguousSimplestWithPitchClassPair(NotePitchClass.AB, NoteLetter.A, NoteLetter.B);
    }

    private static void TestAmbiguousSimplestWithPitchClassPair(
        NotePitchClass pitchClass, NoteLetter sharpLetter, NoteLetter flatLetter)
    {
        TestSimplestWithPitchClassPair(
            pitchClass, new(sharpLetter, Accidental.Sharp()), NonNaturalAccidentalType.Sharp);
        TestSimplestWithPitchClassPair(
            pitchClass, new(flatLetter, Accidental.Flat()), NonNaturalAccidentalType.Flat);
    }

    private static void TestSimplestWithPitchClassPair(
        NotePitchClass pitchClass, NoteSpelling expectedNote,
        NonNaturalAccidentalType? nonNaturalAccidentalType = null)
    {
        Assert.AreEqual(
            expectedNote,
            NoteSpelling.SimplestWithPitchClass(pitchClass, nonNaturalAccidentalType ?? NonNaturalAccidentalType.Flat),
            $"Invalid {nameof(NoteSpelling.SimplestWithPitchClass)}"
                + $"({pitchClass}, {(nonNaturalAccidentalType?.ToString() ?? "_")}) result.");
        Assert.AreEqual(
            pitchClass, expectedNote.PitchClass,
            $"Note '{expectedNote.ToMusicalNotationString()}' did not have the expected pitch class.");
    }

    /// <summary>
    /// Tests of the <see cref="NoteSpelling.operator -(NoteSpelling, NoteSpelling)"/> method.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        foreach (var (First, Second, Difference) in Differences)
        {
            Assert.AreEqual(
                Difference, First - Second,
                $"Invalid {First.ToMusicalNotationString()} - {Second.ToMusicalNotationString()} result.");
            Assert.AreEqual(
                Difference.Inversion(),
                Second - First,
                $"Invalid {Second.ToMusicalNotationString()} - {First.ToMusicalNotationString()} result.");
        }
    }
}
