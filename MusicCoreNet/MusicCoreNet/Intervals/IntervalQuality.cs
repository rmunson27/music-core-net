using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using BaseValues = IntervalQualityKind.Values;

#region Qualities
/// <summary>
/// Represents a general interval quality that can be either perfectable or imperfectable.
/// </summary>
/// <remarks>
/// The default value of this struct represents a perfect interval quality.
/// </remarks>
public readonly record struct IntervalQuality
    : IEquatable<PerfectableIntervalQuality>, IEquatable<ImperfectableIntervalQuality>
{
    #region Constants
    /// <summary>
    /// A minor interval quality.
    /// </summary>
    public static readonly IntervalQuality Minor = ImperfectableIntervalQuality.Minor;

    /// <summary>
    /// A perfect interval quality.
    /// </summary>
    public static readonly IntervalQuality Perfect = PerfectableIntervalQuality.Perfect;

    /// <summary>
    /// A major interval quality.
    /// </summary>
    public static readonly IntervalQuality Major = ImperfectableIntervalQuality.Major;
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.CircleOfFifthsIndex,
        StorageType.Imperfectable => _quality.Imperfectable.CircleOfFifthsIndex,
        _ => _quality.PeripheralDegree + Math.Sign(_quality.PeripheralDegree), // Add 1 to make room for major and minor
    };

    /// <summary>
    /// Gets the kind of this interval quality.
    /// </summary>
    public IntervalQualityKind Kind => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.Kind,
        StorageType.Imperfectable => _quality.Imperfectable.Kind,
        _ => _quality.PeripheralDegree > 0 ? IntervalQualityKind.Augmented : IntervalQualityKind.Diminished,
    };

    /// <summary>
    /// Gets the specific perfectability of this interval quality, or <see langword="null"/> if the quality applies
    /// to both perfectable and imperfectable intervals.
    /// </summary>
    public IntervalPerfectability? Perfectability => PerfectabilityFromStorageType(_storageType);
    private readonly StorageType _storageType;

    /// <summary>
    /// Gets this instance as a <see cref="PerfectableIntervalQuality"/> without fully performing internal storage
    /// type checking.
    /// </summary>
    /// <remarks>
    /// This operation is unsafe and should only be used internally.
    /// </remarks>
    internal PerfectableIntervalQuality UnsafeAsPerfectable
        => _storageType == StorageType.Perfectable
            ? _quality.Perfectable
            : new(_quality.PeripheralDegree);

    /// <summary>
    /// Gets this instance as an <see cref="ImperfectableIntervalQuality"/> without fully performing internal storage
    /// type checking.
    /// </summary>
    /// <remarks>
    /// This operation is unsafe and should only be used internally.
    /// </remarks>
    internal ImperfectableIntervalQuality UnsafeAsImperfectable
        => _storageType == StorageType.Imperfectable
            ? _quality.Imperfectable

            // Remove 1 from negative degree to make room for minor
            : new(_quality.PeripheralDegree < 0 ? _quality.PeripheralDegree - 1 : _quality.PeripheralDegree);

    /// <summary>
    /// Gets this instance as a <see cref="CentralIntervalQuality"/> without fully performing internal storage
    /// type checking.
    /// </summary>
    /// <remarks>
    /// This operation is unsafe and should only be used internally.
    /// </remarks>
    internal CentralIntervalQuality UnsafeAsCentral
        => _storageType == StorageType.Imperfectable
            ? new(_quality.Imperfectable.MajorBasedIndex == 0 ? 1 : -1)
            : CentralIntervalQuality.Perfect;

    /// <summary>
    /// Gets this instance as a <see cref="PeripheralIntervalQuality"/> without fully performing internal storage
    /// type checking.
    /// </summary>
    /// <remarks>
    /// This operation is unsafe and should only be used internally.
    /// </remarks>
    internal PeripheralIntervalQuality UnsafeAsPeripheral
        => _quality.PeripheralDegree > 0
                ? new(PeripheralIntervalQualityKind.Augmented, _quality.PeripheralDegree)
                : new(PeripheralIntervalQualityKind.Diminished, -_quality.PeripheralDegree);

    private readonly Storage _quality;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the perfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(PerfectableIntervalQuality Quality)
    {
        _quality = new();

        if (Quality.PerfectBasedIndex == 0)
        {
            _quality.Perfectable = Quality;
            _storageType = StorageType.Perfectable;
        }
        else // Augmented or diminished
        {
            _quality.PeripheralDegree = Quality.PerfectBasedIndex;
            _storageType = StorageType.Peripheral;
        }
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the imperfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(ImperfectableIntervalQuality Quality)
    {
        _quality = new();

        switch (Quality.MajorBasedIndex)
        {
            case < -1: // Diminished
                _quality.PeripheralDegree = Quality.MajorBasedIndex + 1; // Add 1 to ignore minor
                _storageType = StorageType.Peripheral;
                break;

            case 0 or -1: // Major or minor
                _quality.Imperfectable = Quality;
                _storageType = StorageType.Imperfectable;
                break;

            default: // Augmented
                _quality.PeripheralDegree = Quality.MajorBasedIndex;
                _storageType = StorageType.Peripheral;
                break;
        }
    }

    /// <summary>
    /// Constructs a new <see cref="IntervalQuality"/> representing the non-basic quality (augmented or diminished)
    /// described by the integer passed in.
    /// </summary>
    /// <param name="NonBasicDegree"></param>
    private IntervalQuality([NonZero] int NonBasicDegree)
    {
        _quality = new() { PeripheralDegree = NonBasicDegree };
        _storageType = StorageType.Peripheral;
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new augmented <see cref="IntervalQuality"/> with the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static IntervalQuality Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new diminished <see cref="IntervalQuality"/> with the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static IntervalQuality Diminished([Positive] int Degree = 1)
        => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Gets the quality of the simplest (i.e. closest to perfect) interval spanning the given number of half steps,
    /// or a quality of the supplied tritone quality type if the number of half steps indicates a tritone (6 half
    /// steps, as this case is ambiguous between augmented and diminished).
    /// </summary>
    /// <param name="halfSteps"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="halfSteps"/> was negative.</exception>
    internal static IntervalQuality OfSimplestIntervalWithHalfSteps(
        [NonNegative] int halfSteps, PeripheralIntervalQualityKind tritoneQualityKind)
        => OfSimplestIntervalWithHalfSteps(halfSteps)
            ?? (tritoneQualityKind == PeripheralIntervalQualityKind.Augmented
                    ? PerfectableIntervalQuality.Augmented()
                    : PerfectableIntervalQuality.Diminished());

    /// <summary>
    /// Gets the quality of the simplest (i.e. closest to perfect) interval spanning the given number of half steps,
    /// or <see langword="null"/> if the number of half steps is 6 (as this is a tritone and therefore ambiguous
    /// between augmented and diminished).
    /// </summary>
    /// <param name="halfSteps"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="halfSteps"/> was negative.</exception>
    internal static IntervalQuality? OfSimplestIntervalWithHalfSteps([NonNegative] int halfSteps)
        => (halfSteps % NotePitchClass.ValuesCount) switch
        {
            1 or 3 or 8 or 10 => Minor,
            0 or 5 or 7 => Perfect,
            2 or 4 or 9 or 11 => Major,
            6 => null,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(halfSteps), halfSteps, $"Parameter cannot be negative."),
        };

    /// <summary>
    /// Creates a new <see cref="IntervalQuality"/> from a corresponding perfect-based circle of fifths index.
    /// </summary>
    /// <returns></returns>
    /// <seealso cref="CircleOfFifthsIndex"/>
    public static IntervalQuality FromCircleOfFifthsIndex(int Index) => Index switch
    {
        -1 => Minor,
        0 => Perfect,
        1 => Major,

        // Augmented or diminished - add another space to make room for major and minor
        _ => new(Index - Math.Sign(Index)),
    };
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
        => IsAugmented() ? Try.Success(out Degree, _quality.PeripheralDegree) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => _storageType == StorageType.Peripheral && _quality.PeripheralDegree > 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => _storageType == StorageType.Imperfectable && _quality.Imperfectable.IsMajor();

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => _storageType == StorageType.Perfectable && _quality.Perfectable.IsPerfect();

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => _storageType == StorageType.Imperfectable && _quality.Imperfectable.IsMinor();

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
        => IsDiminished() ? Try.Success(out Degree, -_quality.PeripheralDegree) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => _storageType == StorageType.Peripheral && _quality.PeripheralDegree < 0;
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IntervalQuality other)
        => _storageType == other._storageType
            && _storageType switch
            {
                StorageType.Perfectable => _quality.Perfectable == other._quality.Perfectable,
                StorageType.Imperfectable => _quality.Imperfectable == other._quality.Imperfectable,
                _ => _quality.PeripheralDegree == other._quality.PeripheralDegree,
            };

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.GetHashCode(),
        StorageType.Imperfectable => _quality.Imperfectable.GetHashCode(),
        _ => _quality.PeripheralDegree.GetHashCode(),
    };

    #region Operators And Explicit `IEquatable` Implementations
    #region Perfectable
    /// <summary>
    /// Determines if the qualities passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(IntervalQuality lhs, PerfectableIntervalQuality rhs) => !lhs.Equals(rhs);

    /// <summary>
    /// Determines if the qualities passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(IntervalQuality lhs, PerfectableIntervalQuality rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Determines if the qualities passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(PerfectableIntervalQuality lhs, IntervalQuality rhs) => !rhs.Equals(lhs);

    /// <summary>
    /// Determines if the qualities passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(PerfectableIntervalQuality lhs, IntervalQuality rhs) => rhs.Equals(lhs);

    /// <inheritdoc/>
    public bool Equals(PerfectableIntervalQuality other) => IsPerfectable(out var pThis) && pThis == other;
    #endregion

    #region Imperfectable
    /// <summary>
    /// Determines if the qualities passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(IntervalQuality lhs, ImperfectableIntervalQuality rhs) => !lhs.Equals(rhs);

    /// <summary>
    /// Determines if the qualities passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(IntervalQuality lhs, ImperfectableIntervalQuality rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Determines if the qualities passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(ImperfectableIntervalQuality lhs, IntervalQuality rhs) => !rhs.Equals(lhs);

    /// <summary>
    /// Determines if the qualities passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(ImperfectableIntervalQuality lhs, IntervalQuality rhs) => rhs.Equals(lhs);

    /// <inheritdoc/>
    public bool Equals(ImperfectableIntervalQuality other) => IsImperfectable(out var pThis) && pThis == other;
    #endregion
    #endregion
    #endregion

    #region Conversion
    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// The perfectabilities of this quality and <paramref name="number"/> did not match.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> was not positive.
    /// </exception>
    public Interval WithNumber(int number) => WithNumber((IntervalNumber)number);

    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// The perfectabilities of this quality and <paramref name="number"/> did not match.
    /// </exception>
    public Interval WithNumber(IntervalNumber number) => Interval.Create(this, number);

    /// <summary>
    /// Determines whether this instance is described by the perfectability passed in.
    /// </summary>
    /// <param name="perfectability"></param>
    /// <returns></returns>
    public bool IsPerfectability(IntervalPerfectability perfectability) => perfectability == Perfectable
                                                                            ? IsPerfectable()
                                                                            : IsImperfectable();

    /// <summary>
    /// Determines if this instance represents the quality of a perfectable interval, setting the equivalent
    /// <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if so and setting the
    /// equivalent <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="imperfectableQuality"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableIntervalQuality quality, out ImperfectableIntervalQuality imperfectableQuality)
        => IsPerfectable()
            ? Try.Success(out quality, UnsafeAsPerfectable, out imperfectableQuality)
            : Try.Failure(out quality, out imperfectableQuality, UnsafeAsImperfectable);

    /// <summary>
    /// Determines if this instance represents the quality of a perfectable interval, setting the equivalent
    /// <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality quality)
        => IsPerfectable()
            ? Try.Success(out quality, UnsafeAsPerfectable)
            : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance represents the quality of a perfectable interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => _storageType is not StorageType.Imperfectable;

    /// <summary>
    /// Determines if this instance represents the quality of an imperfectable interval, setting the equivalent
    /// <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if so and setting the
    /// equivalent <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="perfectableQuality"></param>
    /// <returns></returns>
    public bool IsImperfectable(
        out ImperfectableIntervalQuality quality, out PerfectableIntervalQuality perfectableQuality)
        => IsImperfectable()
            ? Try.Success(out quality, UnsafeAsImperfectable, out perfectableQuality)
            : Try.Failure(out quality, out perfectableQuality, UnsafeAsPerfectable);

    /// <summary>
    /// Determines if this instance represents the quality of an imperfectable interval, setting the equivalent
    /// <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableIntervalQuality quality)
        => IsImperfectable()
            ? Try.Success(out quality, UnsafeAsImperfectable)
            : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance represents the quality of an imperfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => _storageType is not StorageType.Perfectable;

    /// <summary>
    /// Determines if this instance represents a central interval quality, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> in an <see langword="out"/> parameter if so and the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="peripheralQuality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality, out PeripheralIntervalQuality peripheralQuality)
        => IsCentral()
            ? Try.Success(out quality, UnsafeAsCentral, out peripheralQuality)
            : Try.Failure(out quality, out peripheralQuality, UnsafeAsPeripheral);

    /// <summary>
    /// Determines if this instance represents a central interval quality, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality)
        => IsCentral() ? Try.Success(out quality, UnsafeAsCentral) : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance represents a central interval quality.
    /// </summary>
    /// <returns></returns>
    public bool IsCentral() => _storageType is not StorageType.Peripheral;

    /// <summary>
    /// Determines if this instance represents a peripheral interval quality, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so, and setting the
    /// equivalent <see cref="CentralIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="centralQuality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality, out CentralIntervalQuality centralQuality)
        => IsPeripheral()
            ? Try.Success(out quality, UnsafeAsPeripheral, out centralQuality)
            : Try.Failure(out quality, out centralQuality, UnsafeAsCentral);

    /// <summary>
    /// Determines if this instance represents a peripheral interval quality, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality)
        => IsPeripheral() ? Try.Success(out quality, UnsafeAsPeripheral) : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance represents a peripheral interval quality.
    /// </summary>
    /// <returns></returns>
    public bool IsPeripheral() => _storageType is StorageType.Peripheral;

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableIntervalQuality"/> to an <see cref="IntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator IntervalQuality(PerfectableIntervalQuality quality) => new(quality);

    /// <summary>
    /// Implicitly converts a <see cref="ImperfectableIntervalQuality"/> to an <see cref="IntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator IntervalQuality(ImperfectableIntervalQuality quality) => new(quality);

    /// <summary>
    /// Implicitly converts a <see cref="CentralIntervalQuality"/> to an <see cref="IntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator IntervalQuality(CentralIntervalQuality quality) => quality.Kind.Value switch
    {
        CentralIntervalQualityKind.Values.Perfect => Perfect,
        CentralIntervalQualityKind.Values.Major => Major,
        _ => Minor,
    };

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQuality"/> to an <see cref="IntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator IntervalQuality(PeripheralIntervalQuality quality)
        => quality.IsDiminished() ? new(-quality.Degree) : new(quality.Degree);
    #endregion

    #region Computation
    /// <summary>
    /// Gets the perfect-based or major-based index of this <see cref="IntervalQuality"/> depending on the
    /// perfectability, using the <paramref name="nonAmbiguousPerfectability"/> parameter to handle ambiguous
    /// augmented or diminished cases.
    /// </summary>
    /// <param name="nonAmbiguousPerfectability"></param>
    /// <returns></returns>
    internal int PerfectOrMajorBasedIndex(IntervalPerfectability nonAmbiguousPerfectability) => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.PerfectBasedIndex,
        StorageType.Imperfectable => _quality.Imperfectable.MajorBasedIndex,
        _ => nonAmbiguousPerfectability == Perfectable
                ? _quality.PeripheralDegree

                // Remove 1 to make room for minor
                : (_quality.PeripheralDegree < 0 ? _quality.PeripheralDegree - 1 : _quality.PeripheralDegree),
    };

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public IntervalQuality Inversion() => _storageType switch
    {
        StorageType.Perfectable => new(_quality.Perfectable.Inversion()),
        StorageType.Imperfectable => new(_quality.Imperfectable.Inversion()),
        _ => new(-_quality.PeripheralDegree),
    };
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => IsPerfectable()
                                            ? _quality.Perfectable.ToString()
                                            : _quality.Imperfectable.ToString();
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// Internally represents the two possible distinct perfectable interval quality types and an integer representing
    /// an augmented or diminished degree as a discriminated union for storage in instances of this type.
    /// </summary>
    /// <remarks>
    /// It is the responsibility of the <see cref="IntervalQuality"/> struct to maintain information about which of
    /// the three options is selected, or undefined behavior could occur.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    private struct Storage
    {
        [FieldOffset(0)]
        public PerfectableIntervalQuality Perfectable;

        [FieldOffset(0)]
        public ImperfectableIntervalQuality Imperfectable;

        [FieldOffset(0)]
        public int PeripheralDegree;
    }

    /// <summary>
    /// Describes the method of internal data storage for this type.
    /// </summary>
    private enum StorageType : byte
    {
        Perfectable = Values.Perfectable,
        Imperfectable = Values.Imperfectable,
        Peripheral = Perfectable + Imperfectable + 1,
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntervalPerfectability? PerfectabilityFromStorageType(StorageType ip) => ip switch
    {
        StorageType.Perfectable => Perfectable,
        StorageType.Imperfectable => Imperfectable,
        _ => null,
    };
    #endregion

    #region Helpers
    /// <summary>
    /// Returns an exception that describes <paramref name="actualQualityName"/> as not being described by
    /// <paramref name="expectedQualityKind"/>.
    /// </summary>
    /// <param name="actualQualityName"></param>
    /// <param name="expectedQualityKind"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static InvalidCastException DoesNotMatch(string actualQualityName, string expectedQualityKind)
        => new($"Interval quality {actualQualityName} is not {expectedQualityKind}.");

    #region Strings
    /// <summary>
    /// Gets a string representing the specified diminished interval quality.
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string DiminishedString(int degree) => $"Diminished {{ Degree = {degree} }}";

    /// <summary>
    /// A string representing the minor interval quality.
    /// </summary>
    internal const string MinorString = "Minor";

    /// <summary>
    /// A string representing the perfect interval quality.
    /// </summary>
    internal const string PerfectString = "Perfect";

    /// <summary>
    /// A string representing the major interval quality.
    /// </summary>
    internal const string MajorString = "Major";

    /// <summary>
    /// Gets a string representing the specified augmented interval quality.
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string AugmentedString(int degree) => $"Augmented {{ Degree == {degree} }}";
    #endregion
    #endregion
}

/// <summary>
/// Represents a central interval quality (minor, perfect or major).
/// </summary>
/// <remarks>
/// The default instance of this type represents the perfect interval quality.
/// </remarks>
public readonly record struct CentralIntervalQuality
{
    #region Constants
    /// <summary>
    /// A minor interval quality.
    /// </summary>
    public static readonly CentralIntervalQuality Minor = new(CentralIntervalQualityKind.Minor);

    /// <summary>
    /// A perfect interval quality.
    /// </summary>
    /// <remarks>
    /// This is the default value of this type.
    /// </remarks>
    public static readonly CentralIntervalQuality Perfect = new(CentralIntervalQualityKind.Perfect);

    /// <summary>
    /// A major interval quality.
    /// </summary>
    public static readonly CentralIntervalQuality Major = new(CentralIntervalQualityKind.Major);
    #endregion

    #region Properties
    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex => (int)Kind.Value;

    private static PerfectableIntervalQuality UnsafeAsPerfectable => PerfectableIntervalQuality.Perfect;

    private ImperfectableIntervalQuality UnsafeAsImperfectable
        => Kind == CentralIntervalQualityKind.Major
            ? ImperfectableIntervalQuality.Major
            : ImperfectableIntervalQuality.Minor;

    /// <summary>
    /// Gets the kind of this instance.
    /// </summary>
    public CentralIntervalQualityKind Kind { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new <see cref="CentralIntervalQuality"/> with the given kind.
    /// </summary>
    /// <param name="kind"></param>
    internal CentralIntervalQuality(CentralIntervalQualityKind kind) { Kind = kind; }

    /// <summary>
    /// Constructs a new <see cref="CentralIntervalQuality"/> with the given circle of fifths index.
    /// </summary>
    /// <param name="circleOfFifthsIndex"></param>
    internal CentralIntervalQuality(int circleOfFifthsIndex)
    {
        Kind = new((CentralIntervalQualityKind.Values)circleOfFifthsIndex);
    }
    #endregion

    #region Classification
    /// <summary>
    /// Determines whether or not this instance is major.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => Kind == CentralIntervalQualityKind.Major;

    /// <summary>
    /// Determines whether or not this instance is perfect.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => Kind == CentralIntervalQualityKind.Perfect;

    /// <summary>
    /// Determines whether or not this instance is minor.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => Kind == CentralIntervalQualityKind.Minor;
    #endregion

    #region Conversion
    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// The perfectabilities of this quality and <paramref name="number"/> did not match.
    /// </exception>
    public Interval WithNumber(IntervalNumber number) => Interval.Create(this, number);

    /// <summary>
    /// Determines whether this instance is described by the perfectability passed in.
    /// </summary>
    /// <param name="perfectability"></param>
    /// <returns></returns>
    public bool IsPerfectability(IntervalPerfectability perfectability) => perfectability == Perfectable
                                                                            ? IsPerfectable()
                                                                            : IsImperfectable();

    /// <summary>
    /// Determines whether this instance is perfectable, setting the equivalent
    /// <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="imperfectableQuality"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableIntervalQuality quality, out ImperfectableIntervalQuality imperfectableQuality)
        => IsPerfectable()
            ? Try.Success(out quality, CentralIntervalQuality.UnsafeAsPerfectable, out imperfectableQuality)
            : Try.Failure(out quality, out imperfectableQuality, UnsafeAsImperfectable);

    /// <summary>
    /// Determines whether this instance is perfectable, setting the equivalent
    /// <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality quality)
        => IsPerfectable() ? Try.Success(out quality, PerfectableIntervalQuality.Perfect) : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance is perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Kind == CentralIntervalQualityKind.Perfect;

    /// <summary>
    /// Determines whether this instance is imperfectable, setting the equivalent
    /// <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if so, and setting the
    /// equivalent <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="perfectableQuality"></param>
    /// <returns></returns>
    public bool IsImperfectable(
        out ImperfectableIntervalQuality quality, out PerfectableIntervalQuality perfectableQuality)
        => IsImperfectable()
            ? Try.Success(out quality, UnsafeAsImperfectable, out perfectableQuality)
            : Try.Failure(out quality, out perfectableQuality, CentralIntervalQuality.UnsafeAsPerfectable);

    /// <summary>
    /// Determines whether this instance is imperfectable, setting the equivalent
    /// <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableIntervalQuality quality)
        => IsImperfectable() ? Try.Success(out quality, UnsafeAsImperfectable) : Try.Failure(out quality);

    /// <summary>
    /// Determines if this instance is imperfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => Kind != CentralIntervalQualityKind.Perfect;

    /// <summary>
    /// Explicitly converts an <see cref="ImperfectableIntervalQuality"/> to a <see cref="CentralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQuality(IntervalQuality quality)
        => quality.IsCentral(out var cq) ? cq : throw IntervalQuality.DoesNotMatch(quality.ToString(), "central");

    /// <summary>
    /// Explicitly converts an <see cref="ImperfectableIntervalQuality"/> to a <see cref="CentralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQuality(ImperfectableIntervalQuality quality)
        => quality.IsCentral(out var cq) ? cq : throw IntervalQuality.DoesNotMatch(quality.ToString(), "central");

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQuality"/> to a <see cref="CentralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator CentralIntervalQuality(PerfectableIntervalQuality quality)
        => quality.IsCentral(out var cq) ? cq : throw IntervalQuality.DoesNotMatch(quality.ToString(), "central");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsMajor()) return IntervalQuality.MajorString;
        else return IntervalQuality.MinorString;
    }
    #endregion
}

/// <summary>
/// Represents a peripheral interval quality (augmented or diminished).
/// </summary>
public readonly record struct PeripheralIntervalQuality
{
    #region Properties
    #region Conversion
    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a unison with this quality.
    /// </summary>
    public SimpleIntervalBase Unison
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Unison);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a second with this quality.
    /// </summary>
    public SimpleIntervalBase Second
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Second);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a third with this quality.
    /// </summary>
    public SimpleIntervalBase Third
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Third);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a fourth with this quality.
    /// </summary>
    public SimpleIntervalBase Fourth
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a fifth with this quality.
    /// </summary>
    public SimpleIntervalBase Fifth
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Fifth);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a sixth with this quality.
    /// </summary>
    public SimpleIntervalBase Sixth
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a seventh with this quality.
    /// </summary>
    public SimpleIntervalBase Seventh
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Seventh);
    #endregion

    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex => (int)Kind.Value;

    /// <summary>
    /// Gets the kind of this instance.
    /// </summary>
    public PeripheralIntervalQualityKind Kind { get; }

    /// <summary>
    /// Gets the degree of the modification represented by this instance (either diminished or augmented).
    /// </summary>
    public int Degree { get; }
    #endregion

    #region Constructor And Factory Methods
    /// <summary>
    /// Creates a new augmented <see cref="PeripheralIntervalQuality"/> with the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static PeripheralIntervalQuality Augmented([Positive] int Degree)
        => Degree > 0
            ? new(PeripheralIntervalQualityKind.Augmented, Degree)
            : throw new ArgumentOutOfRangeException(nameof(Degree), Degree, "Degree must be positive.");

    /// <summary>
    /// Creates a new diminished <see cref="PeripheralIntervalQuality"/> with the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static PeripheralIntervalQuality Diminished([Positive] int Degree)
        => Degree > 0
            ? new(PeripheralIntervalQualityKind.Diminished, Degree)
            : throw new ArgumentOutOfRangeException(nameof(Degree), Degree, "Degree must be positive.");

    /// <summary>
    /// Constructs a new <see cref="PeripheralIntervalQuality"/> with the given kind.
    /// </summary>
    /// <param name="kind"></param>
    internal PeripheralIntervalQuality(PeripheralIntervalQualityKind kind, [Positive] int degree)
    {
        Kind = kind;
        Degree = degree;
    }

    /// <summary>
    /// Constructs a new <see cref="PeripheralIntervalQuality"/> with the given degree, using positive degrees
    /// for augmented and negative degrees for diminished.
    /// </summary>
    /// <param name="degree"></param>
    internal PeripheralIntervalQuality([NonZero] int degree)
    {
        if (degree > 0)
        {
            Kind = PeripheralIntervalQualityKind.Augmented;
            Degree = degree;
        }
        else
        {
            Kind = PeripheralIntervalQualityKind.Diminished;
            Degree = -degree;
        }
    }
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this instance represents the quality of an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => Kind == PeripheralIntervalQualityKind.Augmented;

    /// <summary>
    /// Gets whether or not this instance represents the quality of a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => Kind == PeripheralIntervalQualityKind.Diminished;
    #endregion

    #region Conversion
    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> was not positive.
    /// </exception>
    public Interval WithNumber(int number) => WithNumber((IntervalNumber)number);

    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public Interval WithNumber(IntervalNumber number) => Interval.Create(this, number);

    /// <summary>
    /// Explicitly converts an <see cref="IntervalQuality"/> to a <see cref="PeripheralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQuality(IntervalQuality quality)
        => quality.IsPeripheral(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "peripheral");

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQuality"/> to a <see cref="PeripheralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQuality(PerfectableIntervalQuality quality)
        => quality.IsPeripheral(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "peripheral");

    /// <summary>
    /// Explicitly converts an <see cref="ImperfectableIntervalQuality"/> to a <see cref="PeripheralIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PeripheralIntervalQuality(ImperfectableIntervalQuality quality)
        => quality.IsPeripheral(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "peripheral");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented()) return IntervalQuality.AugmentedString(Degree);
        else return IntervalQuality.DiminishedString(Degree);
    }
    #endregion
}

/// <summary>
/// Represents the quality of a perfectable interval.
/// </summary>
/// <remarks>
/// The default value of this struct represents a perfect interval quality.
/// </remarks>
public readonly record struct PerfectableIntervalQuality
{
    #region Constants
    /// <summary>
    /// A perfectable interval quality representing a perfect interval.
    /// </summary>
    /// <remarks>This is the default value of this type.</remarks>
    public static readonly PerfectableIntervalQuality Perfect = new(0);
    #endregion

    #region Properties And Fields
    #region Conversion
    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a unison with this quality.
    /// </summary>
    public SimpleIntervalBase Unison
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Unison);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a fourth with this quality.
    /// </summary>
    public SimpleIntervalBase Fourth
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a fifth with this quality.
    /// </summary>
    public SimpleIntervalBase Fifth
        => SimpleIntervalBase.CreatePerfectable(this, SimpleIntervalNumber.Fifth);

    private static CentralIntervalQuality UnsafeAsCentral => CentralIntervalQuality.Perfect;

    private PeripheralIntervalQuality UnsafeAsPeripheral => new(PerfectBasedIndex);

    private ImperfectableIntervalQuality UnsafeAsImperfectable
        => PerfectBasedIndex < 0 ? new(PerfectBasedIndex - 1) : new(PerfectBasedIndex);
    #endregion

    /// <summary>
    /// Gets the kind of this perfectable interval quality.
    /// </summary>
    public PerfectableIntervalQualityKind Kind => PerfectBasedIndex switch
    {
        < 0 => PerfectableIntervalQualityKind.Diminished,
        0 => PerfectableIntervalQualityKind.Perfect,
        > 0 => PerfectableIntervalQualityKind.Augmented,
    };

    /// <summary>
    /// Represents this quality as an index based on <see cref="Perfect"/>.
    /// </summary>
    public int PerfectBasedIndex { get; }

    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex
    {
        get
        {
            if (IsAugmented(out var augDegree)) return augDegree + 1; // +1 to make way for major
            else if (IsDiminished(out var dimDegree)) return -dimDegree - 1; // -1 to make way for minor
            else return 0; // Must be perfect
        }
    }
    #endregion

    #region Constructor
    internal PerfectableIntervalQuality(int PerfectBasedIndex) { this.PerfectBasedIndex = PerfectBasedIndex; }
    #endregion

    #region Methods
    #region Factory
    #region Name
    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> representing an augmented interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static PerfectableIntervalQuality Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> representing a diminished interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static PerfectableIntervalQuality Diminished([Positive] int Degree = 1)
        => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)));
    #endregion

    #region Index
    /// <summary>
    /// Creates a new <see cref="PerfectableIntervalQuality"/> from the <see cref="Perfect"/>-relative integer index
    /// passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static PerfectableIntervalQuality FromPerfectBasedIndex(int Index) => new(Index);
    #endregion
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
        => IsAugmented() ? Try.Success(out Degree, PerfectBasedIndex) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => PerfectBasedIndex > 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => PerfectBasedIndex == 0;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
        => IsDiminished() ? Try.Success(out Degree, -PerfectBasedIndex) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => PerfectBasedIndex < 0;
    #endregion

    #region Conversion
    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="number"/> was not perfectable.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> was not positive.
    /// </exception>
    public Interval WithNumber(int number) => WithNumber((IntervalNumber)number);

    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="number"/> was not perfectable.
    /// </exception>
    public Interval WithNumber(IntervalNumber number) => Interval.Create(this, number);

    /// <summary>
    /// Determines whether or not this instance is imperfectable, setting the equivalent
    /// <see cref="ImperfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableIntervalQuality quality)
        => IsImperfectable() ? Try.Success(out quality, UnsafeAsImperfectable) : Try.Failure(out quality);

    /// <summary>
    /// Determines whether or not this instance is imperfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => PerfectBasedIndex != 0;

    /// <summary>
    /// Determines whether or not this instance is central, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> (<see cref="CentralIntervalQuality.Perfect"/>) in an
    /// <see langword="out"/> parameter if so, and setting the equivalent <see cref="PeripheralIntervalQuality"/>
    /// in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="peripheralQuality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality, out PeripheralIntervalQuality peripheralQuality)
        => IsCentral()
            ? Try.Success(out quality, UnsafeAsCentral, out peripheralQuality)
            : Try.Failure(out quality, out peripheralQuality, UnsafeAsPeripheral);

    /// <summary>
    /// Determines whether or not this instance is central, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> (<see cref="CentralIntervalQuality.Perfect"/>) in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality)
        => IsCentral() ? Try.Success(out quality, CentralIntervalQuality.Perfect) : Try.Failure(out quality);

    /// <summary>
    /// Determines whether or not this instance is central.
    /// </summary>
    /// <returns></returns>
    public bool IsCentral() => PerfectBasedIndex == 0;

    /// <summary>
    /// Determines whether or not this instance is peripheral, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so, and setting the
    /// equivalent <see cref="CentralIntervalQuality"/> (<see cref="CentralIntervalQuality.Perfect"/>) in an
    /// <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="centralQuality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality, out CentralIntervalQuality centralQuality)
        => IsPeripheral()
            ? Try.Success(out quality, UnsafeAsPeripheral, out centralQuality)
            : Try.Failure(out quality, out centralQuality, UnsafeAsCentral);

    /// <summary>
    /// Determines whether or not this instance is peripheral, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality)
        => IsPeripheral() ? Try.Success(out quality, UnsafeAsPeripheral) : Try.Failure(out quality);

    /// <summary>
    /// Determines whether this instance is peripheral.
    /// </summary>
    /// <returns></returns>
    public bool IsPeripheral() => PerfectBasedIndex != 0;

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQuality"/> to a <see cref="PerfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator PerfectableIntervalQuality(PeripheralIntervalQuality quality)
        => new(quality.IsAugmented() ? quality.Degree : -quality.Degree);

    /// <summary>
    /// Explicitly converts an <see cref="IntervalQuality"/> to a <see cref="PerfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQuality(IntervalQuality quality)
        => quality.IsPerfectable(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "perfectable");

    /// <summary>
    /// Explicitly converts a <see cref="CentralIntervalQuality"/> to a <see cref="PerfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQuality(CentralIntervalQuality quality)
        => quality.IsPerfectable(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "perfectable");

    /// <summary>
    /// Explicitly converts a <see cref="CentralIntervalQuality"/> to a <see cref="PerfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator PerfectableIntervalQuality(ImperfectableIntervalQuality quality)
        => quality.IsPerfectable(out var pq)
            ? pq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "perfectable");
    #endregion

    #region Computation
    /// <summary>
    /// Returns an interval quality equivalent to this one shifted by a given integer degree.
    /// </summary>
    /// <remarks>
    /// Positive <paramref name="degree"/> values will cause the result to be more augmented, whereas negative
    /// <paramref name="degree"/> values will cause the result to be more diminished.
    /// </remarks>
    /// <param name="degree"></param>
    /// <returns></returns>
    public PerfectableIntervalQuality ShiftedBy(int degree) => new(degree + PerfectBasedIndex);

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public PerfectableIntervalQuality Inversion() => new(-PerfectBasedIndex);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PerfectableIntervalQuality other) => PerfectBasedIndex == other.PerfectBasedIndex;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => PerfectBasedIndex;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return IntervalQuality.AugmentedString(augmentedDegree);
        else if (IsDiminished(out var diminishedDegree)) return IntervalQuality.DiminishedString(diminishedDegree);
        else return IntervalQuality.PerfectString;
    }
    #endregion
    #endregion
}

/// <summary>
/// Represents the quality of a imperfectable interval.
/// </summary>
/// <remarks>
/// The default value of this struct represents a major interval quality.
/// </remarks>
public readonly record struct ImperfectableIntervalQuality
{
    #region Constants
    /// <summary>
    /// An imperfectable interval quality representing a minor interval.
    /// </summary>
    public static readonly ImperfectableIntervalQuality Minor = new(-1);

    /// <summary>
    /// An imperfectable interval quality representing a major interval.
    /// </summary>
    public static readonly ImperfectableIntervalQuality Major = new(0);
    #endregion

    #region Properties And Fields
    #region Conversion
    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a second with this quality.
    /// </summary>
    public SimpleIntervalBase Second
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Second);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a third with this quality.
    /// </summary>
    public SimpleIntervalBase Third
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Third);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a sixth with this quality.
    /// </summary>
    public SimpleIntervalBase Sixth
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Gets a new <see cref="SimpleIntervalBase"/> representing a seventh with this quality.
    /// </summary>
    public SimpleIntervalBase Seventh
        => SimpleIntervalBase.CreateImperfectable(this, SimpleIntervalNumber.Seventh);

    internal CentralIntervalQuality UnsafeAsCentral
        => new(MajorBasedIndex >= 0 ? MajorBasedIndex + 1 : MajorBasedIndex);

    internal PeripheralIntervalQuality UnsafeAsPeripheral
        => MajorBasedIndex > 0
            ? new(PeripheralIntervalQualityKind.Augmented, MajorBasedIndex)
            : new(PeripheralIntervalQualityKind.Diminished, -MajorBasedIndex - 1);

    internal PerfectableIntervalQuality UnsafeAsPerfectable
        => MajorBasedIndex > 0 ? new(MajorBasedIndex) : new(MajorBasedIndex + 1);
    #endregion

    /// <summary>
    /// Gets the kind of this imperfectable interval quality.
    /// </summary>
    public ImperfectableIntervalQualityKind Kind => MajorBasedIndex switch
    {
        < -1 => ImperfectableIntervalQualityKind.Diminished,
        > 0 => ImperfectableIntervalQualityKind.Augmented,
        0 => ImperfectableIntervalQualityKind.Major,
        -1 => ImperfectableIntervalQualityKind.Minor,
    };

    /// <summary>
    /// Represents this quality as an index based on <see cref="Major"/>.
    /// </summary>
    public int MajorBasedIndex { get; }

    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex
    {
        get
        {
            if (IsAugmented(out var augDegree)) return augDegree + 1; // + 1 to make way for major
            else if (IsDiminished(out var dimDegree)) return -dimDegree - 1; // -1 to make way for minor
            else return IsMajor() ? 1 : -1;
        }
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new <see cref="ImperfectableIntervalQuality"/> with the given major-based index.
    /// </summary>
    /// <param name="MajorBasedIndex"></param>
    internal ImperfectableIntervalQuality(int MajorBasedIndex) { this.MajorBasedIndex = MajorBasedIndex; }
    #endregion

    #region Methods
    #region Factory
    #region Name
    /// <summary>
    /// Creates a new <see cref="ImperfectableIntervalQuality"/> representing an augmented interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static ImperfectableIntervalQuality Augmented([Positive] int Degree = 1)
        => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="ImperfectableIntervalQuality"/> representing a diminished interval with the
    /// given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Degree"/> was not positive.
    /// </exception>
    public static ImperfectableIntervalQuality Diminished([Positive] int Degree = 1)
        => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)) - 1);
    #endregion

    #region Index
    /// <summary>
    /// Creates a new <see cref="ImperfectableIntervalQuality"/> from the <see cref="Major"/>-relative integer index
    /// passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static ImperfectableIntervalQuality FromMajorBasedIndex(int Index) => new(Index);
    #endregion
    #endregion

    #region Computation
    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="number"/> was not imperfectable.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> was not positive.
    /// </exception>
    public Interval WithNumber(int number) => WithNumber((IntervalNumber)number);

    /// <summary>
    /// Gets a new <see cref="Interval"/> with this quality and the specified number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="number"/> was not imperfectable.
    /// </exception>
    public Interval WithNumber(IntervalNumber number) => Interval.Create(this, number);

    /// <summary>
    /// Returns an interval quality equivalent to this one shifted by a given integer degree.
    /// </summary>
    /// <remarks>
    /// Positive <paramref name="degree"/> values will cause the result to be more augmented, whereas negative
    /// <paramref name="degree"/> values will cause the result to be more diminished.
    /// </remarks>
    /// <param name="degree"></param>
    /// <returns></returns>
    public ImperfectableIntervalQuality ShiftedBy(int degree) => new(degree + MajorBasedIndex);

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public ImperfectableIntervalQuality Inversion() => new(-MajorBasedIndex - 1);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ImperfectableIntervalQuality other) => MajorBasedIndex == other.MajorBasedIndex;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => MajorBasedIndex;
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
        => IsAugmented() ? Try.Success(out Degree, MajorBasedIndex) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => Kind == ImperfectableIntervalQualityKind.Augmented;

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => Kind == ImperfectableIntervalQualityKind.Major;

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => Kind == ImperfectableIntervalQualityKind.Minor;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
        => IsDiminished() ? Try.Success(out Degree, -MajorBasedIndex - 1) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => Kind == ImperfectableIntervalQualityKind.Diminished;
    #endregion

    #region Conversion
    /// <summary>
    /// Determines whether or not this instance is perfectable, setting the equivalent
    /// <see cref="PerfectableIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality quality)
        => IsPerfectable() ? Try.Success(out quality, UnsafeAsPerfectable) : Try.Failure(out quality);

    /// <summary>
    /// Determines whether this instance is perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => MajorBasedIndex is not (0 or -1);

    /// <summary>
    /// Determines whether or not this instance is central, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> (<see cref="CentralIntervalQuality.Perfect"/>) in an
    /// <see langword="out"/> parameter if so, and setting the equivalent <see cref="PeripheralIntervalQuality"/>
    /// in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="peripheralQuality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality, out PeripheralIntervalQuality peripheralQuality)
        => IsCentral()
            ? Try.Success(out quality, UnsafeAsCentral, out peripheralQuality)
            : Try.Failure(out quality, out peripheralQuality, UnsafeAsPeripheral);

    /// <summary>
    /// Determines whether or not this instance is central, setting the equivalent
    /// <see cref="CentralIntervalQuality"/> (<see cref="CentralIntervalQuality.Perfect"/>) in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsCentral(out CentralIntervalQuality quality)
        => IsCentral()
            ? Try.Success(out quality, UnsafeAsCentral)
            : Try.Failure(out quality);

    /// <summary>
    /// Determines whether or not this instance is central.
    /// </summary>
    /// <returns></returns>
    public bool IsCentral() => MajorBasedIndex is 0 or -1;

    /// <summary>
    /// Determines whether or not this instance is peripheral, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so, and setting the
    /// equivalent <see cref="CentralIntervalQuality"/> in an <see langword="out"/> parameter if not.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="centralQuality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality, out CentralIntervalQuality centralQuality)
        => IsPeripheral()
            ? Try.Success(out quality, UnsafeAsPeripheral, out centralQuality)
            : Try.Failure(out quality, out centralQuality, UnsafeAsCentral);

    /// <summary>
    /// Determines whether or not this instance is peripheral, setting the equivalent
    /// <see cref="PeripheralIntervalQuality"/> in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool IsPeripheral(out PeripheralIntervalQuality quality)
        => IsPeripheral() ? Try.Success(out quality, UnsafeAsPeripheral) : Try.Failure(out quality);

    /// <summary>
    /// Determines whether this instance is peripheral.
    /// </summary>
    /// <returns></returns>
    public bool IsPeripheral() => MajorBasedIndex is not (0 or -1);

    /// <summary>
    /// Implicitly converts a <see cref="PeripheralIntervalQuality"/> to
    /// an <see cref="ImperfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static implicit operator ImperfectableIntervalQuality(PeripheralIntervalQuality quality)
        => quality.IsAugmented() ? new(quality.Degree) : new(-quality.Degree - 1);

    /// <summary>
    /// Explicitly converts an <see cref="IntervalQuality"/> to an <see cref="ImperfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    public static explicit operator ImperfectableIntervalQuality(IntervalQuality quality)
        => quality.IsImperfectable(out var iq)
            ? iq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "imperfectable");

    /// <summary>
    /// Explicitly converts a <see cref="CentralIntervalQuality"/> to an <see cref="ImperfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableIntervalQuality(CentralIntervalQuality quality)
        => quality.IsImperfectable(out var iq)
            ? iq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "imperfectable");

    /// <summary>
    /// Explicitly converts a <see cref="PerfectableIntervalQuality"/> to
    /// an <see cref="ImperfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="quality"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ImperfectableIntervalQuality(PerfectableIntervalQuality quality)
        => quality.IsImperfectable(out var iq)
            ? iq
            : throw IntervalQuality.DoesNotMatch(quality.ToString(), "imperfectable");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return IntervalQuality.AugmentedString(augmentedDegree);
        else if (IsDiminished(out var diminishedDegree)) return IntervalQuality.DiminishedString(diminishedDegree);
        else return MajorBasedIndex == 0 ? IntervalQuality.MajorString : IntervalQuality.MinorString;
    }
    #endregion
    #endregion
}
#endregion

#region Quality Kinds
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
#endregion
