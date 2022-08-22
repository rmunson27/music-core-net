﻿using Rem.Music.MIDI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music.MIDI;

/// <summary>
/// Tests of the <see cref="MIDINotes"/> class.
/// </summary>
[TestClass]
public class MIDINotesTest
{
    private static readonly ImmutableArray<(NotePitchInfo Pitch, int Number)> MIDINumberPairs
        = ImmutableArray.CreateRange(new[]
        {
            // Random tests
            (NotePitchClass.E.WithOctave(4), 64),
            (NotePitchClass.D.WithOctave(3), 50),
            (NotePitchClass.AB.WithOctave(2), 46),
            (NotePitchClass.FG.WithOctave(7), 102),

            // Extrema and important cases
            (MIDINotes.MaxPitch, MIDINotes.MaxNumber),
            (NotePitchInfo.ConcertPitch, 69),
            (NotePitchInfo.C0, 12),
            (MIDINotes.MinPitch, MIDINotes.MinNumber),

            // Out-of-range cases
            (NotePitchClass.C.WithOctave(10), 132),
            (NotePitchClass.D.WithOctave(-2), -10),
        });

    /// <summary>
    /// Tests the <see cref="MIDINotes.MIDINumber(NotePitchInfo)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestMIDINumber()
    {
        foreach (var (Pitch, Number) in MIDINumberPairs)
        {
            Assert.AreEqual(Number, Pitch.MIDINumber(), $"Invalid {Pitch} MIDI number.");
        }
    }

    /// <summary>
    /// Tests the <see cref="MIDINotes.MIDINumberInRange(NotePitchInfo)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestMIDINumberInRange()
    {
        foreach (var (Pitch, Number) in MIDINumberPairs)
        {
            if (MIDINotes.IsNumberInRange(Number))
            {
                Assert.AreEqual(Number, Pitch.MIDINumberInRange(), $"Invalid {Pitch} MIDI number.");
            }
            else
            {
                Assert.ThrowsException<InvalidOperationException>(
                    () => Pitch.MIDINumberInRange(),
                    $"Pitch {Pitch} with expected out-of-range MIDI number {Number} did not throw.");
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="MIDINotes.TryGetMIDINumberInRange(NotePitchInfo, out int)"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestTryGetMIDINumberInRange()
    {
        foreach (var (Pitch, Number) in MIDINumberPairs)
        {
            var result = Pitch.TryGetMIDINumberInRange(out var actualNumber);
            Assert.AreEqual(Number, actualNumber);
            Assert.AreEqual(result, MIDINotes.IsNumberInRange(Number));
        }
    }
}
