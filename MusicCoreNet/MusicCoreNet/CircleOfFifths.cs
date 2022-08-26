using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Helper functionality relating to the circle of fifths.
/// </summary>
///<seealso cref="CircleOfFourths"/>
public static class CircleOfFifths
{
    /// <summary>
    /// A comparer that can be used to compare <see cref="NoteSpelling"/> instances based on their position in
    /// the circle of fifths.
    /// </summary>
    public static readonly IComparer<NoteSpelling> NoteSpellingComparer = new NoteSpellingComparerType();

    private sealed class NoteSpellingComparerType : IComparer<NoteSpelling>
    {
        /// <inheritdoc/>
        public int Compare(NoteSpelling x, NoteSpelling y)
        {
            return x.CircleOfFifthsIndexRelativeToC.CompareTo(y.CircleOfFifthsIndexRelativeToC);
        }
    }
}
