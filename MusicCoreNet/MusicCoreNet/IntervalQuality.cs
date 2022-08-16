using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// General static functionality relating to interval qualities.
/// </summary>
public static class IntervalQuality
{
    /// <summary>
    /// Gets a minor interval quality.
    /// </summary>
    public static NonPerfectableIntervalQuality Minor => NonPerfectableIntervalQuality.Minor;

    /// <summary>
    /// Gets a perfect interval quality.
    /// </summary>
    public static PerfectableIntervalQuality Perfect => PerfectableIntervalQuality.Perfect;

    /// <summary>
    /// Gets a major interval quality.
    /// </summary>
    public static NonPerfectableIntervalQuality Major => NonPerfectableIntervalQuality.Major;
}

/// <summary>
/// Represents the quality of a perfectable interval.
/// </summary>
public readonly record struct PerfectableIntervalQuality
{
    #region Constants
    /// <summary>
    /// A perfectable interval quality representing a perfect interval.
    /// </summary>
    public static readonly PerfectableIntervalQuality Perfect = new(0);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Represents this quality as an index based on <see cref="Perfect"/>.
    /// </summary>
    public int PerfectBasedIndex { get; }
    #endregion

    #region Constructor
    private PerfectableIntervalQuality(int PerfectBasedIndex) { this.PerfectBasedIndex = PerfectBasedIndex; }
    #endregion

    #region Methods
    #region Factory
    #region Name
    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> representing an augmented interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static PerfectableIntervalQuality Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> representing a diminished interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static PerfectableIntervalQuality Diminished([Positive] int Degree = 1)
        => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)));
    #endregion

    #region Index
    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> from the <see cref="Perfect"/>-relative integer index
    /// passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static PerfectableIntervalQuality FromPerfectBasedIndex(int Index) => new(Index);
    #endregion
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
    {
        if (IsAugmented())
        {
            Degree = PerfectBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => PerfectBasedIndex > 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => PerfectBasedIndex == 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
    {
        if (IsDiminished())
        {
            Degree = -PerfectBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => PerfectBasedIndex < 0;
    #endregion

    #region Computation
    /// <summary>
    /// Returns an interval quality equivalent to this one shifted by a given integer degree.
    /// </summary>
    /// <remarks>
    /// Positive <paramref name="degree"/> values will cause the result to be more augmented, whereas negative
    /// <paramref name="degree"/> values will cause the result to be more diminished.
    /// </remarks>
    /// <param name="degree"></param>
    /// <returns></returns>
    public PerfectableIntervalQuality Shift(int degree) => new(degree + PerfectBasedIndex);

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public PerfectableIntervalQuality Inversion() => new(-PerfectBasedIndex);
    #endregion
    #endregion
}

/// <summary>
/// Represents the quality of a non-perfectable interval.
/// </summary>
public readonly record struct NonPerfectableIntervalQuality
{
    #region Constants
    /// <summary>
    /// A non-perfectable interval quality representing a minor interval.
    /// </summary>
    public static readonly NonPerfectableIntervalQuality Minor = new(-1);

    /// <summary>
    /// A non-perfectable interval quality representing a major interval.
    /// </summary>
    public static readonly NonPerfectableIntervalQuality Major = new(0);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Represents this quality as an index based on <see cref="Major"/>.
    /// </summary>
    public int MajorBasedIndex { get; }
    #endregion

    #region Constructor
    private NonPerfectableIntervalQuality(int NumericalValue) { MajorBasedIndex = NumericalValue; }
    #endregion

    #region Methods
    #region Factory
    #region Name
    /// <summary>
    /// Creates a new <see cref="NonPerfectableIntervalQuality"/> representing an augmented interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static NonPerfectableIntervalQuality Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="NonPerfectableIntervalQuality"/> representing a diminished interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static NonPerfectableIntervalQuality Diminished([Positive] int Degree = 1)
        => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)) - 1);
    #endregion

    #region Index
    /// <summary>
    /// Creates a new <see cref="NonPerfectableIntervalQuality"/> from the <see cref="Major"/>-relative integer index
    /// passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static NonPerfectableIntervalQuality FromMajorBasedIndex(int Index) => new(Index);
    #endregion
    #endregion

    #region Computation
    /// <summary>
    /// Returns an interval quality equivalent to this one shifted by a given integer degree.
    /// </summary>
    /// <remarks>
    /// Positive <paramref name="degree"/> values will cause the result to be more augmented, whereas negative
    /// <paramref name="degree"/> values will cause the result to be more diminished.
    /// </remarks>
    /// <param name="degree"></param>
    /// <returns></returns>
    public NonPerfectableIntervalQuality Shift(int degree) => new(degree + MajorBasedIndex);

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public NonPerfectableIntervalQuality Inversion() => new(-MajorBasedIndex - 1);
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
    {
        if (IsAugmented())
        {
            Degree = MajorBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => MajorBasedIndex > 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => MajorBasedIndex == 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => MajorBasedIndex == -1;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
    {
        if (IsDiminished())
        {
            Degree = -MajorBasedIndex - 1;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => MajorBasedIndex < 0;
    #endregion
    #endregion
}
