﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Helper functionality relating to the circle of fifths.
/// </summary>
///<seealso cref="CircleOfFifths"/>
public static class CircleOfFourths
{
    /// <summary>
    /// A comparer that can be used to compare <see cref="NoteSpelling"/> instances based on their position in
    /// the circle of fourths.
    /// </summary>
    public static readonly IComparer<NoteSpelling> NoteSpellingComparer = new NoteSpellingComparerType();

    private sealed class NoteSpellingComparerType : IComparer<NoteSpelling>
    {
        /// <inheritdoc/>
        public int Compare(NoteSpelling x, NoteSpelling y)
        {
            return y.CircleOfFifthsIndexRelativeToC.CompareTo(x.CircleOfFifthsIndexRelativeToC);
        }
    }

    /// <summary>
    /// A comparer that can be used to compare <see cref="NoteLetter"/> instances based on the position of natural
    /// note spellings they describe in the circle of fourths.
    /// </summary>
    public static readonly IComparer<NoteLetter> NoteLetterComparer = new NoteLetterComparerType();

    private sealed class NoteLetterComparerType : IComparer<NoteLetter>
    {
        /// <inheritdoc/>
        public int Compare(NoteLetter x, NoteLetter y)
        {
            return y.CircleOfFifthsIndexRelativeToC.CompareTo(x.CircleOfFifthsIndexRelativeToC);
        }
    }
}
