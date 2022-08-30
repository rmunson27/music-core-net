using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static NotePitchClass;

/// <summary>
/// Tests of the <see cref="NotePitchClass"/> struct.
/// </summary>
[TestClass]
public class NotePitchClassTest
{
    /// <summary>
    /// Ensures the default value is as advertised in doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        Assert.AreEqual(C, default);
    }

    private static readonly ImmutableArray<CBasedSemitones> CBasedSemitoneCases
        = ImmutableArray.CreateRange(new CBasedSemitones[]
        {
            new(C, 0, 0),
            new(CD, 1, 11),
            new(D, 2, 10),
            new(DE, 3, 9),
            new(E, 4, 8),
            new(F, 5, 7),
            new(FG, 6, 6),
            new(G, 7, 5),
            new(GA, 8, 4),
            new(A, 9, 3),
            new(AB, 10, 2),
            new(B, 11, 1),
        });

    /// <summary>
    /// Tests the <see cref="SemitonesAboveC"/> property.
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
    /// Tests the <see cref="SemitonesBelowC"/> property.
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
    /// Tests the <see cref="FromSemitonesAboveC(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromSemitonesAboveC()
    {
        foreach (var (PitchClass, AboveC, _) in CBasedSemitoneCases)
        {
            Assert.AreEqual(PitchClass, FromSemitonesAboveC(AboveC), $"Invalid {AboveC} result.");
        }

        // Ensure that exceptions are thrown if out-of-range
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesAboveC(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesAboveC(12));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesAboveC(13));
    }

    /// <summary>
    /// Tests the <see cref="FromSemitonesBelowC(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromSemitonesBelowC()
    {
        foreach (var (PitchClass, _, BelowC) in CBasedSemitoneCases)
        {
            Assert.AreEqual(PitchClass, FromSemitonesBelowC(BelowC), $"Invalid {BelowC} result.");
        }

        // Ensure that exceptions are thrown if out-of-range
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesBelowC(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesBelowC(12));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => FromSemitonesBelowC(13));
    }

    private readonly record struct CBasedSemitones(
        NotePitchClass PitchClass, [NonNegative] int AboveC, [NonNegative] int BelowC);
}
