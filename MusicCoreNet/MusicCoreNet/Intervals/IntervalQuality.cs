using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

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
        _ => _quality.NonBasicDegree + Math.Sign(_quality.NonBasicDegree), // Add 1 to make room for major and minor
    };

    /// <summary>
    /// Gets the type of this interval quality.
    /// </summary>
    public IntervalQualityType Type => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.Type.ToIntervalQualityType(),
        StorageType.Imperfectable => _quality.Imperfectable.Type.ToIntervalQualityType(),
        _ => _quality.NonBasicDegree > 0 ? IntervalQualityType.Augmented : IntervalQualityType.Diminished,
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
            : new(_quality.NonBasicDegree);

    /// <summary>
    /// Gets this instance as a <see cref="ImperfectableIntervalQuality"/> without fully performing internal storage
    /// type checking.
    /// </summary>
    /// <remarks>
    /// This operation is unsafe and should only be used internally.
    /// </remarks>
    internal ImperfectableIntervalQuality UnsafeAsImperfectable
        => _storageType == StorageType.Imperfectable
            ? _quality.Imperfectable

            // Remove 1 from negative degree to make room for minor
            : new(_quality.NonBasicDegree < 0 ? _quality.NonBasicDegree - 1 : _quality.NonBasicDegree);

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
            _quality.NonBasicDegree = Quality.PerfectBasedIndex;
            _storageType = StorageType.NonBasic;
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
                _quality.NonBasicDegree = Quality.MajorBasedIndex + 1; // Add 1 to ignore minor
                _storageType = StorageType.NonBasic;
                break;

            case 0 or -1: // Major or minor
                _quality.Imperfectable = Quality;
                _storageType = StorageType.Imperfectable;
                break;

            default: // Augmented
                _quality.NonBasicDegree = Quality.MajorBasedIndex;
                _storageType = StorageType.NonBasic;
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
        _quality = new();
        _quality.NonBasicDegree = NonBasicDegree;
        _storageType = StorageType.NonBasic;
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
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="tritoneQualityType"/> was an unnamed enum value.
    /// </exception>
    internal static IntervalQuality OfSimplestIntervalWithHalfSteps(
        [NonNegative] int halfSteps, [NamedEnum] NonBasicIntervalQualityType tritoneQualityType)
        => OfSimplestIntervalWithHalfSteps(halfSteps)
            ?? (Throw.IfEnumArgUnnamed(tritoneQualityType, nameof(tritoneQualityType))
                    == NonBasicIntervalQualityType.Augmented
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
        => (halfSteps % 12) switch
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
    #region Perfectability
    /// <summary>
    /// Determines whether or not this instance can represent qualities of the given interval perfectability.
    /// </summary>
    /// <remarks>
    /// This method will return <see langword="true"/> for both <see cref="Perfectable"/> and
    /// <see cref="Imperfectable"/> if the current instance is augmented or diminished.
    /// </remarks>
    /// <param name="perfectability"></param>
    /// <returns></returns>
    public bool HasPerfectability(IntervalPerfectability perfectability) => perfectability == Perfectable
                                                                                ? IsPerfectable()
                                                                                : IsImperfectable();
                                                                
    #region Perfectable
    /// <summary>
    /// Gets whether or not this instance can describe perfectable intervals, setting the perfectable quality in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality Quality)
    {
        switch (_storageType)
        {
            case StorageType.Imperfectable:
                Quality = default;
                return false;

            default:
                Quality = UnsafeAsPerfectable;
                return true;
        }
    }

    /// <summary>
    /// Gets whether or not this instance can describe perfectable intervals.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Perfectability is null || Perfectability == Perfectable;
    #endregion

    #region Imperfectable
    /// <summary>
    /// Gets whether or not this instance can describe imperfectable intervals, setting the imperfectable quality in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableIntervalQuality Quality)
    {
        switch (_storageType)
        {
            case StorageType.Perfectable:
                Quality = default;
                return false;

            default:
                Quality = UnsafeAsImperfectable;
                return true;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is imperfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => Perfectability is null || Perfectability == Imperfectable;
    #endregion
    #endregion

    #region Specific Quality
    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsAugmented([NonNegative] out int Degree)
    {
        if (IsAugmented())
        {
            Degree = _quality.NonBasicDegree;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => _storageType == StorageType.NonBasic && _quality.NonBasicDegree > 0;

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
    {
        if (IsDiminished())
        {
            Degree = -_quality.NonBasicDegree;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => _storageType == StorageType.NonBasic && _quality.NonBasicDegree < 0;
    #endregion
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
                _ => _quality.NonBasicDegree == other._quality.NonBasicDegree,
            };

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _storageType switch
    {
        StorageType.Perfectable => _quality.Perfectable.GetHashCode(),
        StorageType.Imperfectable => _quality.Imperfectable.GetHashCode(),
        _ => _quality.NonBasicDegree.GetHashCode(),
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
    /// Implicitly converts a <see cref="PerfectableIntervalQuality"/> to an instance of this type.
    /// </summary>
    /// <param name="Quality"></param>
    public static implicit operator IntervalQuality(PerfectableIntervalQuality Quality) => new(Quality);

    /// <summary>
    /// Implicitly converts a <see cref="ImperfectableIntervalQuality"/> to an instance of this type.
    /// </summary>
    /// <param name="Quality"></param>
    public static implicit operator IntervalQuality(ImperfectableIntervalQuality Quality) => new(Quality);

    /// <summary>
    /// Explicitly converts an instance of this type to a <see cref="PerfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="Quality"></param>
    /// <exception cref="InvalidCastException">The instance passed in was not perfectable.</exception>
    public static explicit operator PerfectableIntervalQuality(IntervalQuality Quality)
        => Quality.IsPerfectable(out var pQuality)
            ? pQuality
            : throw new InvalidCastException("Interval quality was not perfectable.");

    /// <summary>
    /// Explicitly converts an instance of this type to a <see cref="ImperfectableIntervalQuality"/>.
    /// </summary>
    /// <param name="Quality"></param>
    /// <exception cref="InvalidCastException">The instance passed in was not perfectable.</exception>
    public static explicit operator ImperfectableIntervalQuality(IntervalQuality Quality)
        => Quality.IsImperfectable(out var iQuality)
            ? iQuality
            : throw new InvalidCastException("Interval quality was not imperfectable.");
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
                ? _quality.NonBasicDegree

                // Remove 1 to make room for minor
                : (_quality.NonBasicDegree < 0 ? _quality.NonBasicDegree - 1 : _quality.NonBasicDegree),
    };

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public IntervalQuality Inversion() => _storageType switch
    {
        StorageType.Perfectable => new(_quality.Perfectable.Inversion()),
        StorageType.Imperfectable => new(_quality.Imperfectable.Inversion()),
        _ => new(-_quality.NonBasicDegree),
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
        public int NonBasicDegree;
    }

    /// <summary>
    /// Describes the method of internal data storage for this type.
    /// </summary>
    private enum StorageType : byte
    {
        Perfectable = IntervalPerfectability.Values.Perfectable,
        Imperfectable = IntervalPerfectability.Values.Imperfectable,
        NonBasic = Perfectable + Imperfectable + 1,
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntervalPerfectability? PerfectabilityFromStorageType(StorageType ip) => ip switch
    {
        StorageType.Perfectable => Perfectable,
        StorageType.Imperfectable => Imperfectable,
        _ => null,
    };
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
    public static readonly PerfectableIntervalQuality Perfect = new(0);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the type of this perfectable interval quality.
    /// </summary>
    public PerfectableIntervalQualityType Type => PerfectBasedIndex switch
    {
        < 0 => PerfectableIntervalQualityType.Diminished,
        0 => PerfectableIntervalQualityType.Perfect,
        > 0 => PerfectableIntervalQualityType.Augmented,
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
    {
        if (IsAugmented())
        {
            Degree = PerfectBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => Type == PerfectableIntervalQualityType.Augmented;

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => Type == PerfectableIntervalQualityType.Perfect;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
    {
        if (IsDiminished())
        {
            Degree = -PerfectBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => Type == PerfectableIntervalQualityType.Diminished;
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
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return $"Augmented {{ Degree = {augmentedDegree} }}";
        else if (IsDiminished(out var diminishedDegree)) return $"Diminished {{ Degree = {diminishedDegree} }}";
        else return "Perfect";
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
    /// <summary>
    /// Gets the type of this imperfectable interval quality.
    /// </summary>
    public ImperfectableIntervalQualityType Type => MajorBasedIndex switch
    {
        < -1 => ImperfectableIntervalQualityType.Diminished,
        -1 => ImperfectableIntervalQualityType.Minor,
        0 => ImperfectableIntervalQualityType.Major,
        > 0 => ImperfectableIntervalQualityType.Augmented,
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
    internal ImperfectableIntervalQuality(int NumericalValue) { MajorBasedIndex = NumericalValue; }
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
    {
        if (IsAugmented())
        {
            Degree = MajorBasedIndex;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => Type == ImperfectableIntervalQualityType.Augmented;

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => Type == ImperfectableIntervalQualityType.Major;

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => Type == ImperfectableIntervalQualityType.Minor;

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
    {
        if (IsDiminished())
        {
            Degree = -MajorBasedIndex - 1;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => Type == ImperfectableIntervalQualityType.Diminished;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (IsAugmented(out var augmentedDegree)) return $"Augmented {{ Degree = {augmentedDegree} }}";
        else if (IsDiminished(out var diminishedDegree)) return $"Diminished {{ Degree = {diminishedDegree} }}";
        else return MajorBasedIndex == 0 ? "Major" : "Minor";
    }
    #endregion
    #endregion
}

/// <summary>
/// Extension methods and other static functionality for the <see cref="IntervalQualityType"/> and other related enums.
/// </summary>
public static class IntervalQualityTypes
{
    /// <summary>
    /// Converts the current instance to an <see cref="IntervalQualityType"/>.
    /// </summary>
    /// <remarks>
    /// This can be treated as an implicit conversion - all (named) instances of
    /// <see cref="NonBasicIntervalQualityType"/> are representable as named instances
    /// of <see cref="IntervalQualityType"/>.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IntervalQualityType ToIntervalQualityType(this NonBasicIntervalQualityType type)
        => (IntervalQualityType)type;

    /// <summary>
    /// Converts the current instance to an <see cref="IntervalQualityType"/>.
    /// </summary>
    /// <remarks>
    /// This can be treated as an implicit conversion - all (named) instances of
    /// <see cref="BasicIntervalQualityType"/> are representable as named instances
    /// of <see cref="IntervalQualityType"/>.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IntervalQualityType ToIntervalQualityType(this BasicIntervalQualityType type)
        => (IntervalQualityType)type;

    /// <summary>
    /// Converts the current instance to an <see cref="IntervalQualityType"/>.
    /// </summary>
    /// <remarks>
    /// This can be treated as an implicit conversion - all (named) instances of
    /// <see cref="PerfectableIntervalQualityType"/> are representable as named instances
    /// of <see cref="IntervalQualityType"/>.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IntervalQualityType ToIntervalQualityType(this PerfectableIntervalQualityType type)
        => (IntervalQualityType)type;

    /// <summary>
    /// Converts the current instance to an <see cref="IntervalQualityType"/>.
    /// </summary>
    /// <remarks>
    /// This can be treated as an implicit conversion - all (named) instances of
    /// <see cref="ImperfectableIntervalQualityType"/> are representable as named instances
    /// of <see cref="IntervalQualityType"/>.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IntervalQualityType ToIntervalQualityType(this ImperfectableIntervalQualityType type)
        => (IntervalQualityType)type;
}

/// <summary>
/// Represents the type of a non-basic interval quality (not major, perfect or minor).
/// </summary>
public enum NonBasicIntervalQualityType : sbyte
{
    /// <summary>
    /// Represents diminished interval qualities.
    /// </summary>
    Diminished = IntervalQualityType.Diminished,

    /// <summary>
    /// Represents augmented interval qualities.
    /// </summary>
    Augmented = IntervalQualityType.Augmented,
}

/// <summary>
/// Represents the type of a basic interval quality (major, perfect or minor).
/// </summary>
public enum BasicIntervalQualityType : sbyte
{
    /// <summary>
    /// Represents minor interval qualities.
    /// </summary>
    Minor = IntervalQualityType.Minor,

    /// <summary>
    /// Represents perfect interval qualities.
    /// </summary>
    Perfect = IntervalQualityType.Perfect,

    /// <summary>
    /// Represents major interval qualities.
    /// </summary>
    Major = IntervalQualityType.Major,
}

/// <summary>
/// Represents the type of a perfectable interval quality.
/// </summary>
public enum PerfectableIntervalQualityType : sbyte
{
    /// <summary>
    /// Represents diminished interval qualities.
    /// </summary>
    Diminished = IntervalQualityType.Diminished,

    /// <summary>
    /// Represents perfect interval qualities.
    /// </summary>
    Perfect = IntervalQualityType.Perfect,

    /// <summary>
    /// Represents augmented interval qualities.
    /// </summary>
    Augmented = IntervalQualityType.Augmented,
}

/// <summary>
/// Represents the type of an imperfectable interval quality.
/// </summary>
public enum ImperfectableIntervalQualityType : sbyte
{
    /// <summary>
    /// Represents diminished interval qualities.
    /// </summary>
    Diminished = IntervalQualityType.Diminished,

    /// <summary>
    /// Represents minor interval qualities.
    /// </summary>
    Minor = IntervalQualityType.Minor,

    /// <summary>
    /// Represents major interval qualities.
    /// </summary>
    Major = IntervalQualityType.Major,

    /// <summary>
    /// Represents augmented interval qualities.
    /// </summary>
    Augmented = IntervalQualityType.Augmented,
}

/// <summary>
/// Represents the type of an interval quality.
/// </summary>
public enum IntervalQualityType : sbyte
{
    /// <summary>
    /// Represents diminished interval qualities.
    /// </summary>
    Diminished = -2,

    /// <summary>
    /// Represents minor interval qualities.
    /// </summary>
    Minor = -1,

    /// <summary>
    /// Represents the perfect interval quality.
    /// </summary>
    Perfect = 0,

    /// <summary>
    /// Represents major interval qualities.
    /// </summary>
    Major = 1,

    /// <summary>
    /// Represents augmented interval qualities.
    /// </summary>
    Augmented = 2,
}

