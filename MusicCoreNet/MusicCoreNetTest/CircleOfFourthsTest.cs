using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="CircleOfFourths"/> class functionality.
/// </summary>
[TestClass]
public class CircleOfFourthsTest
{
    private static readonly ImmutableArray<(NoteSpelling Greater, NoteSpelling Lesser)> NoteSpellingComparisons
        = ImmutableArray.CreateRange(new (NoteSpelling, NoteSpelling)[]
        {
            (Note.A.Natural(), Note.B.Natural()),
            (Note.E.Natural(), Note.C.Sharp()),
            (Note.D.Natural(), Note.D.Sharp()),
            (Note.B.Flat(), Note.F.Natural()),
            (Note.G.Natural(), Note.F.Sharp()),
        });

    /// <summary>
    /// Tests the <see cref="CircleOfFourths.NoteSpellingComparer"/> object functionality.
    /// </summary>
    [TestMethod]
    public void TestNoteSpellingComparer()
    {
        foreach (var (Greater, Lesser) in NoteSpellingComparisons)
        {
            Assert.IsTrue(
                CircleOfFourths.NoteSpellingComparer.Compare(Lesser, Greater) < 0,
                $"Comparer did not uphold {Lesser} < {Greater}.");

            Assert.IsTrue(
                CircleOfFourths.NoteSpellingComparer.Compare(Greater, Lesser) > 0,
                $"Comparer did not uphold {Greater} > {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteSpellingComparer.Compare(Lesser, Lesser) == 0,
                $"Comparer did not uphold {Lesser} == {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteSpellingComparer.Compare(Greater, Greater) == 0,
                $"Comparer did not uphold {Greater} == {Greater}.");
        }
    }

    private static readonly ImmutableArray<(NoteLetter Greater, NoteLetter Lesser)> NoteLetterComparisons
    = ImmutableArray.CreateRange(new[]
    {
            (NoteLetter.A, NoteLetter.B),
            (NoteLetter.C, NoteLetter.B),
            (NoteLetter.F, NoteLetter.G),
            (NoteLetter.F, NoteLetter.C),
            (NoteLetter.D, NoteLetter.E),
    });

    /// <summary>
    /// Tests the <see cref="CircleOfFifths.NoteLetterComparer"/> object functionality.
    /// </summary>
    [TestMethod]
    public void TestNoteLetterComparer()
    {
        foreach (var (Greater, Lesser) in NoteLetterComparisons)
        {
            Assert.IsTrue(
                CircleOfFourths.NoteLetterComparer.Compare(Lesser, Greater) < 0,
                $"Comparer did not uphold {Lesser} < {Greater}.");

            Assert.IsTrue(
                CircleOfFourths.NoteLetterComparer.Compare(Greater, Lesser) > 0,
                $"Comparer did not uphold {Greater} > {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteLetterComparer.Compare(Lesser, Lesser) == 0,
                $"Comparer did not uphold {Lesser} == {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteLetterComparer.Compare(Greater, Greater) == 0,
                $"Comparer did not uphold {Greater} == {Greater}.");
        }
    }
}
