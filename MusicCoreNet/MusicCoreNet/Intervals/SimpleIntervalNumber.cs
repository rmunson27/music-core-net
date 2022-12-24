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
    /// The <see cref="SimpleIntervalNumber"/> representing a fourth.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Fourth = PerfectableSimpleIntervalNumber.Fourth;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a unison.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Unison = PerfectableSimpleIntervalNumber.Unison;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a fifth.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Fifth = PerfectableSimpleIntervalNumber.Fifth;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a second.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Second = ImperfectableSimpleIntervalNumber.Second;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a third.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Third = ImperfectableSimpleIntervalNumber.Third;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a sixth.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Sixth = ImperfectableSimpleIntervalNumber.Sixth;

    /// <summary>
    /// The <see cref="SimpleIntervalNumber"/> representing a seventh.
    /// </summary>
    [NamedEnum] public static readonly SimpleIntervalNumber Seventh = ImperfectableSimpleIntervalNumber.Seventh;
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the number of half steps in a perfect or major interval numbered with this instance depending on whether
    /// or not it is perfectable.
    /// </summary>
    [NonNegative] internal int PerfectOrMajorHalfSteps => IsPerfectable()
                                                            ? InternalNumber.Perfectable.PerfectHalfSteps
                                                            : InternalNumber.Imperfectable.MajorHalfSteps;

    /// <summary>
    /// Gets the circle of fifths index of a perfect or major interval numbered with the current instance relative to
    /// a perfect unison.
    /// </summary>
    /// <remarks>
    /// The interval used in the comparison will be perfect if the current instance is perfectable and major otherwise.
    /// </remarks>
    /// <returns></returns>
    internal int CircleOfFifthsIndex => IsPerfectable()
                                            ? InternalNumber.Perfectable.CircleOfFifthsIndex
                                            : InternalNumber.Imperfectable.CircleOfFifthsIndex;

    /// <summary>
    /// Gets the numerical value of this instance.
    /// </summary>
    [GreaterThanOrEqualToInteger(1), LessThanOrEqualToInteger(7)]
    public int NumericalValue => IsPerfectable()
                                    ? InternalNumber.Perfectable.NumericalValue
                                    : (int)InternalNumber.Imperfectable.NumericalValue;

    /// <summary>
    /// Gets the perfectability of this instance.
    /// </summary>
    public IntervalPerfectability Perfectability { get; }

    private readonly InternalNumberStruct InternalNumber;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new instance of the <see cref="SimpleIntervalNumber"/> struct representing the
    /// <see cref="PerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="Number"/> was an unnamed enum value.</exception>
    public SimpleIntervalNumber(PerfectableSimpleIntervalNumber Number)
    {
        InternalNumber = new() { Perfectable = Number };
        Perfectability = Perfectable;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="SimpleIntervalNumber"/> struct representing the
    /// <see cref="ImperfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="Number"></param>
    public SimpleIntervalNumber(ImperfectableSimpleIntervalNumber Number)
    {
        InternalNumber = new() { Imperfectable = Number };
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
    internal static SimpleIntervalNumber OfSimplestIntervalWithHalfSteps(
        [NonNegative, LessThanInteger(12)] int halfSteps, PeripheralIntervalQualityKind tritoneQualityType)
        => OfSimplestIntervalWithHalfSteps(halfSteps)
            ?? (tritoneQualityType == PeripheralIntervalQualityKind.Augmented
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
        >= -1 and <= 1 => PerfectableSimpleIntervalNumber.FromCircleOfFifthsIndex(Index),
        >= 2 and <= 5 => ImperfectableSimpleIntervalNumber.FromCircleOfFifthsIndex(Index),
        _ => throw new ArgumentOutOfRangeException(
                nameof(Index), Index, "Index did not indicate any perfect or major interval."),
    };

    /// <summary>
    /// Creates a new <see cref="SimpleIntervalNumber"/> from the integer value passed in.
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Value"/> did not represent a simple interval number.
    /// </exception>
    public static SimpleIntervalNumber FromNumericalValue(
        [GreaterThanOrEqualToInteger(1), LessThanOrEqualToInteger(7)] int Value)
        => Value switch
        {
            1 or 4 or 5 => (PerfectableSimpleIntervalNumber)Value,
            2 or 3 or 6 or 7 => (ImperfectableSimpleIntervalNumber)Value,
            _ => throw new ArgumentOutOfRangeException(nameof(Value), Value,
                                                       "Numerical value did not represent a simple interval number."),
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
            Perfectable = InternalNumber.Perfectable;
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
            Number = InternalNumber.Perfectable;
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
    /// Gets the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalNumber Inversion() => IsPerfectable()
                                                ? new(InternalNumber.Perfectable.Inversion)
                                                : new(InternalNumber.Imperfectable.Inversion);
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
            return Perfectability == Perfectable
                    ? InternalNumber.Perfectable == other.InternalNumber.Perfectable
                    : InternalNumber.Imperfectable == other.InternalNumber.Imperfectable;
        }
        else return false;
    }

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => IsPerfectable()
                                            ? InternalNumber.Perfectable.GetHashCode()
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
    public static bool operator ==(int lhs, SimpleIntervalNumber rhs) => lhs == rhs.NumericalValue;

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
    public static bool operator ==(SimpleIntervalNumber lhs, int rhs) => lhs.NumericalValue == rhs;

    /// <summary>
    /// Determines if this instance has a value equal to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Equals(int value) => NumericalValue == value;
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
    public int CompareTo(SimpleIntervalNumber other) => NumericalValue.CompareTo(other.NumericalValue);
    #endregion

    #region int
    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is greater than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator >(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is less than the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator <(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is greater than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator >=(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if the <see cref="SimpleIntervalNumber"/> passed in has a value that is less than or equal to the
    /// <see cref="int"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator <=(SimpleIntervalNumber lhs, int rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than the value of the <see cref="SimpleIntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator >(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) < 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than the value of the <see cref="SimpleIntervalNumber"/>
    /// passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator <(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) > 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is greater than or equal to the value of the
    /// <see cref="SimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator >=(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) <= 0;

    /// <summary>
    /// Determines if the <see cref="int"/> passed in is less than or equal to the value of the
    /// <see cref="SimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public static bool operator <=(int lhs, SimpleIntervalNumber rhs) => rhs.CompareTo(lhs) >= 0;

    /// <summary>
    /// Compares the value of this instance to the integer passed in.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <seealso cref="NumericalValue"/>
    public int CompareTo(int value) => NumericalValue.CompareTo(value);
    #endregion
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts an instance of this struct to an <see cref="int"/>.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator int(SimpleIntervalNumber number) => number.NumericalValue;

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to an instance of this struct.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidCastException">The <see cref="int"/> value was not in the range 1..7.</exception>
    public static explicit operator SimpleIntervalNumber(int value)
        => value < 1 || value > 7
            ? throw new InvalidCastException(
                "Cannot cast integer outside of the range 1..7 to a simple interval number.")
            : FromNumericalValue(value);

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
            ? number.InternalNumber.Perfectable
            : throw new InvalidCastException("NumericalValue did not represent a perfectable simple interval number.");

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
            : throw new InvalidCastException("NumericalValue did not represent an imperfectable simple interval number.");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => IsPerfectable() ? InternalNumber.Perfectable.ToString() : InternalNumber.Imperfectable.ToString();
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

#region Specific Perfectability
/// <summary>
/// Represents the number of a perfectable simple interval.
/// </summary>
/// <remarks>
/// The default value of this struct represents a unison.
/// </remarks>
public readonly record struct PerfectableSimpleIntervalNumber
{
    #region Constants
    /// <summary>
    /// The offset between the numerical value of <see cref="Values.Unison"/> and the numerical value
    /// of <see cref="Unison"/>.
    /// </summary>
    /// <remarks>
    /// This is required so that the <see cref="Unison"/> property, with a <see cref="Value"/> property of
    /// <see cref="Values.Unison"/>, can be the default, and assigns the numerical values of the <see cref="Values"/>
    /// instances to the intended numerical value of the corresponding <see cref="PerfectableSimpleIntervalNumber"/>
    /// instance minus this number.
    /// </remarks>
    private const byte ValuesToNumericalValueAdd = 1;

    /// <inheritdoc cref="Values.Unison"/>
    /// <remarks>
    /// This is the default value of this struct.
    /// </remarks>
    public static readonly PerfectableSimpleIntervalNumber Unison = new(Values.Unison);

    /// <inheritdoc cref="Values.Fourth"/>
    public static readonly PerfectableSimpleIntervalNumber Fourth = new(Values.Fourth);

    /// <inheritdoc cref="Values.Fifth"/>
    public static readonly PerfectableSimpleIntervalNumber Fifth = new(Values.Fifth);
    #endregion

    #region Properties
    /// <summary>
    /// Gets the circle of fifths index of a perfect interval numbered with this instance relative to a
    /// perfect unison.
    /// </summary>
    [GreaterThanOrEqualToInteger(-1), LessThanOrEqualToInteger(1)]
    internal int CircleOfFifthsIndex => Value switch
    {
        Values.Fourth => -1,
        Values.Unison => 0,
        _ => 1, // Values.Fifth is only option
    };

    /// <summary>
    /// Gets the <see cref="PerfectableSimpleIntervalNumber"/> that is the inversion of the this instance.
    /// </summary>
    public PerfectableSimpleIntervalNumber Inversion => Value switch
    {
        Values.Unison => Unison,
        Values.Fourth => Fifth,
        _ => Fourth, // Values.Fifth is only option
    };

    /// <summary>
    /// Gets the number of half steps spanning the perfect version of the <see cref="PerfectableSimpleIntervalNumber"/>
    /// passed in.
    /// </summary>
    [NonNegative]
    internal int PerfectHalfSteps => Value switch
    {
        Values.Unison => 0,
        Values.Fourth => 5,
        _ => 7, // Values.Fifth is only option
    };

    /// <summary>
    /// Gets the numerical value of this instance.
    /// </summary>
    [GreaterThanOrEqualToInteger(1), LessThanOrEqualToInteger(5)]
    public byte NumericalValue => unchecked((byte)((int)Value + ValuesToNumericalValueAdd));

    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely describing this instance.
    /// </summary>
    [NameableEnum] public Values Value { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    private PerfectableSimpleIntervalNumber([NameableEnum] Values Value) { this.Value = Value; }
    #endregion

    #region Methods
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
            _ => throw new ArgumentOutOfRangeException(nameof(Index), Index,
                                                       "Index did not indicate any perfect interval."),
        };

    /// <summary>
    /// Creates a new <see cref="PerfectableSimpleIntervalNumber"/> from the numerical value representing it.
    /// </summary>
    /// <param name="value">
    /// The numerical value of the <see cref="PerfectableSimpleIntervalNumber"/> to create.
    /// <para/>
    /// This value must be 1, 4 or 5, or this method will throw an exception.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> was not 1, 4 or 5.
    /// </exception>
    public static PerfectableSimpleIntervalNumber FromNumericalValue(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)] int value)
        => value switch
        {
            1 => Unison,
            4 => Fourth,
            5 => Fifth,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value,
                                                       "Numerical value did not represent any perfectable interval.")
        };

    /// <summary>
    /// Determines if this instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PerfectableSimpleIntervalNumber other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (int)Value + ValuesToNumericalValueAdd;

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Implicitly converts a <see cref="PerfectableSimpleIntervalNumber"/> to a <see cref="byte"/>.
    /// </summary>
    /// <param name="value"></param>
    [return: GreaterThanOrEqualToInteger(1), LessThanOrEqualToInteger(5)]
    public static implicit operator byte(PerfectableSimpleIntervalNumber value) => value.NumericalValue;

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to a <see cref="PerfectableSimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value was not one of 1, 4 or 5.
    /// </exception>
    public static explicit operator PerfectableSimpleIntervalNumber(
        [GreaterThanOrEqualToInteger(1), LessThanOrEqualToInteger(5)] int value)
        => FromNumericalValue(value);
    #endregion

    #region Backing Enum
    /// <summary>
    /// Represents all possible values of type <see cref="PerfectableSimpleIntervalNumber"/>.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents a unison.
        /// </summary>
        Unison = 0,

        /// <summary>
        /// Represents a fourth.
        /// </summary>
        Fourth = 3,

        /// <summary>
        /// Represents a fifth.
        /// </summary>
        Fifth = 4,
    }
    #endregion
}

/// <summary>
/// Represents the number of an imperfectable simple interval.
/// </summary>
public readonly record struct ImperfectableSimpleIntervalNumber
{
    #region Constants
    /// <summary>
    /// The offset between the numerical value of <see cref="Values.Second"/> and the numerical value
    /// of <see cref="Second"/>.
    /// </summary>
    /// <remarks>
    /// This is required so that the <see cref="Second"/> property, with a <see cref="Value"/> property of
    /// <see cref="Values.Second"/>, can be the default, and assigns the numerical values of the <see cref="Values"/>
    /// instances to the intended numerical value of the corresponding <see cref="ImperfectableSimpleIntervalNumber"/>
    /// instance minus this number.
    /// </remarks>
    private const byte ValuesToNumericalValueAdd = 2;

    /// <inheritdoc cref="Values.Second"/>
    /// <remarks>
    /// This is the default value of this struct.
    /// </remarks>
    public static readonly ImperfectableSimpleIntervalNumber Second = new(Values.Second);

    /// <inheritdoc cref="Values.Third"/>
    public static readonly ImperfectableSimpleIntervalNumber Third = new(Values.Third);

    /// <inheritdoc cref="Values.Sixth"/>
    public static readonly ImperfectableSimpleIntervalNumber Sixth = new(Values.Sixth);

    /// <inheritdoc cref="Values.Seventh"/>
    public static readonly ImperfectableSimpleIntervalNumber Seventh = new(Values.Seventh);
    #endregion

    #region Properties
    /// <summary>
    /// Gets the circle of fifths index of a perfect interval numbered with this instance relative to a
    /// perfect unison.
    /// </summary>
    [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)]
    internal int CircleOfFifthsIndex => Value switch
    {
        Values.Second => 2,
        Values.Third => 4,
        Values.Sixth => 3,
        _ => 5, // Values.Seventh is only option
    };

    /// <summary>
    /// Gets the <see cref="ImperfectableSimpleIntervalNumber"/> that is the inversion of the this instance.
    /// </summary>
    public ImperfectableSimpleIntervalNumber Inversion => Value switch
    {
        Values.Second => Seventh,
        Values.Third => Sixth,
        Values.Sixth => Third,
        _ => Second, // Values.Seventh is only option
    };

    /// <summary>
    /// Gets the number of half steps spanning the major version of the
    /// <see cref="ImperfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(11)]
    internal int MajorHalfSteps => Value switch
    {
        Values.Second => 2,
        Values.Third => 4,
        Values.Sixth => 9,
        _ => 11, // Values.Seventh is only option
    };

    /// <summary>
    /// Gets the numerical value of this instance.
    /// </summary>
    [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(7)]
    public byte NumericalValue => unchecked((byte)((int)Value + ValuesToNumericalValueAdd));

    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely describing this instance.
    /// </summary>
    public Values Value { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance with the supplied value.
    /// </summary>
    /// <param name="Value"></param>
    private ImperfectableSimpleIntervalNumber([NameableEnum] Values Value) { this.Value = Value; }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the <see cref="ImperfectableSimpleIntervalNumber"/> of a major interval with the circle-of-fifths
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any major interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.CircleOfFifthsIndex(PerfectableSimpleIntervalNumber)"/>
    internal static ImperfectableSimpleIntervalNumber FromCircleOfFifthsIndex(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)] int Index)
        => Index switch
        {
            2 => Second,
            3 => Sixth,
            4 => Third,
            5 => Seventh,
            _ => throw new ArgumentOutOfRangeException(nameof(Index), Index,
                                                       "Index did not indicate any major interval."),
        };

    /// <summary>
    /// Creates a new <see cref="ImperfectableSimpleIntervalNumber"/> from the numerical value representing it.
    /// </summary>
    /// <param name="value">
    /// The numerical value of the <see cref="ImperfectableSimpleIntervalNumber"/> to create.
    /// <para/>
    /// This value must be 2, 3, 6 or 7, or this method will throw an exception.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> was not 2, 3, 6 or 7.
    /// </exception>
    public static ImperfectableSimpleIntervalNumber FromNumericalValue(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(7)] int value)
        => value switch
        {
            2 => Second,
            3 => Third,
            6 => Sixth,
            7 => Seventh,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value,
                                                       "Numerical value did not represent any imperfectable interval.")
        };

    /// <summary>
    /// Determines if this instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ImperfectableSimpleIntervalNumber other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (int)Value + ValuesToNumericalValueAdd;

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Implicitly converts an <see cref="ImperfectableSimpleIntervalNumber"/> to a <see cref="byte"/>.
    /// </summary>
    /// <param name="value"></param>
    [return: GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(7)]
    public static implicit operator byte(ImperfectableSimpleIntervalNumber value) => value.NumericalValue;

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to an <see cref="ImperfectableSimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value was not one of 2, 3, 6 or 7.
    /// </exception>
    public static explicit operator ImperfectableSimpleIntervalNumber(
        [GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(7)] int value)
        => FromNumericalValue(value);
    #endregion

    #region Backing Enum
    /// <summary>
    /// Represents all possible values of type <see cref="ImperfectableSimpleIntervalNumber"/>.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents a second.
        /// </summary>
        Second = 0,

        /// <summary>
        /// Represents a third.
        /// </summary>
        Third = 1,

        /// <summary>
        /// Represents a sixth.
        /// </summary>
        Sixth = 4,

        /// <summary>
        /// Represents a seventh.
        /// </summary>
        Seventh = 5,
    }
    #endregion
}
#endregion
