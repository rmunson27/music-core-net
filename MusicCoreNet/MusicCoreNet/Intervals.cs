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
/// Static functionality relating to intervals.
/// </summary>
public static class Intervals
{
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
}

/// <summary>
/// Represents a musical interval between two notes.
/// </summary>
/// <param name="Base">An object defining the quality and base number of the interval.</param>
/// <param name="AdditionalOctaves">
/// The number of additional octaves added onto the base to make up the interval.
/// </param>
public readonly record struct Interval(
    SimpleIntervalBase Base, [NonNegative] int AdditionalOctaves)
    : IDefaultableStruct
{
    #region Properties And Fields
    /// <inheritdoc/>
    public bool IsDefault => _base is null;

    /// <summary>
    /// Gets the number of this interval.
    /// </summary>
    [DoesNotReturnIfInstanceDefault]
    [Positive] public int Number => Base.NumberValue + AdditionalOctaves * 7;

    /// <summary>
    /// Gets the quality of this interval.
    /// </summary>
    [DoesNotReturnIfInstanceDefault]
    public IntervalQuality Quality => Base.Quality;

    /// <summary>
    /// Gets the perfectability of this interval.
    /// </summary>
    [DoesNotReturnIfInstanceDefault]
    public IntervalPerfectability Perfectability => Base.Perfectability;

    /// <summary>
    /// Gets or initializes the base defining the quality and base number of this interval.
    /// </summary>
    /// <exception cref="PropertySetNullException">This property was initialized to <see langword="null"/>.</exception>
    [MaybeDefaultIfInstanceDefault]
    public SimpleIntervalBase Base
    {
        get => _base;
        init => _base = Throw.IfPropSetNull(value);
    }
    private readonly SimpleIntervalBase _base = Throw.IfArgNull(Base, nameof(Base));

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
    #endregion
}
