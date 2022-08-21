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
}
