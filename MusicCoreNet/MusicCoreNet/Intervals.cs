using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Static functionality relating to intervals.
/// </summary>
public static class Intervals
{
    /// <summary>
    /// Gets an object that can be used to build an augmented interval of the given augmented degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AugmentedIntervalBuilder Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Gets an object that can be used to build a minor interval.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MinorIntervalBuilder Minor() => new();

    /// <summary>
    /// Gets an object that can be used to build a perfect interval.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PerfectIntervalBuilder Perfect() => new();

    /// <summary>
    /// Gets an object that can be used to build a major interval.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MajorIntervalBuilder Major() => new();

    /// <summary>
    /// Gets an object that can be used to build a diminished interval of the given diminished degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DiminishedIntervalBuilder Diminished([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));
}
