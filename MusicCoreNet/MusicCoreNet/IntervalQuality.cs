using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// General static functionality relating to interval qualities.
/// </summary>
public static class IntervalQualities
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
/// Represents a general interval quality that can be either perfectable or non-perfectable.
/// </summary>
public readonly record struct IntervalQuality
{
    #region Properties And Fields
    private readonly InternalQualityStruct InternalQuality;

    /// <summary>
    /// Gets the perfectability of this interval quality.
    /// </summary>
    public IntervalPerfectability Perfectability { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the perfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(PerfectableIntervalQuality Quality)
    {
        InternalQuality = new();
        InternalQuality.Perfectable = Quality;
        Perfectability = Perfectable;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the non-perfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(NonPerfectableIntervalQuality Quality)
    {
        InternalQuality = new();
        InternalQuality.NonPerfectable = Quality;
        Perfectability = NonPerfectable;
    }
    #endregion

    #region Methods
    #region Classification
    #region Perfectability
    /// <summary>
    /// Gets whether or not this instance is perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Perfectability == Perfectable;

    /// <summary>
    /// Gets whether or not this instance is perfectable, setting the perfectable interval quality details in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality Quality)
    {
        if (Perfectability == Perfectable)
        {
            Quality = InternalQuality.Perfectable;
            return true;
        }
        else
        {
            Quality = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is non-perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsNonPerfectable() => Perfectability == NonPerfectable;

    /// <summary>
    /// Gets whether or not this instance is non-perfectable, setting the perfectable interval quality details in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsNonPerfectable(out NonPerfectableIntervalQuality Quality)
    {
        if (Perfectability == NonPerfectable)
        {
            Quality = InternalQuality.NonPerfectable;
            return true;
        }
        else
        {
            Quality = default;
            return false;
        }
    }
    #endregion

    #region Specific Quality
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
        => IsPerfectable()
            ? InternalQuality.Perfectable.IsAugmented(out Degree)
            : InternalQuality.NonPerfectable.IsAugmented(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => IsPerfectable()
                                    ? InternalQuality.Perfectable.IsAugmented()
                                    : InternalQuality.NonPerfectable.IsAugmented();

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => IsNonPerfectable() && InternalQuality.NonPerfectable.IsMajor();

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => IsPerfectable() && InternalQuality.Perfectable.IsPerfect();

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => IsNonPerfectable() && InternalQuality.NonPerfectable.IsMinor();

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
        => IsPerfectable()
             ? InternalQuality.Perfectable.IsDiminished(out Degree)
             : InternalQuality.NonPerfectable.IsDiminished(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => IsPerfectable()
                                    ? InternalQuality.Perfectable.IsDiminished()
                                    : InternalQuality.NonPerfectable.IsDiminished();
    #endregion
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IntervalQuality other)
    {
        if (Perfectability == other.Perfectability)
        {
            return Perfectability switch
            {
                Perfectable => InternalQuality.Perfectable == other.InternalQuality.Perfectable,
                _ => InternalQuality.NonPerfectable == other.InternalQuality.NonPerfectable,
            };
        }
        else return false;
    }

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Perfectability == Perfectable
                                            ? InternalQuality.Perfectable.GetHashCode()
                                            : InternalQuality.NonPerfectable.GetHashCode();
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="PerfectableIntervalQuality"/> to an instance of this type.
    /// </summary>
    /// <param name="Quality"></param>
    public static implicit operator IntervalQuality(PerfectableIntervalQuality Quality) => new(Quality);

    /// <summary>
    /// Implicitly converts a <see cref="NonPerfectableIntervalQuality"/> to an instance of this type.
    /// </summary>
    /// <param name="Quality"></param>
    public static implicit operator IntervalQuality(NonPerfectableIntervalQuality Quality) => new(Quality);
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
    public IntervalQuality Shift(int degree)
        => IsPerfectable()
            ? new(new PerfectableIntervalQuality(degree + InternalQuality.Perfectable.PerfectBasedIndex))
            : new(new NonPerfectableIntervalQuality(degree + InternalQuality.NonPerfectable.MajorBasedIndex));

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public IntervalQuality Inversion() => IsPerfectable()
                                            ? new(InternalQuality.Perfectable.Inversion())
                                            : new(InternalQuality.NonPerfectable.Inversion());
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => IsPerfectable()
                                            ? InternalQuality.Perfectable.ToString()
                                            : InternalQuality.NonPerfectable.ToString();
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// Internally represents the two possible interval quality types as a discriminated union.
    /// </summary>
    /// <remarks>
    /// It is the responsibility of the <see cref="IntervalQuality"/> struct to maintain information about which of
    /// the two options is selected, or undefined behavior could occur.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    private struct InternalQualityStruct
    {
        [FieldOffset(0)]
        public PerfectableIntervalQuality Perfectable;

        [FieldOffset(0)]
        public NonPerfectableIntervalQuality NonPerfectable;
    }
    #endregion
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
    internal PerfectableIntervalQuality(int PerfectBasedIndex) { this.PerfectBasedIndex = PerfectBasedIndex; }
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

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return $"Augmented {{ Degree = {augmentedDegree} }}";
        else if (IsDiminished(out var diminishedDegree)) return $"Diminished {{ Degree = {diminishedDegree} }}";
        else return "Perfect";
    }
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
    internal NonPerfectableIntervalQuality(int NumericalValue) { MajorBasedIndex = NumericalValue; }
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
    public bool IsDiminished() => MajorBasedIndex < -1;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return $"Augmented {{ Degree = {augmentedDegree} }}";
        else if (IsDiminished(out var diminishedDegree)) return $"Diminished {{ Degree = {diminishedDegree} }}";
        else return MajorBasedIndex == 0 ? "Major" : "Minor";
    }
    #endregion
    #endregion
}
