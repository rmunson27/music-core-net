using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="NotePitch"/> struct.
/// </summary>
[TestClass]
public class NotePitchTest
{
    private static readonly ImmutableArray<(NotePitch Pitch, int Index)> C0IndexPairs
        = ImmutableArray.CreateRange(new[]
        {
            (NotePitch.C0, 0),
            (NotePitchClass.A.WithOctave(4), 57),
            (NotePitchClass.GA.WithOctave(9), 116),
            (NotePitchClass.B.WithOctave(6), 83),
        });

    /// <summary>
    /// Tests the <see cref="NotePitch.FromC0Index(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromC0Index()
    {
        foreach (var (Pitch, Index) in C0IndexPairs)
        {
            Assert.AreEqual(
                Pitch, NotePitch.FromC0Index(Index),
                $"Invalid {nameof(NotePitch.FromC0Index)} result for index {Index}.");
        }
    }

    /// <summary>
    /// Tests the <see cref="NotePitch.C0Index"/> property.
    /// </summary>
    [TestMethod]
    public void TestC0Index()
    {
        foreach (var (Pitch, Index) in C0IndexPairs)
        {
            Assert.AreEqual(Index, Pitch.C0Index, $"Invalid {Pitch} C0 index.");
        }
    }

    /// <summary>
    /// Tests the half-step addition operator provided by the <see cref="NotePitch"/> struct.
    /// </summary>
    [TestMethod]
    public void TestAddition()
    {
        Assert.AreEqual(NotePitchClass.B.WithOctave(4), NotePitch.ConcertPitch + 2);
        Assert.AreEqual(NotePitchClass.DE.WithOctave(6), NotePitchClass.C.WithOctave(4) + 27);
    }

    /// <summary>
    /// Tests the <see cref="NotePitch.Frequency"/> method.
    /// </summary>
    [TestMethod]
    public void TestFrequency()
    {
        // Numbers obtained for the pitch frequencies only have 2 significant digits
        const double delta = 0.01;

        // Test all possible note pitch classes with various random octaves
        Assert.AreEqual(220.0, NotePitchClass.A.WithOctave(3).Frequency); // Should be exact
        Assert.AreEqual(3729.31, NotePitchClass.AB.WithOctave(7).Frequency, delta);
        Assert.AreEqual(1975.53, NotePitchClass.B.WithOctave(6).Frequency, delta);
        Assert.AreEqual(130.81, NotePitchClass.C.WithOctave(3).Frequency, delta);
        Assert.AreEqual(138.59, NotePitchClass.CD.WithOctave(3).Frequency, delta);
        Assert.AreEqual(293.66, NotePitchClass.D.WithOctave(4).Frequency, delta);
        Assert.AreEqual(77.78, NotePitchClass.DE.WithOctave(2).Frequency, delta);
        Assert.AreEqual(659.26, NotePitchClass.E.WithOctave(5).Frequency, delta);
        Assert.AreEqual(349.23, NotePitchClass.F.WithOctave(4).Frequency, delta);
        Assert.AreEqual(46.25, NotePitchClass.FG.WithOctave(1).Frequency, delta);
        Assert.AreEqual(24.50, NotePitchClass.G.WithOctave(0).Frequency, delta);
        Assert.AreEqual(207.65, NotePitchClass.GA.WithOctave(3).Frequency, delta);

        Assert.AreEqual(440.0, NotePitch.ConcertPitch.Frequency); // Should be exact

        // Test extreme values
        Assert.AreEqual(8.18, NotePitchClass.C.WithOctave(-1).Frequency, delta);
        Assert.AreEqual(13289.75, NotePitchClass.GA.WithOctave(9).Frequency, delta);
    }

    /// <summary>
    /// Tests the difference subtraction operator provided by the <see cref="NotePitch"/> struct.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        Assert.AreEqual(0, NotePitch.ConcertPitch - NotePitch.ConcertPitch);
        Assert.AreEqual(10, NotePitch.ConcertPitch - NotePitchClass.B.WithOctave(3));
        Assert.AreEqual(10, NotePitchClass.C.WithOctave(2) - NotePitchClass.D.WithOctave(1));
        Assert.AreEqual(-16, NotePitchClass.DE.WithOctave(4) - NotePitchClass.G.WithOctave(5));
        Assert.AreEqual(-28, NotePitchClass.F.WithOctave(4) - NotePitchClass.A.WithOctave(6));
    }
}
