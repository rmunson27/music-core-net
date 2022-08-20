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
    /// Gets the circle of fifths index of a perfect interval numbered with the current instance relative to a
    /// perfect unison.
    /// </summary>
    /// <param name="pn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(1)]
    public static int UnisonBasedPerfectIndex([NamedEnum] this PerfectableSimpleIntervalNumber pn) => pn switch
    {
        Fourth => -1,
        Unison => 0,
        Fifth => 1,
        _ => throw UndefinedPerfectable,
    };

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
    /// Gets the circle of fifths index of a major interval numbered with the current instance relative to a
    /// perfect unison.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)]
    public static int UnisonBasedMajorIndex([NamedEnum] this NonPerfectableSimpleIntervalNumber npn) => npn switch
    {
        Second => 2,
        Sixth => 3,
        Third => 4,
        Seventh => 5,
        _ => throw UndefinedNonPerfectable,
    };

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
/// Static functionality for the <see cref="PerfectableSimpleIntervalNumber"/> enum.
/// </summary>
public static class PerfectableSimpleIntervalNumbers
{
    /// <summary>
    /// Gets the <see cref="PerfectableSimpleIntervalNumber"/> of a perfect interval with the circle-of-fifths perfect
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any perfect interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.UnisonBasedPerfectIndex(PerfectableSimpleIntervalNumber)"/>
    public static PerfectableSimpleIntervalNumber FromUnisonBasedPerfectIndex(
        [GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(1)] int Index)
        => Index switch
        {
            -1 => Fourth,
            0 => Unison,
            1 => Fifth,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(Index), Index, "Index did not indicate any perfect interval."),
        };
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
/// Static functionality for the <see cref="PerfectableSimpleIntervalNumber"/> enum.
/// </summary>
public static class NonPerfectableSimpleIntervalNumbers
{
    /// <summary>
    /// Gets the <see cref="NonPerfectableSimpleIntervalNumber"/> of a major interval with the circle-of-fifths perfect
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any major interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.UnisonBasedMajorIndex(NonPerfectableSimpleIntervalNumber)"/>
    public static NonPerfectableSimpleIntervalNumber FromUnisonBasedMajorIndex(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)] int Index)
        => Index switch
        {
            2 => Second,
            3 => Sixth,
            4 => Third,
            5 => Seventh,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(Index), Index, "Index did not indicate any major interval."),
        };
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

