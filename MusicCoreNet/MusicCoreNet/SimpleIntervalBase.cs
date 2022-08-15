using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
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
    private readonly PerfectableIntervalNumber _number = Throw.IfEnumArgUnnamed(Number, nameof(Number));

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
    private readonly NonPerfectableIntervalNumber _number = Throw.IfEnumArgUnnamed(Number, nameof(Number));

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
}

/// <summary>
/// Represents a simple interval base that spans less than one octave.
/// </summary>
/// <remarks>
/// This type is used as a base to construct intervals used in this library.
/// </remarks>
public abstract record class SimpleIntervalBase
{
    /// <summary>
    /// Gets an integer representing the number of the interval.
    /// </summary>
    /// <remarks>
    /// For a simple example, accessing this property on a second will yield 2.
    /// </remarks>
    [Positive] public abstract int NumberIntValue { get; }

    /// <summary>
    /// Prevents this class from being extended outside of this assembly.
    /// </summary>
    private protected SimpleIntervalBase() { }

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
}
