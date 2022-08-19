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
    #region Number
    internal static int UnisonBasedNumberIndex(this SimpleIntervalBase siBase)
#pragma warning disable CS8509 // This should handle everything
        => Throw.IfArgNull(siBase, nameof(siBase)) switch
#pragma warning restore CS8509
        {
            PerfectableSimpleIntervalBase(_, var number) => UnisonBasedIndex(number),
            NonPerfectableSimpleIntervalBase(_, var number) => UnisonBasedIndex(number),
        };

    /// <summary>
    /// Determines the unison-relative index of the <see cref="PerfectableSimpleIntervalNumber"/> passed in with respect to
    /// the circle of fifths.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="number"/> was an unnamed enum value.
    /// </exception>
    public static int UnisonBasedIndex([NamedEnum] PerfectableSimpleIntervalNumber number) => number switch
    {
        Unison => 0,
        Fourth => -1,
        Fifth => 1,
        _ => throw new InvalidEnumArgumentException($"Undefined {nameof(PerfectableSimpleIntervalNumber)} value."),
    };

    internal static PerfectableSimpleIntervalNumber PerfectableNumberFromUnisonBasedIndex(int index) => index switch
    {
        -1 => Fourth,
        0 => Unison,
        1 => Fifth,
        _ => throw new ArgumentOutOfRangeException(
                nameof(index), index,
                $"Index passed in did not represent a {nameof(PerfectableSimpleIntervalNumber)} value."),
    };

    internal static NonPerfectableSimpleIntervalNumber NonPerfectableNumberFromUnisonBasedIndex(int index) => index switch
    {
        2 => Second,
        3 => Sixth,
        4 => Third,
        5 => Seventh,
        _ => throw new ArgumentOutOfRangeException(
                nameof(index), index,
                $"Index passed in did not represent a {nameof(NonPerfectableSimpleIntervalNumber)} value."),
    };

    /// <summary>
    /// Determines the unison-relative index of the <see cref="NonPerfectableSimpleIntervalNumber"/> passed in with respect
    /// to the circle of fifths.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="number"/> was an unnamed enum value.
    /// </exception>
    public static int UnisonBasedIndex([NamedEnum] NonPerfectableSimpleIntervalNumber number) => number switch
    {
        Second => 2,
        Third => 4,
        Sixth => 3,
        Seventh => 5,
        _ => throw new InvalidEnumArgumentException($"Undefined {nameof(NonPerfectableSimpleIntervalNumber)} value."),
    };
    #endregion
}
