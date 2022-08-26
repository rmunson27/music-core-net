using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="CircleOfFifths"/> class functionality.
/// </summary>
[TestClass]
public class CircleOfFifthsTest
{
    private static readonly ImmutableArray<(NoteSpelling Lesser, NoteSpelling Greater)> NoteSpellingComparisons
        = ImmutableArray.CreateRange(new (NoteSpelling, NoteSpelling)[]
        {
            (Note.A().Natural(), Note.B().Natural()),
            (Note.E().Natural(), Note.C().Sharp()),
            (Note.D().Natural(), Note.D().Sharp()),
            (Note.B().Flat(), Note.F().Natural()),
            (Note.G().Natural(), Note.F().Sharp()),
        });

    /// <summary>
    /// Tests the <see cref="CircleOfFifths.NoteSpellingComparer"/> object functionality.
    /// </summary>
    [TestMethod]
    public void TestNoteSpellingComparer()
    {
        foreach (var (Lesser, Greater) in NoteSpellingComparisons)
        {
            Assert.IsTrue(
                CircleOfFifths.NoteSpellingComparer.Compare(Lesser, Greater) < 0,
                $"Comparer did not uphold {Lesser} < {Greater}.");

            Assert.IsTrue(
                CircleOfFifths.NoteSpellingComparer.Compare(Greater, Lesser) > 0,
                $"Comparer did not uphold {Greater} > {Lesser}.");

            Assert.IsTrue(
                CircleOfFifths.NoteSpellingComparer.Compare(Lesser, Lesser) == 0,
                $"Comparer did not uphold {Lesser} == {Lesser}.");

            Assert.IsTrue(
                CircleOfFifths.NoteSpellingComparer.Compare(Greater, Greater) == 0,
                $"Comparer did not uphold {Greater} == {Greater}.");
        }
    }
}
