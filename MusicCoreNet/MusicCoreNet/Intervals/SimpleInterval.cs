using Rem.Core.Attributes;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a simple interval (spanning less than one octave).
/// </summary>
/// <remarks>
/// The default value of this struct represents a perfect unison.
/// </remarks>
public readonly record struct SimpleInterval
{
    #region Constants
    /// <summary>
    /// A <see cref="SimpleInterval"/> representing a perfect fourth.
    /// </summary>
    public static readonly SimpleInterval PerfectFourth = Interval.Perfect.Fourth;

    /// <summary>
    /// A <see cref="SimpleInterval"/> representing a perfect unison.
    /// </summary>
    public static readonly SimpleInterval PerfectUnison = Interval.Perfect.Unison;

    /// <summary>
    /// A <see cref="SimpleInterval"/> representing a perfect fifth.
    /// </summary>
    public static readonly SimpleInterval PerfectFifth = Interval.Perfect.Fifth;
    #endregion

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

    /// <summary>
    /// Creates a new perfectable instance of this struct with the supplied quality and number.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    public static SimpleInterval CreatePerfectable(
        PerfectableIntervalQuality Quality, PerfectableSimpleIntervalNumber Number)
        => new(Quality, Number);

    /// <summary>
    /// Creates a new imperfectable instance of this struct with the supplied quality and number.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    public static SimpleInterval CreateImperfectable(
        ImperfectableIntervalQuality Quality, ImperfectableSimpleIntervalNumber Number)
        => new(Quality, Number);
    #endregion

    #region Classification
    #region Perfectability
    /// <summary>
    /// Gets whether or not this instance represents a perfectable interval, returning the perfectable quality and
    /// number in <see langword="out"/> parameters if so and returning the imperfectable quality and number in
    /// <see langword="out"/> parameters if not.
    /// </summary>
    /// <param name="PerfectableQuality"></param>
    /// <param name="PerfectableNumber"></param>
    /// <param name="ImperfectableQuality"></param>
    /// <param name="ImperfectableNumber"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableIntervalQuality PerfectableQuality,
        out PerfectableSimpleIntervalNumber PerfectableNumber,
        out ImperfectableIntervalQuality ImperfectableQuality,
        out ImperfectableSimpleIntervalNumber ImperfectableNumber)
    {
        if (IsPerfectable())
        {
            PerfectableQuality = (PerfectableIntervalQuality)Quality;
            PerfectableNumber = (PerfectableSimpleIntervalNumber)Number;
            (ImperfectableQuality, ImperfectableNumber) = (default, default);
            return true;
        }
        else
        {
            (PerfectableQuality, PerfectableNumber) = (default, default);
            ImperfectableQuality = (ImperfectableIntervalQuality)Quality;
            ImperfectableNumber = (ImperfectableSimpleIntervalNumber)Number;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance represents a perfectable interval, returning the perfectable quality and
    /// number in <see langword="out"/> parameters if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableIntervalQuality Quality, out PerfectableSimpleIntervalNumber Number)
    {
        if (this.Quality.IsPerfectable(out var pQuality) && this.Number.IsPerfectable(out var pNumber))
        {
            Quality = pQuality;
            Number = pNumber;
            return true;
        }
        else
        {
            Quality = default;
            Number = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance represents a perfectable interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Perfectability == IntervalPerfectability.Perfectable;

    /// <summary>
    /// Gets whether or not this instance represents an imperfectable interval, returning the imperfectable quality
    /// and number in <see langword="out"/> parameters if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsImperfectable(
        out ImperfectableIntervalQuality Quality, out ImperfectableSimpleIntervalNumber Number)
    {
        if (this.Quality.IsImperfectable(out var npQuality) && this.Number.IsImperfectable(out var npNumber))
        {
            Quality = npQuality;
            Number = npNumber;
            return true;
        }
        else
        {
            Quality = default;
            Number = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance represents an imperfectable interval.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => Perfectability == IntervalPerfectability.Imperfectable;
    #endregion

    #region Specific Qualities
    /// <summary>
    /// Gets whether or not this interval is augmented, setting <paramref name="Degree"/> to the degree to which it
    /// is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonZero] out int Degree) => Quality.IsAugmented(out Degree);

    /// <summary>
    /// Gets whether or not this interval base is augmented.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => Quality.IsAugmented();

    /// <summary>
    /// Gets whether or not this interval is diminished, setting <paramref name="Degree"/> to the degree to which it
    /// is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonZero] out int Degree) => Quality.IsDiminished(out Degree);

    /// <summary>
    /// Gets whether or not this interval base is diminished.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => Quality.IsDiminished();
    #endregion
    #endregion

    #region Computation
    /// <summary>
    /// Gets a <see cref="SimpleInterval"/> equivalent to this instance with the quality shifted by the degree
    /// passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public SimpleInterval WithQualityShiftedBy(int Degree)
        => IsPerfectable()
            ? new(Quality.UnsafeAsPerfectable.ShiftedBy(Degree), Number)
            : new(Quality.UnsafeAsImperfectable.ShiftedBy(Degree), Number);
    #endregion

    #region Arithmetic
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
    /// <param name="AdditionalOctaves"></param>
    /// <returns></returns>
    public Interval WithAdditionalOctaves([NonNegative] int AdditionalOctaves)
        => new(this, Throw.IfArgNegative(AdditionalOctaves, nameof(AdditionalOctaves)));
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
