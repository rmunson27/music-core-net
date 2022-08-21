using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using static PerfectableSimpleIntervalNumber;
using static NonPerfectableSimpleIntervalNumber;

/// <summary>
/// Helper methods for dealing with the circle of fifths.
/// </summary>
public static class CircleOfFifths
{
    /// <summary>
    /// A comparer that can be used to compare <see cref="SimpleIntervalBase"/> instances based on their position in
    /// the circle of fifths.
    /// </summary>
    public static readonly IComparer<SimpleIntervalBase> Comparer = new SimpleIntervalBaseComparer();

    private sealed class SimpleIntervalBaseComparer : IComparer<SimpleIntervalBase>
    {
        /// <inheritdoc/>
        public int Compare(SimpleIntervalBase? x, SimpleIntervalBase? y)
        {
            // Set `null` to compare as below any other value
            if (x is null) return y is null ? 0 : -1;
            else if (y is null) return 1;

            // Compare the qualities and then the numbers if the qualities are equal
            return x.Quality.CircleOfFifthsIndex.CompareTo(y.Quality.CircleOfFifthsIndex) switch
            {
                0 => x.Number.CircleOfFifthsIndex().CompareTo(y.Number.CircleOfFifthsIndex()),
                var comp => comp,
            };
        }
    }
}
