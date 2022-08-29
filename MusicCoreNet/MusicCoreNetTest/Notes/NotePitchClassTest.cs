using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="NotePitchClass"/> struct.
/// </summary>
[TestClass]
public class NotePitchClassTest
{
    private static readonly ImmutableArray<CBasedSemitones> CBasedSemitoneCases
        = ImmutableArray.CreateRange(new CBasedSemitones[]
        {
            new(NotePitchClass.C, 0, 0),
            new(NotePitchClass.CD, 1, 11),
            new(NotePitchClass.D, 2, 10),
            new(NotePitchClass.DE, 3, 9),
            new(NotePitchClass.E, 4, 8),
            new(NotePitchClass.F, 5, 7),
            new(NotePitchClass.FG, 6, 6),
            new(NotePitchClass.G, 7, 5),
            new(NotePitchClass.GA, 8, 4),
            new(NotePitchClass.A, 9, 3),
            new(NotePitchClass.AB, 10, 2),
            new(NotePitchClass.B, 11, 1),
        });

    /// <summary>
    /// Tests the <see cref="NotePitchClass.SemitonesAboveC"/> property.
    /// </summary>
    [TestMethod]
    public void TestSemitonesAboveC()
    {
        foreach (var (PitchClass, AboveC, _) in CBasedSemitoneCases)
        {
            Assert.AreEqual(AboveC, PitchClass.SemitonesAboveC, $"Invalid {PitchClass} result.");
        }
    }

    /// <summary>
    /// Tests the <see cref="NotePitchClass.SemitonesBelowC"/> property.
    /// </summary>
    [TestMethod]
    public void TestSemitonesBelowC()
    {
        foreach (var (PitchClass, _, BelowC) in CBasedSemitoneCases)
        {
            Assert.AreEqual(BelowC, PitchClass.SemitonesBelowC, $"Invalid {PitchClass} result.");
        }
    }

    /// <summary>
    /// Tests the <see cref="NotePitchClass.FromSemitonesAboveC(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromSemitonesAboveC()
    {
        foreach (var (PitchClass, AboveC, _) in CBasedSemitoneCases)
        {
            Assert.AreEqual(PitchClass, NotePitchClass.FromSemitonesAboveC(AboveC), $"Invalid {AboveC} result.");
        }

        // Ensure that exceptions are thrown if out-of-range
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesAboveC(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesAboveC(12));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesAboveC(13));
    }

    /// <summary>
    /// Tests the <see cref="NotePitchClass.FromSemitonesBelowC(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromSemitonesBelowC()
    {
        foreach (var (PitchClass, _, BelowC) in CBasedSemitoneCases)
        {
            Assert.AreEqual(PitchClass, NotePitchClass.FromSemitonesBelowC(BelowC), $"Invalid {BelowC} result.");
        }

        // Ensure that exceptions are thrown if out-of-range
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesBelowC(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesBelowC(12));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => NotePitchClass.FromSemitonesBelowC(13));
    }

    private readonly record struct CBasedSemitones(
        NotePitchClass PitchClass, [NonNegative] int AboveC, [NonNegative] int BelowC);
}
