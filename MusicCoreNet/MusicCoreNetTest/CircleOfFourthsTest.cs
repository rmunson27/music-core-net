﻿using System;
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
    private static readonly ImmutableArray<(NoteClass Greater, NoteClass Lesser)> NoteClassComparisons
        = ImmutableArray.CreateRange(new (NoteClass, NoteClass)[]
        {
            (Notes.A().Natural(), Notes.B().Natural()),
            (Notes.E().Natural(), Notes.C().Sharp()),
            (Notes.D().Natural(), Notes.D().Sharp()),
            (Notes.B().Flat(), Notes.F().Natural()),
            (Notes.G().Natural(), Notes.F().Sharp()),
        });

    /// <summary>
    /// Tests the <see cref="CircleOfFourths.NoteClassComparer"/> object functionality.
    /// </summary>
    [TestMethod]
    public void TestNoteClassComparer()
    {
        foreach (var (Greater, Lesser) in NoteClassComparisons)
        {
            Assert.IsTrue(
                CircleOfFourths.NoteClassComparer.Compare(Lesser, Greater) < 0,
                $"Comparer did not uphold {Lesser} < {Greater}.");

            Assert.IsTrue(
                CircleOfFourths.NoteClassComparer.Compare(Greater, Lesser) > 0,
                $"Comparer did not uphold {Greater} > {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteClassComparer.Compare(Lesser, Lesser) == 0,
                $"Comparer did not uphold {Lesser} == {Lesser}.");

            Assert.IsTrue(
                CircleOfFourths.NoteClassComparer.Compare(Greater, Greater) == 0,
                $"Comparer did not uphold {Greater} == {Greater}.");
        }
    }
}
