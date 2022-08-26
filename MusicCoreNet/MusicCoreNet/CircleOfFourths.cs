using System;
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
}
