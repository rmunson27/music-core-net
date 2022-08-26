using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using static PerfectableSimpleIntervalNumber;
using static ImperfectableSimpleIntervalNumber;

#region Structs
/// <summary>
/// Represents the number of an interval.
/// </summary>
/// <remarks>
/// The default value of this struct represents a unison.
/// </remarks>
public readonly record struct IntervalNumber : IEquatable<int>, IComparable<IntervalNumber>, IComparable<int>
{
    #region Properties
    /// <summary>
    /// Gets the integer value of this number.
    /// </summary>
    [Positive] public int Value => Base.Value + AdditionalOctaves * 7;

    /// <summary>
    /// Gets the perfectability of this instance.
    /// </summary>
    [NamedEnum] public IntervalPerfectability Perfectability => Base.Perfectability;

    /// <summary>
    /// Gets whether or not this instance is perfectable.
    /// </summary>
    public bool IsPerfectable => Base.IsPerfectable();

    /// <summary>
    /// Gets whether or not this instance is imperfectable.
    /// </summary>
    public bool IsImperfectable => Base.IsImperfectable();

    /// <summary>
    /// Gets the <see cref="SimpleIntervalNumber"/> base that this instance adds octaves to.
    /// </summary>
    public SimpleIntervalNumber Base { get; }

    /// <summary>
    /// Gets the number of additional octaves added onto <see cref="Base"/>.
    /// </summary>
    [NonNegative] public int AdditionalOctaves { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalNumber"/> struct representing a simple interval number
    /// with a non-negative number of octaves added.
    /// </summary>
    /// <param name="Base"></param>
    /// <param name="AdditionalOctaves"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="AdditionalOctaves"/> was negative.</exception>
    public IntervalNumber(SimpleIntervalNumber Base, [NonNegative] int AdditionalOctaves = 0)
    {
        this.Base = Base;
        this.AdditionalOctaves = Throw.IfArgNegative(AdditionalOctaves, nameof(AdditionalOctaves));
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalNumber"/> struct representing the value passed in.
    /// </summary>
    /// <param name="Value"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Value"/> was negative.</exception>
    public IntervalNumber([Positive] int Value)
    {
        Throw.IfArgNotPositive(Value, nameof(Value));
        var base7Value = Value - 1;
        Base = SimpleIntervalNumber.FromValue(base7Value % 7 + 1);
        AdditionalOctaves = base7Value / 7;
    }
    #endregion

    #region Methods
    #region Classification
    /// <summary>
    /// Gets whether or not this instance represents a simple interval number (less than an octave), setting the
    /// value describing the number in an <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsSimple(out SimpleIntervalNumber Number)
    {
        if (IsSimple())
        {
            Number = Base;
            return true;
        }
        else
        {
            Number = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance represents a simple interval number (less than an octave).
    /// </summary>
    /// <returns></returns>
    public bool IsSimple() => AdditionalOctaves == 0;
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IntervalNumber other)
        => AdditionalOctaves == other.AdditionalOctaves && Base == other.Base;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Base, AdditionalOctaves);

    /// <summary>
    /// Determines if this instance has a value equal to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Equals(int value) => Value == value;

    /// <summary>
    /// Determines if the interval number has a value that is not equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(int lhs, IntervalNumber rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines if the interval number has a value that is equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(int lhs, IntervalNumber rhs) => lhs == rhs.Value;

    /// <summary>
    /// Determines if the interval number has a value that is not equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(IntervalNumber lhs, int rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines if the interval number has a value that is equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(IntervalNumber lhs, int rhs) => lhs.Value == rhs;
    #endregion

    #region Comparison
    #region IntervalNumber
    /// <summary>
    /// Determines if the left-hand <see cref="IntervalNumber"/> is greater than the
    /// right-hand <see cref="IntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(IntervalNumber lhs, IntervalNumber rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if the left-hand <see cref="IntervalNumber"/> is less than the
    /// right-hand <see cref="IntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(IntervalNumber lhs, IntervalNumber rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if the left-hand <see cref="IntervalNumber"/> is greater than or equal to the
    /// right-hand <see cref="IntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(IntervalNumber lhs, IntervalNumber rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if the left-hand <see cref="IntervalNumber"/> is less than or equal to the
    /// right-hand <see cref="IntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(IntervalNumber lhs, IntervalNumber rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Compares this instance to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(IntervalNumber other) => Value.CompareTo(other.Value);
    #endregion

    #region int
    /// <summary>
    /// Determines if the <see cref="IntervalNumber"/> passed in has a value that is greater than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >(IntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if the <see cref="IntervalNumber"/> passed in has a value that is less than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <(IntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if the <see cref="IntervalNumber"/> passed in has a value that is greater than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >=(IntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if the <see cref="IntervalNumber"/> passed in has a value that is less than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <=(IntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than the value of the <see cref="IntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >(int lhs, IntervalNumber rhs) => rhs.CompareTo(lhs) < 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than the value of the <see cref="IntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <(int lhs, IntervalNumber rhs) => rhs.CompareTo(lhs) > 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than or equal to the value of the
    /// <see cref="IntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >=(int lhs, IntervalNumber rhs) => rhs.CompareTo(lhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than or equal to the value of the
    /// <see cref="IntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <=(int lhs, IntervalNumber rhs) => rhs.CompareTo(lhs) >= 0;

    /// <summary>
    /// Compares the value of this instance to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public int CompareTo(int value) => Value.CompareTo(value);
    #endregion
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="SimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator IntervalNumber(SimpleIntervalNumber number) => new(number);

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="InvalidEnumArgumentException">
    /// The <see cref="PerfectableSimpleIntervalNumber"/> to convert was an unnamed enum value.
    /// </exception>
    public static implicit operator IntervalNumber([NamedEnum] PerfectableSimpleIntervalNumber number) => new(number);

    /// <summary>
    /// Implicitly converts a <see cref="ImperfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <exception cref="InvalidEnumArgumentException">
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> to convert was an unnamed enum value.
    /// </exception>
    /// <param name="number"></param>
    public static implicit operator IntervalNumber([NamedEnum] ImperfectableSimpleIntervalNumber number)
        => new(number);

    /// <summary>
    /// Implicitly converts an instance of this struct to an <see cref="int"/>.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator int(IntervalNumber number) => number.Value;

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to an instance of this struct.
    /// </summary>
    /// <param name="i"></param>
    /// <exception cref="InvalidCastException">The <see cref="int"/> value was not positive.</exception>
    public static explicit operator IntervalNumber(int i)
        => i <= 0 ? throw new InvalidCastException("Cannot cast non-positive value to interval number.") : new(i);
    #endregion
    #endregion
}

/// <summary>
/// Represents the number of a simple interval, that can be either perfectable or imperfectable.
/// </summary>
/// <remarks>
/// The default value of this struct represents a unison.
/// </remarks>
public readonly record struct SimpleIntervalNumber
    : IEquatable<PerfectableSimpleIntervalNumber>, IEquatable<ImperfectableSimpleIntervalNumber>, IEquatable<int>,
      IComparable<SimpleIntervalNumber>, IComparable<int>
{
    #region Constants
    /// <summary>
    /// The <see cref="PerfectableSimpleIntervalNumber"/> representing a fourth.
    /// </summary>
    [NamedEnum] public const PerfectableSimpleIntervalNumber Fourth = PerfectableSimpleIntervalNumber.Fourth;

    /// <summary>
    /// The <see cref="PerfectableSimpleIntervalNumber"/> representing a unison.
    /// </summary>
    [NamedEnum] public const PerfectableSimpleIntervalNumber Unison = PerfectableSimpleIntervalNumber.Unison;

    /// <summary>
    /// The <see cref="PerfectableSimpleIntervalNumber"/> representing a fifth.
    /// </summary>
    [NamedEnum] public const PerfectableSimpleIntervalNumber Fifth = PerfectableSimpleIntervalNumber.Fifth;

    /// <summary>
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> representing a second.
    /// </summary>
    [NamedEnum] public const ImperfectableSimpleIntervalNumber Second = ImperfectableSimpleIntervalNumber.Second;

    /// <summary>
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> representing a third.
    /// </summary>
    [NamedEnum] public const ImperfectableSimpleIntervalNumber Third = ImperfectableSimpleIntervalNumber.Third;

    /// <summary>
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> representing a sixth.
    /// </summary>
    [NamedEnum] public const ImperfectableSimpleIntervalNumber Sixth = ImperfectableSimpleIntervalNumber.Sixth;

    /// <summary>
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> representing a seventh.
    /// </summary>
    [NamedEnum] public const ImperfectableSimpleIntervalNumber Seventh = ImperfectableSimpleIntervalNumber.Seventh;
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the number of half steps in a perfect or major interval numbered with this instance depending on whether
    /// or not it is perfectable.
    /// </summary>
    [NonNegative] internal int PerfectOrMajorHalfSteps => IsPerfectable()
                                                            ? NonDefaultPerfectable.PerfectHalfSteps()
                                                            : InternalNumber.Imperfectable.MajorHalfSteps();

    /// <summary>
    /// Gets the integer value of this number.
    /// </summary>
    [Positive] public int Value => IsPerfectable() ? (int)NonDefaultPerfectable : (int)InternalNumber.Imperfectable;

    /// <summary>
    /// Gets the perfectability of this instance.
    /// </summary>
    public IntervalPerfectability Perfectability { get; }

    private readonly InternalNumberStruct InternalNumber;

    /// <summary>
    /// Gets a perfectable interval number that is not the (invalid) default.
    /// </summary>
    /// <remarks>
    /// If this is the default, then the value returned will be <see cref="Unison"/>.
    /// <para/>
    /// This should only be called in cases when the instance is guaranteed to wrap a <see cref="Perfectable"/> number,
    /// or else the behavior is undefined.
    /// </remarks>
    private PerfectableSimpleIntervalNumber NonDefaultPerfectable
        => InternalNumber.Perfectable == default ? Unison : InternalNumber.Perfectable;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="SimpleIntervalNumber"/> struct representing the
    /// <see cref="PerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="Number"/> was an unnamed enum value.</exception>
    public SimpleIntervalNumber([NamedEnum] PerfectableSimpleIntervalNumber Number)
    {
        InternalNumber = new();

        // Store unisons as the default, so that all instances are internally unambiguous
        // The instance will externally treat the (invalid) default as a unison
        InternalNumber.Perfectable = Throw.IfEnumArgUnnamed(Number, nameof(Number)) == Unison ? default : Number;

        Perfectability = Perfectable;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="SimpleIntervalNumber"/> struct representing the
    /// <see cref="ImperfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="Number"/> was an unnamed enum value.</exception>
    public SimpleIntervalNumber([NamedEnum] ImperfectableSimpleIntervalNumber Number)
    {
        InternalNumber = new();
        InternalNumber.Imperfectable = Throw.IfEnumArgUnnamed(Number, nameof(Number));
        Perfectability = Imperfectable;
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Gets the number of the simplest (i.e. closest to perfect) interval spanning the given number of half steps,
    /// or a number based on the supplied tritone quality type if the number of half steps indicates a tritone (6 half
    /// steps, as this case is ambiguous between augmented and diminished).
    /// </summary>
    /// <param name="halfSteps"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="halfSteps"/> was negative or >= 12.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="tritoneQualityType"/> was an unnamed enum value.
    /// </exception>
    internal static SimpleIntervalNumber OfSimplestIntervalWithHalfSteps(
        [NonNegative, LessThanInteger(12)] int halfSteps, [NamedEnum] NonBasicIntervalQualityType tritoneQualityType)
        => OfSimplestIntervalWithHalfSteps(halfSteps)
            ?? (Throw.IfEnumArgUnnamed(tritoneQualityType, nameof(tritoneQualityType))
                    == NonBasicIntervalQualityType.Augmented
                    ? Fourth
                    : Fifth);

    /// <summary>
    /// Gets the number of the simplest (i.e. closest to perfect) interval less than an octave spanning the given
    /// number of half steps, or <see langword="null"/> if the number of half steps is 6 (as this is a tritone and
    /// therefore ambiguous between augmented and diminished).
    /// </summary>
    /// <param name="halfSteps"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="halfSteps"/> was negative or >= 12.
    /// </exception>
    internal static SimpleIntervalNumber? OfSimplestIntervalWithHalfSteps(
        [NonNegative, LessThanInteger(12)] int halfSteps)
        => halfSteps switch
        {
            0 => Unison,
            1 or 2 => Second,
            3 or 4 => Third,
            5 => Fourth,
            6 => null,
            7 => Fifth,
            8 or 9 => Sixth,
            10 or 11 => Seventh,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(halfSteps), halfSteps, "Half steps must be in the range 0..11."),
        };

    /// <summary>
    /// Gets the <see cref="SimpleIntervalNumber"/> of a major or perfect interval with the circle-of-fifths
    /// perfect-unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any perfect or major interval.
    /// </exception>
    /// <see cref="CircleOfFifthsIndex"/>
    internal static SimpleIntervalNumber FromCircleOfFifthsIndex(
        [GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(5)] int Index) => Index switch
    {
        >= -1 and <= 1 => PerfectableSimpleIntervalNumbers.FromCircleOfFifthsIndex(Index),
        >= 2 and <= 5 => ImperfectableSimpleIntervalNumbers.FromCircleOfFifthsIndex(Index),
        _ => throw new ArgumentOutOfRangeException(
                nameof(Index), Index, "Index did not indicate any perfect or major interval."),
    };

    /// <summary>
    /// Creates a new <see cref="SimpleIntervalNumber"/> from the integer value passed in.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Value"/> was not in the range 1..7.
    /// </exception>
    public static SimpleIntervalNumber FromValue(int Value) => Value switch
    {
        1 or 4 or 5 => (PerfectableSimpleIntervalNumber)Value,
        2 or 3 or 6 or 7 => (ImperfectableSimpleIntervalNumber)Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, "Value was not in the range 1..7."),
    };
    #endregion

    #region Classification
    #region Perfectable
    /// <summary>
    /// Gets whether or not this instance is perfectable, setting <paramref name="Perfectable"/> to the perfectable
    /// number value if so and setting <paramref name="Imperfectable"/> to the imperfectable value otherwise.
    /// </summary>
    /// <param name="Perfectable"></param>
    /// <param name="Imperfectable"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableSimpleIntervalNumber Perfectable, out ImperfectableSimpleIntervalNumber Imperfectable)
    {
        if (IsPerfectable())
        {
            Perfectable = NonDefaultPerfectable;
            Imperfectable = default;
            return true;
        }
        else
        {
            Perfectable = default;
            Imperfectable = InternalNumber.Imperfectable;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is perfectable, setting the perfectable number value in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsPerfectable(out PerfectableSimpleIntervalNumber Number)
    {
        if (IsPerfectable())
        {
            Number = NonDefaultPerfectable;
            return true;
        }
        else
        {
            Number = default;
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
    /// Gets whether or not this instance is imperfectable, setting the imperfectable number value in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsImperfectable(out ImperfectableSimpleIntervalNumber Number)
    {
        if (IsImperfectable())
        {
            Number = InternalNumber.Imperfectable;
            return true;
        }
        else
        {
            Number = default;
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

    #region Computation
    /// <summary>
    /// Gets the circle of fifths index of a perfect or major interval numbered with the current instance relative to
    /// a perfect unison.
    /// </summary>
    /// <remarks>
    /// The interval used in the comparison will be perfect if the current instance is perfectable and major otherwise.
    /// </remarks>
    /// <returns></returns>
    internal int CircleOfFifthsIndex() => IsPerfectable()
                                            ? NonDefaultPerfectable.CircleOfFifthsIndex()
                                            : InternalNumber.Imperfectable.CircleOfFifthsIndex();

    /// <summary>
    /// Gets the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalNumber Inversion() => IsPerfectable()
                                                ? new(NonDefaultPerfectable.Inversion())
                                                : new(InternalNumber.Imperfectable.Inversion());
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SimpleIntervalNumber other)
    {
        if (Perfectability == other.Perfectability)
        {
            return Perfectability switch
            {
                Perfectable => InternalNumber.Perfectable == other.InternalNumber.Perfectable,
                _ => InternalNumber.Imperfectable == other.InternalNumber.Imperfectable,
            };
        }
        else return false;
    }

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => IsPerfectable()
                                            ? NonDefaultPerfectable.GetHashCode()
                                            : InternalNumber.Imperfectable.GetHashCode();

    #region Operators And Explicit `IEquatable` Implementations
    #region Perfectable
    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(SimpleIntervalNumber lhs, PerfectableSimpleIntervalNumber rhs) => !lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(SimpleIntervalNumber lhs, PerfectableSimpleIntervalNumber rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(PerfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => !rhs.Equals(lhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(PerfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => rhs.Equals(lhs);

    /// <inheritdoc/>
    public bool Equals(PerfectableSimpleIntervalNumber number)
        => IsPerfectable(out var thisNumber) && thisNumber == number;
    #endregion

    #region Imperfectable
    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(SimpleIntervalNumber lhs, ImperfectableSimpleIntervalNumber rhs)
        => !lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(SimpleIntervalNumber lhs, ImperfectableSimpleIntervalNumber rhs)
        => lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(ImperfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs)
        => !rhs.Equals(lhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(ImperfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs)
        => rhs.Equals(lhs);

    /// <inheritdoc/>
    public bool Equals(ImperfectableSimpleIntervalNumber number)
        => IsImperfectable(out var thisNumber) && thisNumber == number;
    #endregion

    #region int
    /// <summary>
    /// Determines if the interval number has a value that is not equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(int lhs, SimpleIntervalNumber rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines if the interval number has a value that is equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(int lhs, SimpleIntervalNumber rhs) => lhs == rhs.Value;

    /// <summary>
    /// Determines if the interval number has a value that is not equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(SimpleIntervalNumber lhs, int rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines if the interval number has a value that is equal to the integer passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(SimpleIntervalNumber lhs, int rhs) => lhs.Value == rhs;

    /// <summary>
    /// Determines if this instance has a value equal to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Equals(int value) => Value == value;
    #endregion
    #endregion
    #endregion

    #region Comparison
    #region SimpleIntervalNumber
    /// <summary>
    /// Determines if the left-hand <see cref="SimpleIntervalNumber"/> is greater than the
    /// right-hand <see cref="SimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(SimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if the left-hand <see cref="SimpleIntervalNumber"/> is less than the
    /// right-hand <see cref="SimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(SimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if the left-hand <see cref="SimpleIntervalNumber"/> is greater than or equal to the
    /// right-hand <see cref="SimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(SimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if the left-hand <see cref="SimpleIntervalNumber"/> is less than or equal to the
    /// right-hand <see cref="SimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(SimpleIntervalNumber lhs, SimpleIntervalNumber rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Compares this instance to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(SimpleIntervalNumber other) => Value.CompareTo(other.Value);
    #endregion

    #region int
    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is greater than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is less than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is greater than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >=(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is less than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <=(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than the value of the <see cref="SimpleIntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) < 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than the value of the <see cref="SimpleIntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) > 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than or equal to the value of the
    /// <see cref="SimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator >=(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than or equal to the value of the
    /// <see cref="SimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public static bool operator <=(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) >= 0;

    /// <summary>
    /// Compares the value of this instance to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <seealso cref="Value"/>
    public int CompareTo(int value) => Value.CompareTo(value);
    #endregion
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts an instance of this struct to an <see cref="int"/>.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator int(SimpleIntervalNumber number) => number.Value;

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to an instance of this struct.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidCastException">The <see cref="int"/> value was not in the range 1..7.</exception>
    public static explicit operator SimpleIntervalNumber(int value)
        => value < 1 || value > 7
            ? throw new InvalidCastException(
                "Cannot cast integer outside of the range 1..7 to a simple interval number.")
            : FromValue(value);

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator SimpleIntervalNumber(PerfectableSimpleIntervalNumber number) => new(number);

    /// <summary>
    /// Implicitly converts a <see cref="ImperfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator SimpleIntervalNumber(ImperfectableSimpleIntervalNumber number) => new(number);

    /// <summary>
    /// Explicitly converts an instance of this struct to a <see cref="PerfectableSimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> did not represent a perfectable simple interval number.
    /// </exception>
    public static explicit operator PerfectableSimpleIntervalNumber(SimpleIntervalNumber number)
        => number.IsPerfectable()
            ? number.NonDefaultPerfectable
            : throw new InvalidCastException("Value did not represent a perfectable simple interval number.");

    /// <summary>
    /// Explicitly converts an instance of this struct to a <see cref="ImperfectableSimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> did not represent an imperfectable simple interval number.
    /// </exception>
    public static explicit operator ImperfectableSimpleIntervalNumber(SimpleIntervalNumber number)
        => number.IsImperfectable()
            ? number.InternalNumber.Imperfectable
            : throw new InvalidCastException("Value did not represent an imperfectable simple interval number.");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => IsPerfectable() ? NonDefaultPerfectable.ToString() : InternalNumber.Imperfectable.ToString();
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// Internally represents both explicit simple interval number types as a union.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    private struct InternalNumberStruct
    {
        [FieldOffset(0)]
        public ImperfectableSimpleIntervalNumber Imperfectable;

        [FieldOffset(0)]
        public PerfectableSimpleIntervalNumber Perfectable;
    }
    #endregion
}
#endregion

#region Enums
/// <summary>
/// Static functionality for the <see cref="PerfectableSimpleIntervalNumber"/> enum.
/// </summary>
public static class PerfectableSimpleIntervalNumbers
{
    /// <summary>
    /// Gets the circle of fifths index of a perfect interval numbered with the current instance relative to a
    /// perfect unison.
    /// </summary>
    /// <remarks></remarks>
    /// <param name="pn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(1)]
    internal static int CircleOfFifthsIndex([NamedEnum] this PerfectableSimpleIntervalNumber pn) => pn switch
    {
        Fourth => -1,
        Unison => 0,
        Fifth => 1,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets the <see cref="PerfectableSimpleIntervalNumber"/> that is the inversion of the current instance.
    /// </summary>
    /// <param name="pn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static PerfectableSimpleIntervalNumber Inversion([NamedEnum] this PerfectableSimpleIntervalNumber pn)
        => pn switch
        {
            Unison => Unison,
            Fourth => Fifth,
            Fifth => Fourth,
            _ => throw Undefined,
        };

    /// <summary>
    /// Gets the number of half steps spanning the major version of the current
    /// <see cref="PerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="pn"></param>
    /// <returns></returns>
    [return: NonNegative]
    internal static int PerfectHalfSteps(this PerfectableSimpleIntervalNumber pn) => pn switch
    {
        Unison => 0,
        Fourth => 5,
        Fifth => 7,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets the <see cref="PerfectableSimpleIntervalNumber"/> of a perfect interval with the circle-of-fifths perfect
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any perfect interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.CircleOfFifthsIndex(PerfectableSimpleIntervalNumber)"/>
    internal static PerfectableSimpleIntervalNumber FromCircleOfFifthsIndex(
        [GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(1)] int Index)
        => Index switch
        {
            -1 => Fourth,
            0 => Unison,
            1 => Fifth,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(Index), Index, "Index did not indicate any perfect interval."),
        };

    private static InvalidEnumArgumentException Undefined
        => new($"Undefined {nameof(PerfectableSimpleIntervalNumber)} value.");
}

/// <summary>
/// Represents the number of a perfectable simple interval.
/// </summary>
public enum PerfectableSimpleIntervalNumber : byte
{
    /// <summary>
    /// Represents a unison.
    /// </summary>
    Unison = 1,

    /// <summary>
    /// Represents a fourth.
    /// </summary>
    Fourth = 4,

    /// <summary>
    /// Represents a fifth.
    /// </summary>
    Fifth = 5,
}

/// <summary>
/// Static functionality for the <see cref="ImperfectableSimpleIntervalNumber"/> enum.
/// </summary>
public static class ImperfectableSimpleIntervalNumbers
{
    /// <summary>
    /// Gets the circle of fifths index of a major interval numbered with the current instance relative to a
    /// perfect unison.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)]
    internal static int CircleOfFifthsIndex([NamedEnum] this ImperfectableSimpleIntervalNumber npn)
        => npn switch
        {
            Second => 2,
            Sixth => 3,
            Third => 4,
            Seventh => 5,
            _ => throw Undefined,
        };

    /// <summary>
    /// Gets the <see cref="ImperfectableSimpleIntervalNumber"/> that is the inversion of the current instance.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static ImperfectableSimpleIntervalNumber Inversion([NamedEnum] this ImperfectableSimpleIntervalNumber npn)
        => npn switch
        {
            Second => Seventh,
            Third => Sixth,
            Sixth => Third,
            Seventh => Second,
            _ => throw Undefined,
        };

    /// <summary>
    /// Gets the number of half steps spanning the major version of the current
    /// <see cref="ImperfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    [return: Positive]
    internal static int MajorHalfSteps(this ImperfectableSimpleIntervalNumber npn) => npn switch
    {
        Second => 2,
        Third => 4,
        Sixth => 9,
        Seventh => 11,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets the <see cref="ImperfectableSimpleIntervalNumber"/> of a major interval with the circle-of-fifths perfect
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any major interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.CircleOfFifthsIndex(ImperfectableSimpleIntervalNumber)"/>
    internal static ImperfectableSimpleIntervalNumber FromCircleOfFifthsIndex(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)] int Index)
        => Index switch
        {
            2 => Second,
            3 => Sixth,
            4 => Third,
            5 => Seventh,
            _ => throw new ArgumentOutOfRangeException(
                    nameof(Index), Index, "Index did not indicate any major interval."),
        };

    private static InvalidEnumArgumentException Undefined
        => new($"Undefined {nameof(ImperfectableSimpleIntervalNumber)} value.");
}

/// <summary>
/// Represents the number of an imperfectable simple interval.
/// </summary>
public enum ImperfectableSimpleIntervalNumber : byte
{
    /// <summary>
    /// Represents a second.
    /// </summary>
    Second = 2,

    /// <summary>
    /// Represents a third.
    /// </summary>
    Third = 3,

    /// <summary>
    /// Represents a sixth.
    /// </summary>
    Sixth = 6,

    /// <summary>
    /// Represents a seventh.
    /// </summary>
    Seventh = 7,
}
#endregion

