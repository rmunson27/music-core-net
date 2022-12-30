using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using BaseValues = IntervalQualityKind.Values;

/// <summary>
/// Represents the kind of peripheral interval qualities (augmented or diminished).
/// </summary>
/// <remarks>
/// The default instance of this struct represents augmented intervals.
/// </remarks>
public readonly record struct PeripheralIntervalQualityKind
{
    #region Constants
    /// <summary>
    /// The number of distinct <see cref="PeripheralIntervalQualityKind"/> values.
    /// </summary>
    public const int ValuesCount = 2;

    /// <summary>
    /// The offset to subtract from <see cref="Value"/> to get the value of the
    /// corresponding <see cref="IntervalQualityKind"/> value.
    /// </summary>
    /// <remarks>
    /// This makes <see cref="Augmented"/> the default.
    /// </remarks>
    internal const sbyte ValueOffset = -(sbyte)BaseValues.Augmented;

    /// <inheritdoc cref="Values.Augmented"/>
    public static readonly PeripheralIntervalQualityKind Augmented = new(Values.Augmented);

    /// <inheritdoc cref="Values.Diminished"/>
    public static readonly PeripheralIntervalQualityKind Diminished = new(Values.Diminished);
    #endregion

    /// <summary>
    /// Gets this instance uniquely represented as an <see langword="enum"/> value.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    internal PeripheralIntervalQualityKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    #region Conversions
    /// <summary>
    /// Explicitly converts an <see cref="IntervalQualityKind"/> to a <see cref="PeripheralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQualityKind(IntervalQualityKind kind) => kind.Value switch
    {
        BaseValues.Augmented or BaseValues.Diminished => new((Values)unchecked(kind.Value + ValueOffset)),
        _ => throw NotPeripheral(kind.ToString()),
    };

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQualityKind"/> to
    /// a <see cref="PeripheralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQualityKind(PerfectableIntervalQualityKind kind)
        => kind.Value == PerfectableIntervalQualityKind.Values.Perfect
            ? throw NotPeripheral(nameof(PerfectableIntervalQualityKind.Perfect))
            : kind.AsPeripheralUnsafe();

    /// <summary>
    /// Explicitly converts an <see cref="ImperfectableIntervalQualityKind"/> to
    /// a <see cref="PeripheralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQualityKind(ImperfectableIntervalQualityKind kind)
        => kind.Value switch
        {
            ImperfectableIntervalQualityKind.Values.Augmented or ImperfectableIntervalQualityKind.Values.Diminished
                => kind.AsPeripheralUnsafe(),

            _ => throw NotPeripheral(kind.ToString()),
        };

    /// <summary>
    /// Returns an exception describing the kind with name <paramref name="kindName"/> as not peripheral.
    /// </summary>
    /// <param name="kindName"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InvalidCastException NotPeripheral(string kindName)
        => new($"{kindName} interval quality kind is not peripheral.");
    #endregion

    /// <summary>
    /// Represents all possible values of type <see cref="PeripheralIntervalQualityKind"/> as
    /// <see langword="enum"/> values.
    /// </summary>
    public enum Values : sbyte
    {
        /// <inheritdoc cref="BaseValues.Augmented"/>
        Augmented = BaseValues.Augmented + ValueOffset,

        /// <inheritdoc cref="BaseValues.Diminished"/>
        Diminished = BaseValues.Diminished + ValueOffset,
    }
}

/// <summary>
/// Represents the kind of one of the three central interval qualities (minor, perfect or major).
/// </summary>
/// <remarks>
/// The default instance of this struct represents the perfect interval quality.
/// </remarks>
public readonly record struct CentralIntervalQualityKind
{
    #region Constants
    /// <summary>
    /// The number of distinct <see cref="CentralIntervalQualityKind"/> values.
    /// </summary>
    public const int ValuesCount = 3;

    /// <inheritdoc cref="Values.Major"/>
    public static readonly CentralIntervalQualityKind Major = new(Values.Major);

    /// <inheritdoc cref="Values.Perfect"/>
    /// <remarks>
    /// This is the default value of its type.
    /// </remarks>
    public static readonly CentralIntervalQualityKind Perfect = new(Values.Perfect);

    /// <inheritdoc cref="Values.Minor"/>
    public static readonly CentralIntervalQualityKind Minor = new(Values.Minor);
    #endregion

    /// <summary>
    /// Gets this instance uniquely represented as an <see langword="enum"/> value.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    internal CentralIntervalQualityKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    #region Conversions
    #region Operators
    /// <summary>
    /// Explicitly converts an <see cref="IntervalQualityKind"/> to a <see cref="CentralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQualityKind(IntervalQualityKind kind) => kind.Value switch
    {
        BaseValues.Augmented or BaseValues.Diminished => throw NotCentral(kind.ToString()),
        _ => kind.AsCentralUnsafe(),
    };

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQualityKind"/> to
    /// a <see cref="CentralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQualityKind(PerfectableIntervalQualityKind kind)
        => kind.Value == PerfectableIntervalQualityKind.Values.Perfect
            ? Perfect
            : throw NotCentral(kind.ToString());

    /// <summary>
    /// Explicitly converts an <see cref="ImperfectableIntervalQualityKind"/> to
    /// a <see cref="CentralIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQualityKind(ImperfectableIntervalQualityKind kind)
        => kind.Value switch
        {
            ImperfectableIntervalQualityKind.Values.Augmented or ImperfectableIntervalQualityKind.Values.Diminished
                => throw NotCentral(kind.ToString()),

            _ => kind.AsCentralUnsafe(),
        };

    /// <summary>
    /// Returns an exception describing the kind with name <paramref name="kindName"/> as not central.
    /// </summary>
    /// <param name="kindName"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InvalidCastException NotCentral(string kindName)
        => new($"{kindName} interval quality kind is not central.");
    #endregion

    #region IsType
    /// <param name="imperfectableKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ImperfectableIntervalQualityKind"/> if
    /// this instance is not perfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is perfectable.
    /// </param>
    /// <inheritdoc cref="IsPerfectable(out PerfectableIntervalQualityKind)"/>
    public bool IsPerfectable(
        out PerfectableIntervalQualityKind kind, out ImperfectableIntervalQualityKind imperfectableKind)
        => IsPerfectable()
            ? Try.Success(successOut: out kind, PerfectableIntervalQualityKind.Perfect,
                          failureOut: out imperfectableKind)
            : Try.Failure(successOut: out kind, failureOut: out imperfectableKind, AsImperfectableUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PerfectableIntervalQualityKind"/>
    /// (<see cref="PerfectableIntervalQualityKind.Perfect"/>) if this instance is perfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not perfectable; however, as the default
    /// value of type <see cref="PerfectableIntervalQualityKind"/>
    /// is <see cref="PerfectableIntervalQualityKind.Perfect"/>, it will always be set to that value.
    /// </param>
    /// <inheritdoc cref="IsPerfectable()"/>
    public bool IsPerfectable(out PerfectableIntervalQualityKind kind)
        => IsPerfectable() ? Try.Success(out kind, PerfectableIntervalQualityKind.Perfect) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a perfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a perfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPerfectable() => Value is Values.Perfect;

    /// <param name="perfectableKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PerfectableIntervalQualityKind"/>
    /// (<see cref="PerfectableIntervalQualityKind.Perfect"/>) if this instance is not imperfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is imperfectable; however, as the default
    /// value of type <see cref="PerfectableIntervalQualityKind"/>
    /// is <see cref="PerfectableIntervalQualityKind.Perfect"/>, it will always be set to that value.
    /// </param>
    /// <inheritdoc cref="IsImperfectable(out ImperfectableIntervalQualityKind)"/>
    public bool IsImperfectable(
        out ImperfectableIntervalQualityKind kind, out PerfectableIntervalQualityKind perfectableKind)
        => IsImperfectable()
            ? Try.Success(successOut: out kind, AsImperfectableUnsafe(), failureOut: out perfectableKind)
            : Try.Failure(successOut: out kind,
                          failureOut: out perfectableKind, PerfectableIntervalQualityKind.Perfect);

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ImperfectableIntervalQualityKind"/> if
    /// this instance is imperfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not imperfectable.
    /// </param>
    /// <inheritdoc cref="IsImperfectable()"/>
    public bool IsImperfectable(out ImperfectableIntervalQualityKind kind)
        => IsImperfectable() ? Try.Success(out kind, AsImperfectableUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is an imperfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is an imperfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsImperfectable() => Value is not Values.Perfect;
    #endregion

    #region Unsafe
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ImperfectableIntervalQualityKind AsImperfectableUnsafe()
        => new((ImperfectableIntervalQualityKind.Values)
                unchecked(Value + ImperfectableIntervalQualityKind.ValueOffset));
    #endregion
    #endregion

    /// <summary>
    /// Represents all possible values of type <see cref="CentralIntervalQualityKind"/> as
    /// <see langword="enum"/> values.
    /// </summary>
    public enum Values : sbyte
    {
        /// <inheritdoc cref="BaseValues.Minor"/>
        Minor = BaseValues.Minor,

        /// <inheritdoc cref="BaseValues.Perfect"/>
        Perfect = BaseValues.Perfect,

        /// <inheritdoc cref="BaseValues.Major"/>
        Major = BaseValues.Major,
    }
}

/// <summary>
/// Represents the kind of an imperfectable interval quality (perfect, diminished, etc).
/// </summary>
/// <remarks>
/// The default instance of this struct represents the perfect interval quality.
/// </remarks>
public readonly record struct PerfectableIntervalQualityKind
{
    #region Constants
    /// <summary>
    /// The number of distinct <see cref="PerfectableIntervalQualityKind"/> values.
    /// </summary>
    public const int ValuesCount = 3;

    /// <inheritdoc cref="Values.Augmented"/>
    public static readonly PerfectableIntervalQualityKind Augmented = new(Values.Augmented);

    /// <inheritdoc cref="Values.Perfect"/>
    /// <remarks>
    /// This is the default value of its type.
    /// </remarks>
    public static readonly PerfectableIntervalQualityKind Perfect = new(Values.Perfect);

    /// <inheritdoc cref="Values.Diminished"/>
    public static readonly PerfectableIntervalQualityKind Diminished = new(Values.Diminished);
    #endregion

    /// <summary>
    /// Gets this instance uniquely represented as an <see langword="enum"/> value.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    internal PerfectableIntervalQualityKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    #region Conversions
    #region Operators
    /// <summary>
    /// Explicitly converts an <see cref="IntervalQualityKind"/> to a <see cref="PerfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQualityKind(IntervalQualityKind kind) => kind.Value switch
    {
        BaseValues.Minor or BaseValues.Major => throw NotPerfectable(kind.ToString()),
        _ => kind.AsPerfectableUnsafe(),
    };

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQualityKind"/> to
    /// a <see cref="PerfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator PerfectableIntervalQualityKind(PeripheralIntervalQualityKind kind)
        => new((Values)unchecked(kind.Value - PeripheralIntervalQualityKind.ValueOffset));

    /// <summary>
    /// Explicitly converts a <see cref="CentralIntervalQualityKind"/> to
    /// a <see cref="PerfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQualityKind(CentralIntervalQualityKind kind)
        => kind.Value == CentralIntervalQualityKind.Values.Perfect
            ? Perfect
            : throw NotPerfectable(kind.ToString());

    /// <summary>
    /// Explicitly converts a <see cref="ImperfectableIntervalQualityKind"/> to
    /// a <see cref="PerfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQualityKind(ImperfectableIntervalQualityKind kind)
        => kind.Value switch
        {
            ImperfectableIntervalQualityKind.Values.Minor or ImperfectableIntervalQualityKind.Values.Major
                => throw NotPerfectable(kind.ToString()),

            _ => kind.AsPerfectableUnsafe(),
        };

    /// <summary>
    /// Returns an exception describing the kind with name <paramref name="kindName"/> as not perfectable.
    /// </summary>
    /// <param name="kindName"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InvalidCastException NotPerfectable(string kindName)
        => new($"{kindName} interval quality kind is not perfectable.");
    #endregion

    #region IsType
    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ImperfectableIntervalQualityKind"/> if
    /// this instance is imperfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not imperfectable.
    /// </param>
    /// <inheritdoc cref="IsImperfectable()"/>
    public bool IsImperfectable(out ImperfectableIntervalQualityKind kind)
        => IsImperfectable() ? Try.Success(out kind, AsImperfectableUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is an imperfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is an imperfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsImperfectable() => Value is not Values.Perfect;

    /// <param name="peripheralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is not central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is central.
    /// </param>
    /// <inheritdoc cref="IsCentral(out CentralIntervalQualityKind)"/>
    public bool IsCentral(
        out CentralIntervalQualityKind kind, out PeripheralIntervalQualityKind peripheralKind)
        => IsCentral()
            ? Try.Success(successOut: out kind, CentralIntervalQualityKind.Perfect, failureOut: out peripheralKind)
            : Try.Failure(successOut: out kind, failureOut: out peripheralKind, AsPeripheralUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/>
    /// (<see cref="CentralIntervalQualityKind.Perfect"/>) if this instance is central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not central; however, as the default
    /// value of type <see cref="CentralIntervalQualityKind"/> is <see cref="CentralIntervalQualityKind.Perfect"/>,
    /// it will always be set to that value.
    /// </param>
    /// <inheritdoc cref="IsCentral()"/>
    public bool IsCentral(out CentralIntervalQualityKind kind)
        => IsCentral() ? Try.Success(out kind, CentralIntervalQualityKind.Perfect) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a central interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a central interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsCentral() => Value is Values.Perfect;

    /// <param name="centralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/>
    /// (<see cref="CentralIntervalQualityKind.Perfect"/>) if this instance is not peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is peripheral; however, as the default
    /// value of type <see cref="CentralIntervalQualityKind"/> is <see cref="CentralIntervalQualityKind.Perfect"/>,
    /// it will always be set to that value.
    /// </param>
    /// <inheritdoc cref="IsPeripheral(out PeripheralIntervalQualityKind)"/>
    public bool IsPeripheral(out PeripheralIntervalQualityKind kind, out CentralIntervalQualityKind centralKind)
        => IsPeripheral()
            ? Try.Success(successOut: out kind, AsPeripheralUnsafe(), failureOut: out centralKind)
            : Try.Failure(successOut: out kind, failureOut: out centralKind, CentralIntervalQualityKind.Perfect);

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not peripheral.
    /// </param>
    /// <inheritdoc cref="IsPeripheral()"/>
    public bool IsPeripheral(out PeripheralIntervalQualityKind kind)
        => IsPeripheral() ? Try.Success(out kind, AsPeripheralUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a peripheral interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a peripheral interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPeripheral() => Value is Values.Augmented or Values.Diminished;
    #endregion

    #region Unsafe
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ImperfectableIntervalQualityKind AsImperfectableUnsafe()
        => new((ImperfectableIntervalQualityKind.Values)
                unchecked(Value + ImperfectableIntervalQualityKind.ValueOffset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal PeripheralIntervalQualityKind AsPeripheralUnsafe()
        => new((PeripheralIntervalQualityKind.Values)
                unchecked(Value + PeripheralIntervalQualityKind.ValueOffset));
    #endregion
    #endregion

    /// <summary>
    /// Represents all possible values of the <see cref="PerfectableIntervalQualityKind"/> type as
    /// <see langword="enum"/> values.
    /// </summary>
    public enum Values : sbyte
    {
        /// <inheritdoc cref="BaseValues.Augmented"/>
        Augmented = BaseValues.Augmented,

        /// <inheritdoc cref="BaseValues.Perfect"/>
        Perfect = BaseValues.Perfect,

        /// <inheritdoc cref="BaseValues.Diminished"/>
        Diminished = BaseValues.Diminished,
    }
}

/// <summary>
/// Represents the kind of an imperfectable interval quality (major, diminished, etc).
/// </summary>
/// <remarks>
/// The default instance of this struct represents the major interval quality.
/// </remarks>
public readonly record struct ImperfectableIntervalQualityKind
{
    #region Constants
    /// <summary>
    /// The number of distinct <see cref="ImperfectableIntervalQualityKind"/> values.
    /// </summary>
    public const int ValuesCount = 4;

    /// <summary>
    /// The offset to subtract from <see cref="Value"/> to get the value of the
    /// corresponding <see cref="IntervalQualityKind"/> value.
    /// </summary>
    /// <remarks>
    /// This makes <see cref="Major"/> the default.
    /// </remarks>
    internal const sbyte ValueOffset = -(sbyte)BaseValues.Major;

    /// <inheritdoc cref="Values.Augmented"/>
    public static readonly ImperfectableIntervalQualityKind Augmented = new(Values.Augmented);

    /// <inheritdoc cref="Values.Major"/>
    /// <remarks>
    /// This is the default value of this type.
    /// </remarks>
    public static readonly ImperfectableIntervalQualityKind Major = new(Values.Major);

    /// <inheritdoc cref="Values.Minor"/>
    public static readonly ImperfectableIntervalQualityKind Minor = new(Values.Minor);

    /// <inheritdoc cref="Values.Diminished"/>
    public static readonly ImperfectableIntervalQualityKind Diminished = new(Values.Diminished);
    #endregion

    /// <inheritdoc cref="IntervalQualityKind.Value"/>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    internal ImperfectableIntervalQualityKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    #region Conversions
    #region Operators
    /// <summary>
    /// Explicitly converts an <see cref="IntervalQualityKind"/> to an <see cref="ImperfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableIntervalQualityKind(IntervalQualityKind kind)
        => kind.Value == BaseValues.Perfect
            ? throw NotImperfectable(kind.ToString())
            : kind.AsImperfectableUnsafe();

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQualityKind"/> to
    /// an <see cref="ImperfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator ImperfectableIntervalQualityKind(PeripheralIntervalQualityKind kind)
        => new((Values)unchecked(kind.Value - PeripheralIntervalQualityKind.ValueOffset + ValueOffset));

    /// <summary>
    /// Explicitly converts a <see cref="CentralIntervalQualityKind"/> to
    /// an <see cref="ImperfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableIntervalQualityKind(CentralIntervalQualityKind kind)
        => kind.IsImperfectable(out var ik)
            ? ik
            : throw NotImperfectable(nameof(PerfectableIntervalQualityKind.Perfect));

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQualityKind"/> to
    /// an <see cref="ImperfectableIntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableIntervalQualityKind(PerfectableIntervalQualityKind kind)
        => kind.IsImperfectable(out var ik)
            ? ik
            : throw NotImperfectable(nameof(PerfectableIntervalQualityKind.Perfect));

    /// <summary>
    /// Returns an exception describing the kind with name <paramref name="kindName"/> as not imperfectable.
    /// </summary>
    /// <param name="kindName"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static InvalidCastException NotImperfectable(string kindName)
        => new($"{kindName} interval quality kind is not imperfectable.");
    #endregion

    #region IsType
    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PerfectableIntervalQualityKind"/> if
    /// this instance is perfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not perfectable.
    /// </param>
    /// <inheritdoc cref="IsPerfectable()"/>
    public bool IsPerfectable(out PerfectableIntervalQualityKind kind)
        => IsPerfectable() ? Try.Success(out kind, AsPerfectableUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a perfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a perfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPerfectable() => Value is Values.Augmented or Values.Diminished;

    /// <param name="peripheralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is not central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is central.
    /// </param>
    /// <inheritdoc cref="IsCentral(out CentralIntervalQualityKind)"/>
    public bool IsCentral(
        out CentralIntervalQualityKind kind, out PeripheralIntervalQualityKind peripheralKind)
        => IsCentral()
            ? Try.Success(successOut: out kind, AsCentralUnsafe(), failureOut: out peripheralKind)
            : Try.Failure(successOut: out kind, failureOut: out peripheralKind, AsPeripheralUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/> if
    /// this instance is central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not central.
    /// </param>
    /// <inheritdoc cref="IsCentral()"/>
    public bool IsCentral(out CentralIntervalQualityKind kind)
        => IsCentral() ? Try.Success(out kind, AsCentralUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a central interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a central interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsCentral() => Value is Values.Major or Values.Minor;

    /// <param name="centralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/> if
    /// this instance is not peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is peripheral.
    /// </param>
    /// <inheritdoc cref="IsPeripheral(out PeripheralIntervalQualityKind)"/>
    public bool IsPeripheral(
        out PeripheralIntervalQualityKind kind, out CentralIntervalQualityKind centralKind)
        => IsPeripheral()
            ? Try.Success(successOut: out kind, AsPeripheralUnsafe(), failureOut: out centralKind)
            : Try.Failure(successOut: out kind, failureOut: out centralKind, AsCentralUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not peripheral.
    /// </param>
    /// <inheritdoc cref="IsPeripheral()"/>
    public bool IsPeripheral(out PeripheralIntervalQualityKind kind)
        => IsPeripheral() ? Try.Success(out kind, AsPeripheralUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a peripheral interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a peripheral interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPeripheral() => Value is Values.Augmented or Values.Diminished;
    #endregion

    #region Unsafe
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal PerfectableIntervalQualityKind AsPerfectableUnsafe()
        => new((PerfectableIntervalQualityKind.Values)unchecked(Value - ValueOffset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal CentralIntervalQualityKind AsCentralUnsafe()
        => new((CentralIntervalQualityKind.Values)unchecked(Value - ValueOffset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal PeripheralIntervalQualityKind AsPeripheralUnsafe()
        => new((PeripheralIntervalQualityKind.Values)
                unchecked(Value - ValueOffset + PeripheralIntervalQualityKind.ValueOffset));
    #endregion
    #endregion

    /// <summary>
    /// Represents all possible values of the <see cref="ImperfectableIntervalQualityKind"/> type as
    /// <see langword="enum"/> values.
    /// </summary>
    public enum Values : sbyte
    {
        /// <inheritdoc cref="BaseValues.Augmented"/>
        Augmented = BaseValues.Augmented + ValueOffset,

        /// <inheritdoc cref="BaseValues.Major"/>
        Major = BaseValues.Major + ValueOffset,

        /// <inheritdoc cref="BaseValues.Minor"/>
        Minor = BaseValues.Minor + ValueOffset,

        /// <inheritdoc cref="BaseValues.Diminished"/>
        Diminished = BaseValues.Diminished + ValueOffset,
    }
}

/// <summary>
/// Represents the kind of an interval quality (major, perfect, diminished, etc).
/// </summary>
/// <remarks>
/// The default value of this struct represents the perfect interval quality.
/// </remarks>
public readonly record struct IntervalQualityKind
{
    #region Constants
    /// <summary>
    /// The number of distinct <see cref="IntervalQualityKind"/> values.
    /// </summary>
    public const int ValuesCount = 5;

    /// <inheritdoc cref="Values.Augmented"/>
    public static readonly IntervalQualityKind Augmented = new(Values.Augmented);

    /// <inheritdoc cref="Values.Major"/>
    public static readonly IntervalQualityKind Major = new(Values.Major);

    /// <inheritdoc cref="Values.Perfect"/>
    /// <remarks>
    /// This is the default value of its type.
    /// </remarks>
    public static readonly IntervalQualityKind Perfect = new(Values.Perfect);

    /// <inheritdoc cref="Values.Minor"/>
    public static readonly IntervalQualityKind Minor = new(Values.Minor);

    /// <inheritdoc cref="Values.Diminished"/>
    public static readonly IntervalQualityKind Diminished = new(Values.Diminished);
    #endregion

    /// <summary>
    /// Gets this instance uniquely represented as an <see langword="enum"/> value.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    private IntervalQualityKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    #region Conversions
    #region Operators
    /// <summary>
    /// Implicitly converts an <see cref="ImperfectableIntervalQualityKind"/> to an <see cref="IntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator IntervalQualityKind(ImperfectableIntervalQualityKind kind)
        => new((Values)unchecked(kind.Value - ImperfectableIntervalQualityKind.ValueOffset));

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableIntervalQualityKind"/> to an <see cref="IntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator IntervalQualityKind(PerfectableIntervalQualityKind kind)
        => new((Values)kind.Value);

    /// <summary>
    /// Implicitly converts a <see cref="CentralIntervalQualityKind"/> to an <see cref="IntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator IntervalQualityKind(CentralIntervalQualityKind kind) => new((Values)kind.Value);

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQualityKind"/> to an <see cref="IntervalQualityKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator IntervalQualityKind(PeripheralIntervalQualityKind kind)
        => new((Values)unchecked(kind.Value - PeripheralIntervalQualityKind.ValueOffset));
    #endregion

    #region IsType
    /// <param name="imperfectableKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ImperfectableIntervalQualityKind"/> if
    /// this instance is not perfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is perfectable.
    /// </param>
    /// <inheritdoc cref="IsPerfectable(out PerfectableIntervalQualityKind)"/>
    public bool IsPerfectable(
        out PerfectableIntervalQualityKind kind, out ImperfectableIntervalQualityKind imperfectableKind)
        => IsPerfectable()
            ? Try.Success(successOut: out kind, AsPerfectableUnsafe(), failureOut: out imperfectableKind)
            : Try.Failure(successOut: out kind, failureOut: out imperfectableKind, AsImperfectableUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PerfectableIntervalQualityKind"/> if
    /// this instance is perfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not perfectable.
    /// </param>
    /// <inheritdoc cref="IsPerfectable()"/>
    public bool IsPerfectable(out PerfectableIntervalQualityKind kind)
        => IsPerfectable() ? Try.Success(out kind, AsPerfectableUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a perfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a perfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPerfectable() => Value is not (Values.Minor or Values.Major);

    /// <param name="perfectableKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PerfectableIntervalQualityKind"/>
    /// (<see cref="PerfectableIntervalQualityKind.Perfect"/>) if this instance is not imperfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is imperfectable; however, as the default
    /// value of type <see cref="PerfectableIntervalQualityKind"/> is
    /// <see cref="PerfectableIntervalQualityKind.Perfect"/>, it will always be set to that value.
    /// </param>
    /// <inheritdoc cref="IsImperfectable(out ImperfectableIntervalQualityKind)"/>
    public bool IsImperfectable(
        out ImperfectableIntervalQualityKind kind, out PerfectableIntervalQualityKind perfectableKind)
        => IsImperfectable()
            ? Try.Success(successOut: out kind, AsImperfectableUnsafe(), failureOut: out perfectableKind)
            : Try.Failure(successOut: out kind,
                          failureOut: out perfectableKind, PerfectableIntervalQualityKind.Perfect);

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ImperfectableIntervalQualityKind"/> if
    /// this instance is imperfectable.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not imperfectable.
    /// </param>
    /// <inheritdoc cref="IsImperfectable()"/>
    public bool IsImperfectable(out ImperfectableIntervalQualityKind kind)
        => IsImperfectable() ? Try.Success(out kind, AsImperfectableUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is an imperfectable interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is an imperfectable interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsImperfectable() => Value is not Values.Perfect;

    /// <param name="peripheralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is not central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is central.
    /// </param>
    /// <inheritdoc cref="IsCentral(out CentralIntervalQualityKind)"/>
    public bool IsCentral(out CentralIntervalQualityKind kind, out PeripheralIntervalQualityKind peripheralKind)
        => IsCentral()
            ? Try.Success(successOut: out kind, AsCentralUnsafe(), failureOut: out peripheralKind)
            : Try.Failure(successOut: out kind, failureOut: out peripheralKind, AsPeripheralUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/> if
    /// this instance is central.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not central.
    /// </param>
    /// <inheritdoc cref="IsCentral()"/>
    public bool IsCentral(out CentralIntervalQualityKind kind)
        => IsCentral() ? Try.Success(out kind, AsCentralUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a central interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a central interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsCentral() => Value is not (Values.Diminished or Values.Augmented);

    /// <param name="centralKind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="CentralIntervalQualityKind"/> if
    /// this instance is not peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is peripheral.
    /// </param>
    /// <inheritdoc cref="IsPeripheral(out PeripheralIntervalQualityKind)"/>
    public bool IsPeripheral(out PeripheralIntervalQualityKind kind, out CentralIntervalQualityKind centralKind)
        => IsPeripheral()
            ? Try.Success(successOut: out kind, AsPeripheralUnsafe(), failureOut: out centralKind)
            : Try.Failure(successOut: out kind, failureOut: out centralKind, AsCentralUnsafe());

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="PeripheralIntervalQualityKind"/> if
    /// this instance is peripheral.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance is not peripheral.
    /// </param>
    /// <inheritdoc cref="IsPeripheral()"/>
    public bool IsPeripheral(out PeripheralIntervalQualityKind kind)
        => IsPeripheral() ? Try.Success(out kind, AsPeripheralUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Returns whether or not this instance is a peripheral interval quality type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a peripheral interval quality type, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsPeripheral() => Value is Values.Augmented or Values.Diminished;
    #endregion

    #region Unsafe
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal PerfectableIntervalQualityKind AsPerfectableUnsafe() => new((PerfectableIntervalQualityKind.Values)Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ImperfectableIntervalQualityKind AsImperfectableUnsafe()
        => new((ImperfectableIntervalQualityKind.Values)
                unchecked(Value + ImperfectableIntervalQualityKind.ValueOffset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal CentralIntervalQualityKind AsCentralUnsafe() => new((CentralIntervalQualityKind.Values)Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal PeripheralIntervalQualityKind AsPeripheralUnsafe()
        => new((PeripheralIntervalQualityKind.Values)unchecked(Value + PeripheralIntervalQualityKind.ValueOffset));
    #endregion
    #endregion

    /// <summary>
    /// Represents all possible values of type <see cref="IntervalQualityKind"/> as <see langword="enum"/> values.
    /// </summary>
    public enum Values : sbyte
    {
        /// <summary>
        /// Represents diminished interval qualities.
        /// </summary>
        Diminished = -2,

        /// <summary>
        /// Represents the minor interval quality.
        /// </summary>
        Minor = -1,

        /// <summary>
        /// Represents the perfect interval quality.
        /// </summary>
        Perfect = 0,

        /// <summary>
        /// Represents the major interval quality.
        /// </summary>
        Major = 1,

        /// <summary>
        /// Represents augmented interval qualities.
        /// </summary>
        Augmented = 2,
    }
}
