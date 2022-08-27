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
/// <param name="Base">An object defining the quality and base number of the interval.</param>
/// <param name="AdditionalOctaves">
/// The number of additional octaves added onto the base to make up the interval.
/// </param>
public readonly record struct Interval(SimpleIntervalBase Base, [NonNegative] int AdditionalOctaves)
{
    #region Constants
    /// <summary>
    /// An <see cref="Interval"/> representing a perfect unison.
    /// </summary>
    public static readonly Interval PerfectUnison = SimpleIntervalBase.PerfectUnison;

    /// <summary>
    /// An <see cref="Interval"/> representing a perfect octave.
    /// </summary>
    public static readonly Interval PerfectOctave = new(SimpleIntervalBase.PerfectUnison, AdditionalOctaves: 1);
    #endregion

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
        => new(SimpleIntervalBase.Create(Quality, Number.Base), Number.AdditionalOctaves);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Interval other) => AdditionalOctaves == other.AdditionalOctaves && Base == other.Base;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Base, AdditionalOctaves);
    #endregion

    #region Computation
    /// <summary>
    /// Gets an <see cref="Interval"/> equivalent to this instance with the quality shifted by the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public Interval WithQualityShift(int Degree) => this with { Base = Base.WithQualityShiftedBy(Degree) };
    #endregion

    #region Arithmetic
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
        out SimpleIntervalBase newBase, out int newAdditionalOctaves)
    {
        newBase = lhs.Base.MinusWithUnderflow(rhs.Base, out var baseAdditionUnderflows);

        newAdditionalOctaves = lhs.AdditionalOctaves - rhs.AdditionalOctaves;
        if (baseAdditionUnderflows) newAdditionalOctaves--;
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="SimpleIntervalBase"/> to an <see cref="Interval"/>.
    /// </summary>
    /// <param name="base"></param>
    public static implicit operator Interval(SimpleIntervalBase @base) => new(@base, AdditionalOctaves: 0);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(Interval)} {{ Quality = {Quality}, Number = {Number} }}";
    #endregion

    #region Builder
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
    #endregion
    #endregion
}
