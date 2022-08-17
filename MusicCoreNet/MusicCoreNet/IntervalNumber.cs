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
/// Static functionality relating to interval numbers.
/// </summary>
public static class IntervalNumbers
{
    /// <summary>
    /// Gets the perfectability of the interval named by the number passed in.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> was not positive.</exception>
    public static IntervalPerfectability GetPerfectability([Positive] int number)
        // The numbers starts at 1 (unison), and there are seven values before the perfectability cycle repeats
        // Figure out where the number is in the cycle to determine the result
        => ((Throw.IfArgNotPositive(number, nameof(number)) - 1) % 7 + 1) switch
        {
            1 or 4 or 5 => Perfectable,
            _ => NonPerfectable,
        };

    internal static IntervalPerfectability GetPerfectabilityFromUnisonBasedIndex(int index) => index switch
    {
        >= -1 and <= 1 => Perfectable,
        >= 2 and <= 5 => NonPerfectable,
        _ => throw new Exception("Bug - should not happen ever."),
    };
}

/// <summary>
/// Static functionality relating to simple interval numbers.
/// </summary>
public static class SimpleIntervalNumbers
{
    #region Perfectable
    /// <summary>
    /// Gets the <see cref="PerfectableSimpleIntervalNumber"/> that is the inversion of the current instance.
    /// </summary>
    /// <param name="pn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static PerfectableSimpleIntervalNumber Inversion([NamedEnum] this PerfectableSimpleIntervalNumber pn)
        => pn switch
        {
            Unison => Unison,
            Fourth => Fifth,
            Fifth => Fourth,
            _ => throw UndefinedPerfectable,
        };

    /// <summary>
    /// Gets the number of half steps spanning the major version of the current
    /// <see cref="PerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="pn"></param>
    /// <returns></returns>
    [return: NonNegative]
    public static int PerfectHalfSteps(this PerfectableSimpleIntervalNumber pn) => pn switch
    {
        Unison => 0,
        Fourth => 5,
        Fifth => 7,
        _ => throw UndefinedPerfectable,
    };

    private static InvalidEnumArgumentException UndefinedPerfectable
        => new($"Undefined {nameof(PerfectableSimpleIntervalNumber)} value.");
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Gets the <see cref="NonPerfectableSimpleIntervalNumber"/> that is the inversion of the current instance.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static NonPerfectableSimpleIntervalNumber Inversion([NamedEnum] this NonPerfectableSimpleIntervalNumber npn)
        => npn switch
        {
            Second => Seventh,
            Third => Sixth,
            Sixth => Third,
            Seventh => Second,
            _ => throw UndefinedNonPerfectable,
        };

    /// <summary>
    /// Gets the number of half steps spanning the major version of the current
    /// <see cref="NonPerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    [return: Positive] public static int MajorHalfSteps(this NonPerfectableSimpleIntervalNumber npn) => npn switch
    {
        Second => 2,
        Third => 4,
        Sixth => 9,
        Seventh => 11,
        _ => throw UndefinedNonPerfectable,
    };

    private static InvalidEnumArgumentException UndefinedNonPerfectable
        => new($"Undefined {nameof(NonPerfectableSimpleIntervalNumber)} value.");
    #endregion
}

/// <summary>
/// Represents the number of a perfectable simple interval.
/// </summary>
public enum PerfectableSimpleIntervalNumber : byte
{
    /// <summary>
    /// Represents a unison.
    /// </summary>
    Unison = 1,

    /// <summary>
    /// Represents a fourth.
    /// </summary>
    Fourth = 4,

    /// <summary>
    /// Represents a fifth.
    /// </summary>
    Fifth = 5,
}

/// <summary>
/// Represents the number of a non-perfectable simple interval.
/// </summary>
public enum NonPerfectableSimpleIntervalNumber : byte
{
    /// <summary>
    /// Represents a second.
    /// </summary>
    Second = 2,

    /// <summary>
    /// Represents a third.
    /// </summary>
    Third = 3,

    /// <summary>
    /// Represents a sixth.
    /// </summary>
    Sixth = 6,

    /// <summary>
    /// Represents a seventh.
    /// </summary>
    Seventh = 7,
}

/// <summary>
/// Represents the perfectability of an interval.
/// </summary>
public enum IntervalPerfectability : byte
{
    /// <summary>
    /// Represents perfectable intervals (i.e. a perfect fourth).
    /// </summary>
    Perfectable,

    /// <summary>
    /// Represents non-perfectable intervals (i.e. a major second).
    /// </summary>
    NonPerfectable,
}

