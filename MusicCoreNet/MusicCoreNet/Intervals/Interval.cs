using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a musical interval between two notes.
/// </summary>
/// <remarks>
/// The default value of this struct is a perfect unison.
/// </remarks>
/// <param name="Base">An object defining the quality and base number of the interval.</param>
/// <param name="AdditionalOctaves">
/// The number of additional octaves added onto the base to make up the interval.
/// </param>
public readonly record struct Interval(SimpleInterval Base, [NonNegative] int AdditionalOctaves)
{
    #region Properties And Fields
    /// <summary>
    /// Gets the number of this interval.
    /// </summary>
    public IntervalNumber Number => new(Base.Number, AdditionalOctaves);

    /// <summary>
    /// Gets the quality of this interval.
    /// </summary>
    public IntervalQuality Quality => Base.Quality;

    /// <summary>
    /// Gets the perfectability of this interval.
    /// </summary>
    public IntervalPerfectability Perfectability => Base.Perfectability;

    /// <summary>
    /// Gets the number of half steps spanned by this instance.
    /// </summary>
    public int HalfSteps => Base.HalfSteps + _additionalOctaves * NotePitchClass.ValuesCount;

    /// <summary>
    /// Gets or initializes the number of additional octaves added on to the base to make up the interval.
    /// </summary>
    /// <exception cref="PropertySetOutOfRangeException">This property was initialized to a negative value.</exception>
    [NonNegative] public int AdditionalOctaves
    {
        get => _additionalOctaves;
        init => _additionalOctaves = Throw.IfPropSetNegative(value);
    }
    [NonNegative] private readonly int _additionalOctaves
        = Throw.IfArgNegative(AdditionalOctaves, nameof(AdditionalOctaves));
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="Interval"/> with the given quality and number.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// The perfectability of the quality and number did not match.
    /// </exception>
    public static Interval Create(IntervalQuality Quality, IntervalNumber Number)
        => new(SimpleInterval.Create(Quality, Number.SimpleBase), Number.AdditionalOctaves);
    #endregion

    #region Equality
    /// <summary>
    /// Determines whether two <see cref="Interval"/> instances are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(in Interval lhs, in Interval rhs) => lhs.Equals(in rhs);

    /// <summary>
    /// Determines whether two <see cref="Interval"/> instances are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(in Interval lhs, in Interval rhs) => !lhs.Equals(in rhs);

    bool IEquatable<Interval>.Equals(Interval other) => Equals(in other);

    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(in Interval other) => AdditionalOctaves == other.AdditionalOctaves && Base == other.Base;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Base, AdditionalOctaves);
    #endregion

    #region Arithmetic
    /// <summary>
    /// Returns a new <see cref="Interval"/> equivalent to the supplied instance modified by the
    /// specified <see cref="Accidental"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="accidental"></param>
    /// <returns></returns>
    public static Interval operator +(in Interval interval, Accidental accidental)
        => interval with { Base = interval.Base + accidental };

    /// <summary>
    /// Computes the sum of the two <see cref="Interval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Interval operator +(in Interval lhs, in Interval rhs)
    {
        var newBase = lhs.Base.PlusWithOverflow(rhs.Base, out var baseAdditionOverflows);

        var newAdditionalOctaves = lhs.AdditionalOctaves + rhs.AdditionalOctaves;
        if (baseAdditionOverflows) newAdditionalOctaves++;

        return new(newBase, newAdditionalOctaves);
    }

    /// <summary>
    /// Computes the difference between the two <see cref="Interval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">
    /// The difference between the intervals underflows past a unison.
    /// <para/>
    /// This will occur if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </exception>
    public static Interval operator -(in Interval lhs, in Interval rhs)
    {
        SubtractInPlace(in lhs, in rhs, out var newBase, out var newAdditionalOctaves);
        if (newAdditionalOctaves < 0) throw new OverflowException("The difference underflows past a unison.");
        return new(newBase, newAdditionalOctaves);
    }

    /// <summary>
    /// Computes the difference between the two <see cref="Interval"/> instances passed in, returning the resulting
    /// components that can be used to construct an <see cref="Interval"/> in <see langword="out"/> parameters.
    /// </summary>
    /// <remarks>
    /// This method is internal since it can be used to provide subtraction both for this struct and
    /// <see cref="SignedInterval"/> instances.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="newBase"></param>
    /// <param name="newAdditionalOctaves"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SubtractInPlace(
        in Interval lhs, in Interval rhs,
        out SimpleInterval newBase, out int newAdditionalOctaves)
    {
        newBase = lhs.Base.MinusWithUnderflow(rhs.Base, out var baseAdditionUnderflows);

        newAdditionalOctaves = lhs.AdditionalOctaves - rhs.AdditionalOctaves;
        if (baseAdditionUnderflows) newAdditionalOctaves--;
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="SimpleInterval"/> to an <see cref="Interval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator Interval(SimpleInterval interval) => new(interval, AdditionalOctaves: 0);

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableSimpleInterval"/> to an <see cref="Interval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator Interval(PerfectableSimpleInterval interval)
        => new(interval, AdditionalOctaves: 0);

    /// <summary>
    /// Implicitly converts an <see cref="ImperfectableSimpleInterval"/> to an <see cref="Interval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator Interval(ImperfectableSimpleInterval interval)
        => new(interval, AdditionalOctaves: 0);

    /// <summary>
    /// Determines whether this instance represents a simple interval, setting the equivalent
    /// <see cref="SimpleInterval"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="simpleInterval"></param>
    /// <returns></returns>
    public bool IsSimple(out SimpleInterval simpleInterval)
        => Number.IsSimple(out var simpleNumber)
            ? Try.Success(out simpleInterval, new(Quality, simpleNumber))
            : Try.Failure(out simpleInterval);

    /// <summary>
    /// Determines whether this instance represents a simple interval.
    /// </summary>
    /// <returns></returns>
    public bool IsSimple() => AdditionalOctaves == 0;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToString(Quality.ToString(), Number.Abbreviation);
    #endregion

    #region Qualities
    /// <summary>
    /// Creates a new <see cref="PeripheralIntervalQuality"/> representing an augmented interval quality with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static PeripheralIntervalQuality Augmented([Positive] int Degree = 1)
        => PeripheralIntervalQuality.Augmented(Degree);

    /// <summary>
    /// Gets an <see cref="ImperfectableIntervalQuality"/> representing a minor interval quality.
    /// </summary>
    public static ImperfectableIntervalQuality Minor => ImperfectableIntervalQuality.Minor;

    /// <summary>
    /// Gets a <see cref="PerfectableIntervalQuality"/> representing a perfect interval quality.
    /// </summary>
    public static PerfectableIntervalQuality Perfect => PerfectableIntervalQuality.Perfect;

    /// <summary>
    /// Gets an <see cref="ImperfectableIntervalQuality"/> representing a major interval quality.
    /// </summary>
    public static ImperfectableIntervalQuality Major => ImperfectableIntervalQuality.Major;

    /// <summary>
    /// Creates a new <see cref="PeripheralIntervalQuality"/> representing an augmented interval quality with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static PeripheralIntervalQuality Diminished([Positive] int Degree = 1)
        => PeripheralIntervalQuality.Diminished(Degree);
    #endregion

    #region Helpers
    /// <summary>
    /// Creates an exception for interval quality and number arguments with mismatched perfectability.
    /// </summary>
    /// <param name="numberPerfectability">
    /// The perfectability of the interval number in question.
    /// <para/>
    /// The quality perfectability will be inferred from this value.
    /// </param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ArgumentException PerfectabilityMismatch(IntervalPerfectability numberPerfectability)
        => new(
            $"Quality perfectability ({numberPerfectability.Opposite.ToString().ToLower()}) did not match"
                + $" number perfectability ({numberPerfectability.ToString().ToLower()}).");

    /// <summary>
    /// Gets a string representing the interval described by the strings passed in.
    /// </summary>
    /// <param name="qualityStr"></param>
    /// <param name="numberStr"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ToString(string qualityStr, string numberStr)
        => $"Interval {{ Quality = {qualityStr}, Number = {numberStr} }}";
    #endregion
    #endregion
}
