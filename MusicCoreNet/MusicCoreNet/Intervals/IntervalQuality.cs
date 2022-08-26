using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Gets a minor interval quality.
    /// </summary>
    public static ImperfectableIntervalQuality Minor => ImperfectableIntervalQuality.Minor;

    /// <summary>
    /// Gets a perfect interval quality.
    /// </summary>
    public static PerfectableIntervalQuality Perfect => PerfectableIntervalQuality.Perfect;

    /// <summary>
    /// Gets a major interval quality.
    /// </summary>
    public static ImperfectableIntervalQuality Major => ImperfectableIntervalQuality.Major;
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the value of the <see cref="PerfectableIntervalQuality.PerfectBasedIndex"/> or
    /// <see cref="ImperfectableIntervalQuality.MajorBasedIndex"/> property depending on which type is
    /// represented internally.
    /// </summary>
    internal int PerfectOrMajorBasedIndex => IsPerfectable()
                                                ? InternalQuality.Perfectable.PerfectBasedIndex
                                                : InternalQuality.Imperfectable.MajorBasedIndex;

    /// <summary>
    /// Gets an index that can be used to order <see cref="IntervalQuality"/> instances based on their positions in
    /// the circle of fifths relative to <see cref="PerfectableIntervalQuality.Perfect"/>.
    /// </summary>
    public int CircleOfFifthsIndex => IsPerfectable()
                                        ? InternalQuality.Perfectable.CircleOfFifthsIndex
                                        : InternalQuality.Imperfectable.CircleOfFifthsIndex;

    /// <summary>
    /// Gets the perfectability of this interval quality.
    /// </summary>
    public IntervalPerfectability Perfectability { get; }

    private readonly InternalQualityStruct InternalQuality;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the perfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(PerfectableIntervalQuality Quality)
    {
        InternalQuality = new();
        InternalQuality.Perfectable = Quality;
        Perfectability = Perfectable;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalQuality"/> struct representing the imperfectable interval
    /// quality passed in.
    /// </summary>
    /// <param name="Quality"></param>
    public IntervalQuality(ImperfectableIntervalQuality Quality)
    {
        InternalQuality = new();
        InternalQuality.Imperfectable = Quality;
        Perfectability = Imperfectable;
    }
    #endregion

    #region Methods
    #region Factory
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
    public static IntervalQuality FromCircleOfFifthsIndex(int Index)
    {
        // Handle the simple hard-codeable cases
        switch (Index)
        {
            case -1: return ImperfectableIntervalQuality.Minor;
            case 0: return PerfectableIntervalQuality.Perfect;
            case 1: return ImperfectableIntervalQuality.Major;
        }

        if (Index < 0) // Diminished
        {
            if (Index % 2 == 0) return PerfectableIntervalQuality.Diminished(-Index / 2);
            else return ImperfectableIntervalQuality.Diminished((-Index - 1) / 2);
        }
        else // Augmented
        {
            if (Index % 2 == 0) return PerfectableIntervalQuality.Augmented(Index / 2);
            else return ImperfectableIntervalQuality.Augmented((Index - 1) / 2);
        }
    }
    #endregion

    #region Classification
    #region Perfectability
    #region Perfectable
    /// <summary>
    /// Gets whether or not this instance is perfectable, setting <paramref name="Perfectable"/> to the perfectable
    /// quality if so and setting <paramref name="Imperfectable"/> to the imperfectable quality otherwise.
    /// </summary>
    /// <param name="Perfectable"></param>
    /// <param name="Imperfectable"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableIntervalQuality Perfectable, out ImperfectableIntervalQuality Imperfectable)
    {
        if (IsPerfectable())
        {
            Perfectable = InternalQuality.Perfectable;
            Imperfectable = default;
            return true;
        }
        else
        {
            Perfectable = default;
            Imperfectable = InternalQuality.Imperfectable;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is perfectable, setting the perfectable quality in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableIntervalQuality Quality)
    {
        if (IsPerfectable())
        {
            Quality = InternalQuality.Perfectable;
            return true;
        }
        else
        {
            Quality = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfectable() => Perfectability == Perfectable;
    #endregion

    #region Imperfectable
    /// <summary>
    /// Gets whether or not this instance is imperfectable, setting the imperfectable quality in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Quality"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableIntervalQuality Quality)
    {
        if (IsImperfectable())
        {
            Quality = InternalQuality.Imperfectable;
            return true;
        }
        else
        {
            Quality = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is imperfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsImperfectable() => Perfectability == Imperfectable;
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
        => IsPerfectable()
            ? InternalQuality.Perfectable.IsAugmented(out Degree)
            : InternalQuality.Imperfectable.IsAugmented(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents an augmented interval.
    /// </summary>
    /// <returns></returns>
    public bool IsAugmented() => IsPerfectable()
                                    ? InternalQuality.Perfectable.IsAugmented()
                                    : InternalQuality.Imperfectable.IsAugmented();

    /// <summary>
    /// Gets whether or not this interval quality represents a major interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMajor() => IsImperfectable() && InternalQuality.Imperfectable.IsMajor();

    /// <summary>
    /// Gets whether or not this interval quality represents a perfect interval.
    /// </summary>
    /// <returns></returns>
    public bool IsPerfect() => IsPerfectable() && InternalQuality.Perfectable.IsPerfect();

    /// <summary>
    /// Gets whether or not this interval quality represents a minor interval.
    /// </summary>
    /// <returns></returns>
    public bool IsMinor() => IsImperfectable() && InternalQuality.Imperfectable.IsMinor();

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval, setting the
    /// <paramref name="Degree"/> parameter to the degree to which it is if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsDiminished([NonNegative] out int Degree)
        => IsPerfectable()
             ? InternalQuality.Perfectable.IsDiminished(out Degree)
             : InternalQuality.Imperfectable.IsDiminished(out Degree);

    /// <summary>
    /// Gets whether or not this interval quality represents a diminished interval.
    /// </summary>
    /// <returns></returns>
    public bool IsDiminished() => IsPerfectable()
                                    ? InternalQuality.Perfectable.IsDiminished()
                                    : InternalQuality.Imperfectable.IsDiminished();
    #endregion
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IntervalQuality other)
    {
        if (Perfectability == other.Perfectability)
        {
            return Perfectability switch
            {
                Perfectable => InternalQuality.Perfectable == other.InternalQuality.Perfectable,
                _ => InternalQuality.Imperfectable == other.InternalQuality.Imperfectable,
            };
        }
        else return false;
    }

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Perfectability == Perfectable
                                            ? InternalQuality.Perfectable.GetHashCode()
                                            : InternalQuality.Imperfectable.GetHashCode();

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
    internal IntervalQuality Shift(int degree)
        => IsPerfectable()
            ? new(new PerfectableIntervalQuality(degree + InternalQuality.Perfectable.PerfectBasedIndex))
            : new(new ImperfectableIntervalQuality(degree + InternalQuality.Imperfectable.MajorBasedIndex));

    /// <summary>
    /// Returns an interval quality equivalent to the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public IntervalQuality Inversion() => IsPerfectable()
                                            ? new(InternalQuality.Perfectable.Inversion())
                                            : new(InternalQuality.Imperfectable.Inversion());
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => IsPerfectable()
                                            ? InternalQuality.Perfectable.ToString()
                                            : InternalQuality.Imperfectable.ToString();
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// Internally represents the two possible interval quality types as a discriminated union.
    /// </summary>
    /// <remarks>
    /// It is the responsibility of the <see cref="IntervalQuality"/> struct to maintain information about which of
    /// the two options is selected, or undefined behavior could occur.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    private struct InternalQualityStruct
    {
        [FieldOffset(0)]
        public PerfectableIntervalQuality Perfectable;

        [FieldOffset(0)]
        public ImperfectableIntervalQuality Imperfectable;
    }
    #endregion
}

/// <summary>
/// Represents the quality of a perfectable interval.
/// </summary>
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
            // Expand the degrees of augmented or diminished to allow for the imperfectable qualities to fit
            // into the ordering
            if (IsAugmented(out var augDegree)) return augDegree * 2;
            else if (IsDiminished(out var dimDegree)) return -dimDegree * 2;

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
    public PerfectableIntervalQuality Shift(int degree) => new(degree + PerfectBasedIndex);

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
            // Expand the degrees of augmented or diminished to allow for the perfectable qualities to fit into
            // the ordering
            if (IsAugmented(out var augDegree)) return augDegree * 2 + 1;
            else if (IsDiminished(out var dimDegree)) return -dimDegree * 2 - 1;

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
    public ImperfectableIntervalQuality Shift(int degree) => new(degree + MajorBasedIndex);

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

