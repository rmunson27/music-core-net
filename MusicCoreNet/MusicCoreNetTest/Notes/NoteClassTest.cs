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
    /// <summary>
    /// A list of tuples containing two <see cref="NoteClass"/> instances and a <see cref="SimpleIntervalBase"/>
    /// representing the difference between the first and the second.
    /// </summary>
    private static readonly ImmutableArray<(NoteClass First, NoteClass Second, SimpleIntervalBase Difference)> Differences
        = ImmutableArray.CreateRange(new[]
        {
            (Notes.A().Natural(), Notes.C().Natural(), Intervals.Major().Sixth()),
            (Notes.F().Sharp(), Notes.C().Natural(), Intervals.Augmented().Fourth()),
            (Notes.G().Sharp(), Notes.B().Flat(), Intervals.Augmented().Sixth()),
            (Notes.B().Sharp(), Notes.D().Flat(), Intervals.Augmented(2).Sixth()),
            (Notes.E().Natural(), Notes.G().Sharp(), Intervals.Minor().Sixth()),
        });

    /// <summary>
    /// Tests the <see cref="NoteClass.operator +(NoteClass, SimpleIntervalBase)"/> method.
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
    /// Tests the <see cref="NoteClass.operator -(NoteClass, SimpleIntervalBase)"/> method.
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
    /// Tests the <see cref="NoteClass.PitchClass"/> property.
    /// </summary>
    [TestMethod]
    public void TestPitchClass()
    {
        Assert.AreEqual(NotePitchClass.A, Notes.A().Natural().PitchClass);
        Assert.AreEqual(NotePitchClass.A, Notes.B().Flat(2).PitchClass);
        Assert.AreEqual(NotePitchClass.G, Notes.A().Flat(2).PitchClass);
    }

    /// <summary>
    /// Tests of the <see cref="NoteClass.operator -(NoteClass, NoteClass)"/> method.
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
