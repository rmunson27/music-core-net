using Rem.Core.Attributes;
using Rem.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the perfectability of an interval (either perfectable or imperfectable).
/// </summary>
/// <remarks>
/// The default value of this struct represents perfectable intervals.
/// </remarks>
public readonly record struct IntervalPerfectability
{
    /// <inheritdoc cref="Values.Perfectable"/>
    /// <remarks>
    /// This is the default value of this struct.
    /// </remarks>
    public static readonly IntervalPerfectability Perfectable = new(Values.Perfectable);

    /// <inheritdoc cref="Values.Imperfectable"/>
    public static readonly IntervalPerfectability Imperfectable = new(Values.Imperfectable);

    /// <summary>
    /// Gets a unique identifier for this value.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance of this struct.
    /// </summary>
    /// <param name="Value"></param>
    private IntervalPerfectability([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Implicitly converts a <see cref="Values"/> instance to the value it represents.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidCastException">The value was undefined.</exception>
    public static implicit operator IntervalPerfectability([NameableEnum] Values value)
        => Enums.IsDefined(value)
            ? new(value)
            : throw new InvalidCastException("Invalid unnamed interval perfectability value.");

    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Represents the possible values of this struct.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents perfectable intervals (i.e. a perfect fourth).
        /// </summary>
        Perfectable,

        /// <summary>
        /// Represents imperfectable intervals (i.e. a major second).
        /// </summary>
        Imperfectable,
    }
}

