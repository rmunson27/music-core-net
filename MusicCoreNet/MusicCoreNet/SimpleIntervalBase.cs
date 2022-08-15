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
    [NamedEnum] PerfectableIntervalQuality Quality, PerfectableSimpleIntervalNumber Number) : SimpleIntervalBase
{
    /// <inheritdoc/>
    [Positive] public override int NumberValue => (int)Number;

    /// <inheritdoc/>
    private protected override int QualityPerfectOrMajorBasedIndex => Quality.PerfectBasedIndex;

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

    /// <inheritdoc/>
    public override bool IsAugmented([NonZero] out int Degree) => Quality.IsAugmented(out Degree);

    /// <inheritdoc/>
    public override bool IsAugmented() => Quality.IsAugmented();

    /// <inheritdoc/>
    public override bool IsDiminished([NonNegative] out int Degree) => Quality.IsDiminished(out Degree);

    /// <inheritdoc/>
    public override bool IsDiminished() => Quality.IsDiminished();

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
    [NamedEnum] NonPerfectableIntervalQuality Quality, NonPerfectableSimpleIntervalNumber Number) : SimpleIntervalBase
{
    /// <inheritdoc/>
    [Positive] public override int NumberValue => (int)Number;

    /// <inheritdoc/>
    private protected override int QualityPerfectOrMajorBasedIndex => Quality.MajorBasedIndex;

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

    /// <inheritdoc/>
    public override bool IsAugmented([NonZero] out int Degree) => Quality.IsAugmented(out Degree);

    /// <inheritdoc/>
    public override bool IsAugmented() => Quality.IsAugmented();

    /// <inheritdoc/>
    public override bool IsDiminished([NonNegative] out int Degree) => Quality.IsDiminished(out Degree);

    /// <inheritdoc/>
    public override bool IsDiminished() => Quality.IsDiminished();

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
    #region Properties
    /// <summary>
    /// Gets an integer representing the number of the interval.
    /// </summary>
    /// <remarks>
    /// For a simple example, accessing this property on a second will yield 2.
    /// </remarks>
    [Positive] public abstract int NumberValue { get; }

    /// <summary>
    /// Gets the <see cref="PerfectableIntervalQuality.PerfectBasedIndex"/> or
    /// <see cref="NonPerfectableIntervalQuality.MajorBasedIndex"/> property based on which type of interval
    /// this is.
    /// </summary>
    private protected abstract int QualityPerfectOrMajorBasedIndex { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Prevents this class from being extended outside of this assembly.
    /// </summary>
    private protected SimpleIntervalBase() { }
    #endregion

    #region Methods
    #region Classification
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

    #region Arithmetic
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
