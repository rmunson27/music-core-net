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
    /// A comparer that can be used to compare <see cref="NoteClass"/> instances based on their position in
    /// the circle of fourths.
    /// </summary>
    public static readonly IComparer<NoteClass> NoteClassComparer = new NoteClassComparerType();

    private sealed class NoteClassComparerType : IComparer<NoteClass>
    {
        /// <inheritdoc/>
        public int Compare(NoteClass x, NoteClass y)
        {
            return y.CircleOfFifthsIndexRelativeToC.CompareTo(x.CircleOfFifthsIndexRelativeToC);
        }
    }
}
