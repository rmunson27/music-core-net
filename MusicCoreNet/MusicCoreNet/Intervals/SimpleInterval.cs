using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a perfectable simple interval (spanning less than one octave).
/// </summary>
/// <remarks>
/// The default value of this struct represents a perfect unison.
/// </remarks>
/// <param name="Quality">The quality of the interval.</param>
/// <param name="Number">The number of the interval.</param>
public readonly record struct PerfectableSimpleInterval(
    PerfectableIntervalQuality Quality, PerfectableSimpleIntervalNumber Number)
{
    /// <summary>
    /// Gets the circle of fifths index of this instance relative to a perfect unison.
    /// </summary>
    public int CircleOfFifthsIndex => Number.CircleOfFifthsIndex
                                        + Quality.PerfectBasedIndex * SimpleIntervalNumber.ValuesCount;

    /// <summary>
    /// Gets the number of half steps spanning this instance.
    /// </summary>
    public int HalfSteps => Number.PerfectHalfSteps + Quality.PerfectBasedIndex;

    /// <summary>
    /// Returns a new <see cref="PerfectableSimpleInterval"/> equivalent to the supplied value modified by the
    /// supplied <see cref="Accidental"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="accidental"></param>
    /// <returns></returns>
    public static PerfectableSimpleInterval operator +(PerfectableSimpleInterval interval, Accidental accidental)
        => interval with { Quality = interval.Quality + accidental };

    /// <summary>
    /// Explicitly converts a <see cref="SimpleInterval"/> to an <see cref="ImperfectableSimpleInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableSimpleInterval(SimpleInterval interval)
        => interval.IsPerfectable(out var p)
            ? p
            : throw new InvalidCastException($"Simple interval '{interval}' is not perfectable.");

    /// <summary>
    /// Gets the <see cref="Interval"/> equivalent to this instance with the number of additional octaves passed in.
    /// </summary>
    /// <param name="octaves"></param>
    /// <returns></returns>
    public Interval PlusOctaves([NonNegative] int octaves)
        => new(this, Throw.IfArgNegative(octaves, nameof(octaves)));

    /// <summary>
    /// Determines if this instance equals another <see cref="ImperfectableSimpleInterval"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PerfectableSimpleInterval other) => Quality == other.Quality && Number == other.Number;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Quality, Number);

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Interval.ToString(Quality.ToString(), Number.Abbreviation);
}

/// <summary>
/// Represents an imperfectable simple interval (spanning less than one octave).
/// </summary>
/// <remarks>
/// The default value of this struct represents a major second.
/// </remarks>
/// <param name="Quality">The quality of the interval.</param>
/// <param name="Number">The number of the interval.</param>
public readonly record struct ImperfectableSimpleInterval(
    ImperfectableIntervalQuality Quality, ImperfectableSimpleIntervalNumber Number)
{
    /// <summary>
    /// Gets the circle of fifths index of this instance relative to a perfect unison.
    /// </summary>
    public int CircleOfFifthsIndex => Number.MajorCircleOfFifthsIndex
                                        + Quality.MajorBasedIndex * SimpleIntervalNumber.ValuesCount;

    /// <summary>
    /// Gets the number of half steps spanning this instance.
    /// </summary>
    public int HalfSteps => Number.MajorHalfSteps + Quality.MajorBasedIndex;

    /// <summary>
    /// Returns a new <see cref="ImperfectableSimpleInterval"/> equivalent to the supplied value modified by the
    /// supplied <see cref="Accidental"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="accidental"></param>
    /// <returns></returns>
    public static ImperfectableSimpleInterval operator +(ImperfectableSimpleInterval interval, Accidental accidental)
        => interval with { Quality = interval.Quality + accidental };

    /// <summary>
    /// Explicitly converts a <see cref="SimpleInterval"/> to an <see cref="ImperfectableSimpleInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableSimpleInterval(SimpleInterval interval)
        => interval.IsImperfectable(out var ip)
            ? ip
            : throw new InvalidCastException($"Simple interval '{interval}' is not imperfectable.");

    /// <summary>
    /// Gets the <see cref="Interval"/> equivalent to this instance with the number of additional octaves passed in.
    /// </summary>
    /// <param name="octaves"></param>
    /// <returns></returns>
    public Interval PlusOctaves([NonNegative] int octaves)
        => new(this, Throw.IfArgNegative(octaves, nameof(octaves)));

    /// <summary>
    /// Determines if this instance equals another <see cref="ImperfectableSimpleInterval"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ImperfectableSimpleInterval other) => Quality == other.Quality && Number == other.Number;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Quality, Number);

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Interval.ToString(Quality.ToString(), Number.Abbreviation);
}

/// <summary>
/// Represents a simple interval (spanning less than one octave).
/// </summary>
/// <remarks>
/// The default value of this struct represents a perfect unison.
/// </remarks>
public readonly record struct SimpleInterval
{
    #region Properties
    /// <summary>
    /// Gets a <see cref="SimpleInterval"/> equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public SimpleInterval Inversion => new(Quality.Inversion, Number.Inversion);

    /// <summary>
    /// Gets the number of the interval represented by this instance.
    /// </summary>
    public SimpleIntervalNumber Number { get; }

    /// <summary>
    /// Gets the quality of the interval represented by the current instance.
    /// </summary>
    public IntervalQuality Quality { get; }

    /// <summary>
    /// Gets the circle of fifths index of this instance relative to a perfect unison.
    /// </summary>
    public int CircleOfFifthsIndex => Number.CircleOfFifthsIndex
                                        + Quality.PerfectOrMajorBasedIndex(Perfectability)
                                            * SimpleIntervalNumber.ValuesCount;

    /// <summary>
    /// Gets the number of half steps spanning the simple interval represented by this object.
    /// </summary>
    public int HalfSteps => Number.PerfectOrMajorHalfSteps + Quality.PerfectOrMajorBasedIndex(Perfectability);

    /// <summary>
    /// Gets the perfectability of the current instance.
    /// </summary>
    public IntervalPerfectability Perfectability => Number.Perfectability; // Quality perfectability could be ambiguous
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="SimpleInterval"/> struct with the quality and number
    /// passed in.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    internal SimpleInterval(IntervalQuality Quality, SimpleIntervalNumber Number)
    {
        this.Quality = Quality;
        this.Number = Number;
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="SimpleInterval"/> with the quality and number passed in.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// The perfectabilities of the quality and number did not match.
    /// </exception>
    public static SimpleInterval Create(IntervalQuality Quality, SimpleIntervalNumber Number)
        => Quality.IsPerfectability(Number.Perfectability)
            ? new(Quality, Number)
            : throw Interval.PerfectabilityMismatch(Number.Perfectability);

    /// <summary>
    /// Creates a new <see cref="SimpleInterval"/> spanning the number of half steps passed in with the simplest
    /// possible interval quality (i.e. closest to perfect).
    /// </summary>
    /// <param name="halfSteps">The number of half steps to span.</param>
    /// <param name="tritoneQualityType">
    /// The kind of quality to assign to a tritone (6 half steps).
    /// <para/>
    /// This resolves the ambiguity between an augmented fourth and a diminished fifth.
    /// </param>
    /// <returns>
    /// The <see cref="SimpleInterval"/> spanning <paramref name="halfSteps"/> half steps with the simplest
    /// possible interval quality, or the result of applying <paramref name="tritoneQualityType"/> to a fourth or a
    /// fifth as appropriate if <paramref name="halfSteps"/> indicates a tritone.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="halfSteps"/> was negative or spanned an octave or more.
    /// </exception>
    public static SimpleInterval SimplestWithHalfSteps(
        [NonNegative, LessThanInteger(NotePitchClass.ValuesCount)] int halfSteps,
        PeripheralIntervalQualityKind tritoneQualityType)
        => new(
            IntervalQuality.OfSimplestIntervalWithHalfSteps(halfSteps, tritoneQualityType),
            SimpleIntervalNumber.OfSimplestIntervalWithHalfSteps(halfSteps, tritoneQualityType));

    /// <summary>
    /// Creates a new <see cref="SimpleInterval"/> spanning the number of half steps passed in with the simplest
    /// possible interval quality (i.e. closest to perfect).
    /// </summary>
    /// <param name="halfSteps">The number of half steps to span.</param>
    /// <returns>
    /// The <see cref="SimpleInterval"/> spanning <paramref name="halfSteps"/> half steps with the simplest
    /// possible interval quality, or <see langword="null"/> if <paramref name="halfSteps"/> is equal to 6 (a tritone,
    /// as this case is ambiguous between an augmented fourth and a diminished fifth).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="halfSteps"/> was negative or spanned an octave or more.
    /// </exception>
    public static SimpleInterval? SimplestWithHalfSteps(
        [NonNegative, LessThanInteger(NotePitchClass.ValuesCount)] int halfSteps)
        => IntervalQuality.OfSimplestIntervalWithHalfSteps(halfSteps) is IntervalQuality quality
            ? new(quality, (SimpleIntervalNumber)SimpleIntervalNumber.OfSimplestIntervalWithHalfSteps(halfSteps)!)
            : null;

    /// <summary>
    /// Creates a new <see cref="SimpleInterval"/> from its perfect-unison-based circle of fifths index.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static SimpleInterval FromCircleOfFifthsIndex(int Index)
    {
        // Find the quality and number circle of fifths indexes
        // Need to adjust for the fact that a perfect fourth has index -1 with respect to a perfect unison
        // (and the perfect quality has index 0 in that case)
        var qualityCircleOfFifthsIndex
            = Maths.FloorDivRem(Index + 1, SimpleIntervalNumber.ValuesCount, out var numberCircleOfFifthsIndex);
        numberCircleOfFifthsIndex--;
        if (numberCircleOfFifthsIndex == -1) qualityCircleOfFifthsIndex++;

        var number = SimpleIntervalNumber.FromCircleOfFifthsIndex(numberCircleOfFifthsIndex);
        IntervalQuality quality = number.IsPerfectable()
                                    ? PerfectableIntervalQuality.FromPerfectBasedIndex(qualityCircleOfFifthsIndex)
                                    : ImperfectableIntervalQuality.FromMajorBasedIndex(qualityCircleOfFifthsIndex);
        return new(quality, number);
    }
    #endregion

    #region Arithmetic
    /// <summary>
    /// Returns a new <see cref="SimpleInterval"/> equivalent to the supplied instance modified by the
    /// supplied <see cref="Accidental"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="accidental"></param>
    /// <returns></returns>
    public static SimpleInterval operator +(SimpleInterval interval, Accidental accidental)
        => interval.IsPerfectable(out var p, out var ip)
            ? p + accidental
            : ip + accidental;

    /// <summary>
    /// Computes the difference between this <see cref="SimpleInterval"/> and another, collapsing the result into a
    /// <see cref="SimpleInterval"/> and setting whether or not the subtraction underflows past a unison in an
    /// <see langword="out"/> parameter.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="underflows"></param>
    /// <returns></returns>
    internal SimpleInterval MinusWithUnderflow(SimpleInterval other, out bool underflows)
    {
        // Subtraction underflows past a unison depending on the difference between the interval numbers
        // Add 1 to the difference, as for example, a third (3) minus a unison (1) is a third (3), not a second (2).
        underflows = Number - other.Number + 1 <= 0;

        return this - other;
    }

    /// <summary>
    /// Subtracts the two <see cref="SimpleInterval"/> instances.
    /// </summary>
    /// <remarks>
    /// This method collapses the result into an instance of <see cref="SimpleInterval"/> by adding or removing
    /// octaves so that the return value is less than an octave.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleInterval operator -(SimpleInterval lhs, SimpleInterval rhs) => lhs + (-rhs);

    /// <summary>
    /// Computes the sum of this <see cref="SimpleInterval"/> and another, collapsing the result into a
    /// <see cref="SimpleInterval"/> and setting whether or not the addition overflows past an octave in an
    /// <see langword="out"/> parameter.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="overflows"></param>
    /// <returns></returns>
    internal SimpleInterval PlusWithOverflow(SimpleInterval other, out bool overflows)
    {
        // Addition overflows past an octave depending on the sum of the interval numbers
        // Subtract 1 from the sum, as for example, a unison (1) plus a unison (1) is a unison (1), not a second (2). 
        overflows = Number + other.Number - 1 >= 8;

        return this + other;
    }

    /// <summary>
    /// Adds the two <see cref="SimpleInterval"/> instances.
    /// </summary>
    /// <remarks>
    /// This method collapses the result into an instance of <see cref="SimpleInterval"/> by adding or removing
    /// octaves so that the return value is less than an octave.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleInterval operator +(SimpleInterval lhs, SimpleInterval rhs)
    {
        #region Number
        var newUnisonBasedNumberIndex = lhs.Number.CircleOfFifthsIndex + rhs.Number.CircleOfFifthsIndex;
        var ubni_determinant = newUnisonBasedNumberIndex + 1; // Get rid of the -1 label for fourths
        var qualityShift = Maths.FloorDivRem(ubni_determinant, SimpleIntervalNumber.ValuesCount, out ubni_determinant);
        newUnisonBasedNumberIndex = ubni_determinant - 1; // Add back the -1 label for fourths
        var newNumber = SimpleIntervalNumber.FromCircleOfFifthsIndex(newUnisonBasedNumberIndex);
        #endregion

        #region Quality
        var newQualityIndex = lhs.Quality.PerfectOrMajorBasedIndex(lhs.Perfectability)
                                + rhs.Quality.PerfectOrMajorBasedIndex(rhs.Perfectability)
                                + qualityShift;

        IntervalQuality newQuality = newNumber.IsPerfectable()
                                        ? PerfectableIntervalQuality.FromPerfectBasedIndex(newQualityIndex)
                                        : ImperfectableIntervalQuality.FromMajorBasedIndex(newQualityIndex);
        #endregion

        return new(newQuality, newNumber);
    }

    /// <summary>
    /// Gets the inversion of the <see cref="SimpleInterval"/> instance passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SimpleInterval operator -(SimpleInterval value) => value.Inversion;

    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another instance of this type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SimpleInterval other) => Quality == other.Quality && Number == other.Number;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Quality, Number);
    #endregion

    #region Conversion
    /// <summary>
    /// Gets the <see cref="Interval"/> equivalent to this instance with the number of additional octaves passed in.
    /// </summary>
    /// <param name="octaves"></param>
    /// <returns></returns>
    public Interval PlusOctaves([NonNegative] int octaves)
        => new(this, Throw.IfArgNegative(octaves, nameof(octaves)));

    /// <summary>
    /// Determines whether this instance is imperfectable, setting the equivalent
    /// <see cref="ImperfectableSimpleInterval"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public bool IsImperfectable(
        out ImperfectableSimpleInterval interval, out PerfectableSimpleInterval perfectableInterval)
        => Number.IsImperfectable(out var imperfectableNumber, out var perfectableNumber)
            ? Try.Success(out interval, new(Quality.UnsafeAsImperfectable, imperfectableNumber),
                          out perfectableInterval)
            : Try.Failure(out interval, out perfectableInterval, new(Quality.UnsafeAsPerfectable, perfectableNumber));

    /// <summary>
    /// Determines whether this instance is imperfectable, setting the equivalent
    /// <see cref="ImperfectableSimpleInterval"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableSimpleInterval interval)
        => Number.IsImperfectable(out var imperfectableNumber)
            ? Try.Success(out interval, new(Quality.UnsafeAsImperfectable, imperfectableNumber))
            : Try.Failure(out interval);

    /// <summary>
    /// Determines whether this instance is imperfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => Number.IsImperfectable();

    /// <summary>
    /// Determines whether this instance is perfectable, setting the equivalent
    /// <see cref="PerfectableSimpleInterval"/> in an <see langword="out"/> parameter if so and setting the equivalent
    /// <see cref="ImperfectableSimpleInterval"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="imperfectableInterval"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableSimpleInterval interval, out ImperfectableSimpleInterval imperfectableInterval)
        => Number.IsPerfectable(out var perfectableNumber, out var imperfectableNumber)
            ? Try.Success(out interval, new(Quality.UnsafeAsPerfectable, perfectableNumber),
                          out imperfectableInterval)
            : Try.Failure(out interval,
                          out imperfectableInterval, new(Quality.UnsafeAsImperfectable, imperfectableNumber));

    /// <summary>
    /// Determines whether this instance is perfectable, setting the equivalent
    /// <see cref="PerfectableSimpleInterval"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableSimpleInterval interval)
        => Number.IsPerfectable(out var perfectableNumber)
            ? Try.Success(out interval, new(Quality.UnsafeAsPerfectable, perfectableNumber))
            : Try.Failure(out interval);

    /// <summary>
    /// Determines whether this instance is perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Number.IsPerfectable();

    /// <summary>
    /// Explicitly converts an <see cref="Interval"/> to a <see cref="SimpleInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator SimpleInterval(Interval interval)
        => interval.Number.IsSimple(out var simpleNumber)
            ? new(interval.Quality, simpleNumber)
            : throw new InvalidCastException($"Interval {interval} is not simple.");

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableSimpleInterval"/> to a <see cref="SimpleInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator SimpleInterval(PerfectableSimpleInterval interval)
        => new(interval.Quality, interval.Number);

    /// <summary>
    /// Implicitly converts an <see cref="ImperfectableSimpleInterval"/> to a <see cref="SimpleInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator SimpleInterval(ImperfectableSimpleInterval interval)
        => new(interval.Quality, interval.Number);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Interval.ToString(Quality.ToString(), Number.Abbreviation);
    #endregion
    #endregion
}
