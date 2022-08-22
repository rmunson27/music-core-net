﻿using System;
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
    private static readonly ImmutableArray<(Note Left, Note Right, Interval Result)> DifferenceTests
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
    /// Tests of the <see cref="Note"/> subtraction operator.
    /// </summary>
    [TestMethod]
    public void TestDifference()
    {
        foreach (var (Left, Right, Result) in DifferenceTests)
        {
            Assert.AreEqual(
                Result, Left - Right,
                $"Unexpected {Left.ToMusicalNotationString()} - {Right.ToMusicalNotationString()} result.");
            Assert.AreEqual(
                -(SignedInterval)Result, Right - Left,
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
}