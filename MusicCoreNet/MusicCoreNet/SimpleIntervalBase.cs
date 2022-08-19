using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a perfectable simple interval base that spans less than one octave.
/// </summary>
/// <param name="Quality">The quality of the interval.</param>
/// <param name="Number">The number of the interval.</param>
/// <exception cref="InvalidEnumArgumentException">
/// <paramref name="Number"/> was an unnamed enum value.
/// </exception>
public sealed record class PerfectableSimpleIntervalBase(
    PerfectableIntervalQuality Quality, [NamedEnum] PerfectableSimpleIntervalNumber Number) : SimpleIntervalBase
{
    #region Properties And Fields
    /// <inheritdoc/>
    [Positive] public override int NumberValue => (int)Number;

    /// <inheritdoc/>
    public override int HalfSteps => Number.PerfectHalfSteps() + Quality.PerfectBasedIndex;

    /// <inheritdoc/>
    private protected override int QualityPerfectOrMajorBasedIndex => Quality.PerfectBasedIndex;

    private protected override IntervalQuality QualityInternal => Quality;

    /// <summary>
    /// Gets or initializes the quality of the interval represented by the current instance.
    /// </summary>
    public new PerfectableIntervalQuality Quality { get; init; } = Quality;

    /// <inheritdoc/>
    public override IntervalPerfectability Perfectability => Perfectable;

    /// <summary>
    /// Gets or initializes the number of this interval.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    [NamedEnum] public PerfectableSimpleIntervalNumber Number
    {
        get => _number;
        init => _number = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly PerfectableSimpleIntervalNumber _number = Throw.IfEnumArgUnnamed(Number, nameof(Number));
    #endregion

    #region Methods
    #region Classification
    /// <inheritdoc/>
    public override bool IsAugmented([NonZero] out int Degree) => Quality.IsAugmented(out Degree);

    /// <inheritdoc/>
    public override bool IsAugmented() => Quality.IsAugmented();

    /// <inheritdoc/>
    public override bool IsDiminished([NonNegative] out int Degree) => Quality.IsDiminished(out Degree);

    /// <inheritdoc/>
    public override bool IsDiminished() => Quality.IsDiminished();
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this object is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PerfectableSimpleIntervalBase? other)
        => other is not null && Number == other.Number && Quality == other.Quality;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Quality, Number);
    #endregion

    #region Arithmetic
    private protected override SimpleIntervalBase InversionInternal() => Inversion();

    /// <summary>
    /// Gets the inversion of the <see cref="PerfectableSimpleIntervalBase"/> instance passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> was <see langword="null"/>.</exception>
    public static PerfectableSimpleIntervalBase operator -(PerfectableSimpleIntervalBase value)
        => Throw.IfArgNull(value, nameof(value)).Inversion();

    /// <summary>
    /// Gets a <see cref="PerfectableSimpleIntervalBase"/> equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public new PerfectableSimpleIntervalBase Inversion() => new(Quality.Inversion(), Number.Inversion());
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(SimpleIntervalBase)} {{ Quality = {Quality}, Number = {Number} }}";
    #endregion
    #endregion
}

/// <summary>
/// Represents a non-perfectable simple interval base that spans less than one octave.
/// </summary>
/// <param name="Quality">The quality of the interval.</param>
/// <param name="Number">The number of the interval.</param>
/// <exception cref="InvalidEnumArgumentException">
/// <paramref name="Number"/> was an unnamed enum value.
/// </exception>
public sealed record class NonPerfectableSimpleIntervalBase(
    NonPerfectableIntervalQuality Quality, [NamedEnum] NonPerfectableSimpleIntervalNumber Number) : SimpleIntervalBase
{
    #region Properties And Fields
    /// <inheritdoc/>
    [Positive] public override int NumberValue => (int)Number;

    /// <inheritdoc/>
    public override int HalfSteps => Number.MajorHalfSteps() + Quality.MajorBasedIndex;

    /// <inheritdoc/>
    private protected override int QualityPerfectOrMajorBasedIndex => Quality.MajorBasedIndex;

    private protected override IntervalQuality QualityInternal => Quality;

    /// <summary>
    /// Gets or initializes the quality of the interval represented by the current instance.
    /// </summary>
    public new NonPerfectableIntervalQuality Quality { get; init; } = Quality;

    /// <inheritdoc/>
    public override IntervalPerfectability Perfectability => NonPerfectable;

    /// <summary>
    /// Gets or initializes the number of this interval.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    [NamedEnum] public NonPerfectableSimpleIntervalNumber Number
    {
        get => _number;
        init => _number = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly NonPerfectableSimpleIntervalNumber _number = Throw.IfEnumArgUnnamed(Number, nameof(Number));
    #endregion

    #region Methods
    #region Classification
    /// <inheritdoc/>
    public override bool IsAugmented([NonZero] out int Degree) => Quality.IsAugmented(out Degree);

    /// <inheritdoc/>
    public override bool IsAugmented() => Quality.IsAugmented();

    /// <inheritdoc/>
    public override bool IsDiminished([NonNegative] out int Degree) => Quality.IsDiminished(out Degree);

    /// <inheritdoc/>
    public override bool IsDiminished() => Quality.IsDiminished();
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this object is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NonPerfectableSimpleIntervalBase? other)
        => other is not null && Number == other.Number && Quality == other.Quality;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Quality, Number);
    #endregion

    #region Arithmetic
    private protected override SimpleIntervalBase InversionInternal() => Inversion();

    /// <summary>
    /// Gets the inversion of the <see cref="NonPerfectableSimpleIntervalBase"/> instance passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> was <see langword="null"/>.</exception>
    public static NonPerfectableSimpleIntervalBase operator -(NonPerfectableSimpleIntervalBase value)
        => Throw.IfArgNull(value, nameof(value)).Inversion();

    /// <summary>
    /// Gets a <see cref="NonPerfectableSimpleIntervalBase"/> equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public new NonPerfectableSimpleIntervalBase Inversion() => new(Quality.Inversion(), Number.Inversion());
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(SimpleIntervalBase)} {{ Quality = {Quality}, Number = {Number} }}";
    #endregion
    #endregion
}

/// <summary>
/// Represents a simple interval base that spans less than one octave.
/// </summary>
/// <remarks>
/// This type is used as a base to construct intervals used in this library.
/// <para/>
/// The only possible subclasses of this class are <see cref="PerfectableSimpleIntervalBase"/>
/// and <see cref="NonPerfectableSimpleIntervalBase"/>.
/// </remarks>
public abstract record class SimpleIntervalBase
{
    #region Constants
    /// <summary>
    /// A <see cref="SimpleIntervalBase"/> representing a perfect unison.
    /// </summary>
    public static readonly PerfectableSimpleIntervalBase PerfectUnison = Intervals.Perfect().Unison();
    #endregion

    #region Properties
    /// <summary>
    /// Gets an integer representing the number of the interval.
    /// </summary>
    /// <remarks>
    /// For a simple example, accessing this property on a second will yield 2.
    /// </remarks>
    [Positive] public abstract int NumberValue { get; }

    /// <summary>
    /// Gets the quality of the interval represented by the current instance.
    /// </summary>
    public IntervalQuality Quality => QualityInternal;

    /// <summary>
    /// Allows subclasses to supply their own interval qualities with specific perfectability.
    /// </summary>
    private protected abstract IntervalQuality QualityInternal { get; }

    /// <summary>
    /// Gets the number of half steps spanning the simple interval represented by this object.
    /// </summary>
    public abstract int HalfSteps { get; }

    /// <summary>
    /// Gets the perfectability of the current instance.
    /// </summary>
    public abstract IntervalPerfectability Perfectability { get; }

    /// <summary>
    /// Gets the <see cref="PerfectableIntervalQuality.PerfectBasedIndex"/> or
    /// <see cref="NonPerfectableIntervalQuality.MajorBasedIndex"/> property based on which type of interval
    /// this is.
    /// </summary>
    private protected abstract int QualityPerfectOrMajorBasedIndex { get; }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="SimpleIntervalBase"/> spanning the number of half steps passed in with the simplest
    /// possible interval quality.
    /// </summary>
    /// <param name="halfSteps"></param>
    /// <returns>
    /// The <see cref="SimpleIntervalBase"/> spanning <paramref name="halfSteps"/> half steps with the simplest
    /// possible interval quality, or <see langword="null"/> if <paramref name="halfSteps"/> is equal to 6 (a tritone,
    /// as this case is ambiguous between an augmented fourth and a diminished fifth).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="halfSteps"/> was negative or greater than or equal to 12.
    /// </exception>
    public static SimpleIntervalBase? SimplestQualityWithHalfSteps([NonNegative, LessThanInteger(12)] int halfSteps)
        => halfSteps switch
        {
            0 => PerfectUnison,
            1 => Intervals.Minor().Second(),
            2 => Intervals.Major().Second(),
            3 => Intervals.Minor().Third(),
            4 => Intervals.Major().Third(),
            5 => Intervals.Perfect().Fourth(),
            6 => null,
            7 => Intervals.Perfect().Fifth(),
            8 => Intervals.Minor().Sixth(),
            9 => Intervals.Major().Sixth(),
            10 => Intervals.Minor().Seventh(),
            11 => Intervals.Major().Seventh(),
            _ => throw new ArgumentOutOfRangeException(
                    nameof(halfSteps), halfSteps, $"Parameter must be in the range [0, 11]."),
        };
    #endregion

    #region Classification
    #region Perfectability
    /// <summary>
    /// Gets whether or not this instance represents a perfectable interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Perfectability == Perfectable;

    /// <summary>
    /// Gets whether or not this instance represents a non-perfectable interval.
    /// </summary>
    /// <returns></returns>
    public bool IsNonPerfectable() => Perfectability == NonPerfectable;
    #endregion

    #region Specific Qualities
    /// <summary>
    /// Gets whether or not this interval is augmented, setting <paramref name="Degree"/> to the degree to which it
    /// is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public abstract bool IsAugmented([NonZero] out int Degree);

    /// <summary>
    /// Gets whether or not this interval base is augmented.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsAugmented();

    /// <summary>
    /// Gets whether or not this interval is diminished, setting <paramref name="Degree"/> to the degree to which it
    /// is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public abstract bool IsDiminished([NonZero] out int Degree);

    /// <summary>
    /// Gets whether or not this interval base is diminished.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsDiminished();
    #endregion
    #endregion

    #region Arithmetic
    /// <summary>
    /// Computes the difference between this <see cref="SimpleIntervalBase"/> and another, collapsing the result into a
    /// <see cref="SimpleIntervalBase"/> and setting whether or not the subtraction underflows past a unison in an
    /// <see langword="out"/> parameter.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="underflows"></param>
    /// <returns></returns>
    internal SimpleIntervalBase MinusWithUnderflow(SimpleIntervalBase other, out bool underflows)
    {
        // Subtraction underflows past a unison depending on the difference between the interval numbers
        // Add 1 to the difference, as for example, a third (3) minus a unison (1) is a third (3), not a second (2).
        underflows = NumberValue - other.NumberValue + 1 <= 0;

        return this - other;
    }

    /// <summary>
    /// Subtracts the two <see cref="SimpleIntervalBase"/> instances.
    /// </summary>
    /// <remarks>
    /// This method collapses the result into an instance of <see cref="SimpleIntervalBase"/> by adding or removing
    /// octaves so that the return value is less than an octave.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either <paramref name="lhs"/> or <paramref name="rhs"/> was <see langword="null"/>.
    /// </exception>
    public static SimpleIntervalBase operator -(SimpleIntervalBase lhs, SimpleIntervalBase rhs) => lhs + (-rhs);

    /// <summary>
    /// Computes the sum of this <see cref="SimpleIntervalBase"/> and another, collapsing the result into a
    /// <see cref="SimpleIntervalBase"/> and setting whether or not the addition overflows past an octave in an
    /// <see langword="out"/> parameter.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="overflows"></param>
    /// <returns></returns>
    internal SimpleIntervalBase PlusWithOverflow(SimpleIntervalBase other, out bool overflows)
    {
        // Addition overflows past an octave depending on the sum of the interval numbers
        // Subtract 1 from the sum, as for example, a unison (1) plus a unison (1) is a unison (1), not a second (2). 
        overflows = NumberValue + other.NumberValue - 1 >= 8;

        return this + other;
    }

    /// <summary>
    /// Adds the two <see cref="SimpleIntervalBase"/> instances.
    /// </summary>
    /// <remarks>
    /// This method collapses the result into an instance of <see cref="SimpleIntervalBase"/> by adding or removing
    /// octaves so that the return value is less than an octave.
    /// </remarks>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either <paramref name="lhs"/> or <paramref name="rhs"/> was <see langword="null"/>.
    /// </exception>
    public static SimpleIntervalBase operator +(SimpleIntervalBase lhs, SimpleIntervalBase rhs)
    {
        Throw.IfArgNull(lhs, nameof(lhs));
        Throw.IfArgNull(rhs, nameof(rhs));

        #region Number
        var newUnisonBasedNumberIndex = CircleOfFifths.UnisonBasedNumberIndex(lhs)
                                            + CircleOfFifths.UnisonBasedNumberIndex(rhs);
        var ubni_determinant = newUnisonBasedNumberIndex + 1; // Get rid of the -1 label for fourths
        var qualityShift = Maths.FloorDivRem(ubni_determinant, 7, out ubni_determinant);
        newUnisonBasedNumberIndex = ubni_determinant - 1; // Add back the -1 label for fourths
        #endregion

        #region Quality
        var newQualityIndex = lhs.QualityPerfectOrMajorBasedIndex + rhs.QualityPerfectOrMajorBasedIndex + qualityShift;
        #endregion

        #region Return
        // The perfectability of the number passed in determines what kind of interval base we are creating
        return IntervalNumbers.GetPerfectabilityFromUnisonBasedIndex(newUnisonBasedNumberIndex) switch
        {
            Perfectable => new PerfectableSimpleIntervalBase(
                            PerfectableIntervalQuality.FromPerfectBasedIndex(newQualityIndex),
                            CircleOfFifths.PerfectableNumberFromUnisonBasedIndex(newUnisonBasedNumberIndex)),
            _ => new NonPerfectableSimpleIntervalBase(
                    NonPerfectableIntervalQuality.FromMajorBasedIndex(newQualityIndex),
                    CircleOfFifths.NonPerfectableNumberFromUnisonBasedIndex(newUnisonBasedNumberIndex)),
        };
        #endregion
    }

    /// <summary>
    /// Gets the inversion of the <see cref="SimpleIntervalBase"/> instance passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> was <see langword="null"/>.</exception>
    public static SimpleIntervalBase operator -(SimpleIntervalBase value)
        => Throw.IfArgNull(value, nameof(value)).Inversion();

    /// <summary>
    /// Gets a <see cref="SimpleIntervalBase"/> equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Inversion() => InversionInternal();

    /// <summary>
    /// Allows <see cref="Inversion"/> to be implemented with a covariant return type in subclasses.
    /// </summary>
    /// <returns></returns>
    private protected abstract SimpleIntervalBase InversionInternal();
    #endregion
    #endregion
}
