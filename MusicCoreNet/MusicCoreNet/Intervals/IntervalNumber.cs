﻿using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using static PerfectableSimpleIntervalNumber;
using static NonPerfectableSimpleIntervalNumber;

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
    /// Gets whether or not this instance is non-perfectable.
    /// </summary>
    public bool IsNonPerfectable => Base.IsNonPerfectable();

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
    /// Implicitly converts a <see cref="NonPerfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <exception cref="InvalidEnumArgumentException">
    /// The <see cref="NonPerfectableSimpleIntervalNumber"/> to convert was an unnamed enum value.
    /// </exception>
    /// <param name="number"></param>
    public static implicit operator IntervalNumber([NamedEnum] NonPerfectableSimpleIntervalNumber number)
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
/// Represents the number of a simple interval, that can be either perfectable or non-perfectable.
/// </summary>
/// <remarks>
/// The default value of this struct represents a unison.
/// </remarks>
public readonly record struct SimpleIntervalNumber
    : IEquatable<PerfectableSimpleIntervalNumber>, IEquatable<NonPerfectableSimpleIntervalNumber>, IEquatable<int>,
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
    /// The <see cref="NonPerfectableSimpleIntervalNumber"/> representing a second.
    /// </summary>
    [NamedEnum] public const NonPerfectableSimpleIntervalNumber Second = NonPerfectableSimpleIntervalNumber.Second;

    /// <summary>
    /// The <see cref="NonPerfectableSimpleIntervalNumber"/> representing a third.
    /// </summary>
    [NamedEnum] public const NonPerfectableSimpleIntervalNumber Third = NonPerfectableSimpleIntervalNumber.Third;

    /// <summary>
    /// The <see cref="NonPerfectableSimpleIntervalNumber"/> representing a sixth.
    /// </summary>
    [NamedEnum] public const NonPerfectableSimpleIntervalNumber Sixth = NonPerfectableSimpleIntervalNumber.Sixth;

    /// <summary>
    /// The <see cref="NonPerfectableSimpleIntervalNumber"/> representing a seventh.
    /// </summary>
    [NamedEnum] public const NonPerfectableSimpleIntervalNumber Seventh = NonPerfectableSimpleIntervalNumber.Seventh;
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the number of half steps in a perfect or major interval numbered with this instance depending on whether
    /// or not it is perfectable.
    /// </summary>
    [NonNegative] internal int PerfectOrMajorHalfSteps => IsPerfectable()
                                                            ? NonDefaultPerfectable.PerfectHalfSteps()
                                                            : InternalNumber.NonPerfectable.MajorHalfSteps();

    /// <summary>
    /// Gets the integer value of this number.
    /// </summary>
    [Positive] public int Value => IsPerfectable() ? (int)NonDefaultPerfectable : (int)InternalNumber.NonPerfectable;

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
    /// <see cref="NonPerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="Number"/> was an unnamed enum value.</exception>
    public SimpleIntervalNumber([NamedEnum] NonPerfectableSimpleIntervalNumber Number)
    {
        InternalNumber = new();
        InternalNumber.NonPerfectable = Throw.IfEnumArgUnnamed(Number, nameof(Number));
        Perfectability = NonPerfectable;
    }
    #endregion

    #region Methods
    #region Factory
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
        >= 2 and <= 5 => NonPerfectableSimpleIntervalNumbers.FromCircleOfFifthsIndex(Index),
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
        2 or 3 or 6 or 7 => (NonPerfectableSimpleIntervalNumber)Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, "Value was not in the range 1..7."),
    };
    #endregion

    #region Classification
    #region Perfectable
    /// <summary>
    /// Gets whether or not this instance is perfectable, setting <paramref name="Perfectable"/> to the perfectable
    /// number value if so and setting <paramref name="NonPerfectable"/> to the non-perfectable value otherwise.
    /// </summary>
    /// <param name="Perfectable"></param>
    /// <param name="NonPerfectable"></param>
    /// <returns></returns>
    public bool IsPerfectable(
        out PerfectableSimpleIntervalNumber Perfectable, out NonPerfectableSimpleIntervalNumber NonPerfectable)
    {
        if (IsPerfectable())
        {
            Perfectable = NonDefaultPerfectable;
            NonPerfectable = default;
            return true;
        }
        else
        {
            Perfectable = default;
            NonPerfectable = InternalNumber.NonPerfectable;
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

    #region Non-Perfectable
    /// <summary>
    /// Gets whether or not this instance is non-perfectable, setting the non-perfectable number value in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public bool IsNonPerfectable(out NonPerfectableSimpleIntervalNumber Number)
    {
        if (IsNonPerfectable())
        {
            Number = InternalNumber.NonPerfectable;
            return true;
        }
        else
        {
            Number = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not this instance is non-perfectable.
    /// </summary>
    /// <returns></returns>
    public bool IsNonPerfectable() => Perfectability == NonPerfectable;
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
                                            : InternalNumber.NonPerfectable.CircleOfFifthsIndex();

    /// <summary>
    /// Gets the inversion of the current instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalNumber Inversion() => IsPerfectable()
                                                ? new(NonDefaultPerfectable.Inversion())
                                                : new(InternalNumber.NonPerfectable.Inversion());
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
                _ => InternalNumber.NonPerfectable == other.InternalNumber.NonPerfectable,
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
                                            : InternalNumber.NonPerfectable.GetHashCode();

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

    #region Non-Perfectable
    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(SimpleIntervalNumber lhs, NonPerfectableSimpleIntervalNumber rhs)
        => !lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(SimpleIntervalNumber lhs, NonPerfectableSimpleIntervalNumber rhs)
        => lhs.Equals(rhs);

    /// <summary>
    /// Determines if the interval numbers passed in are not equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator !=(NonPerfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs)
        => !rhs.Equals(lhs);

    /// <summary>
    /// Determines if the interval numbers passed in are equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator ==(NonPerfectableSimpleIntervalNumber lhs, SimpleIntervalNumber rhs)
        => rhs.Equals(lhs);

    /// <inheritdoc/>
    public bool Equals(NonPerfectableSimpleIntervalNumber number)
        => IsNonPerfectable(out var thisNumber) && thisNumber == number;
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
    /// Implicitly converts a <see cref="NonPerfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    public static implicit operator SimpleIntervalNumber(NonPerfectableSimpleIntervalNumber number) => new(number);

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
    /// Explicitly converts an instance of this struct to a <see cref="NonPerfectableSimpleIntervalNumber"/>.
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="InvalidCastException">
    /// <paramref name="number"/> did not represent a non-perfectable simple interval number.
    /// </exception>
    public static explicit operator NonPerfectableSimpleIntervalNumber(SimpleIntervalNumber number)
        => number.IsNonPerfectable()
            ? number.InternalNumber.NonPerfectable
            : throw new InvalidCastException("Value did not represent a non-perfectable simple interval number.");
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => IsPerfectable() ? NonDefaultPerfectable.ToString() : InternalNumber.NonPerfectable.ToString();
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
        public NonPerfectableSimpleIntervalNumber NonPerfectable;

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
/// Static functionality for the <see cref="PerfectableSimpleIntervalNumber"/> enum.
/// </summary>
public static class NonPerfectableSimpleIntervalNumbers
{
    /// <summary>
    /// Gets the circle of fifths index of a major interval numbered with the current instance relative to a
    /// perfect unison.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: GreaterThanOrEqualToInteger(2), LessThanOrEqualToInteger(5)]
    internal static int CircleOfFifthsIndex([NamedEnum] this NonPerfectableSimpleIntervalNumber npn)
        => npn switch
        {
            Second => 2,
            Sixth => 3,
            Third => 4,
            Seventh => 5,
            _ => throw Undefined,
        };

    /// <summary>
    /// Gets the <see cref="NonPerfectableSimpleIntervalNumber"/> that is the inversion of the current instance.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static NonPerfectableSimpleIntervalNumber Inversion([NamedEnum] this NonPerfectableSimpleIntervalNumber npn)
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
    /// <see cref="NonPerfectableSimpleIntervalNumber"/> passed in.
    /// </summary>
    /// <param name="npn"></param>
    /// <returns></returns>
    [return: Positive]
    internal static int MajorHalfSteps(this NonPerfectableSimpleIntervalNumber npn) => npn switch
    {
        Second => 2,
        Third => 4,
        Sixth => 9,
        Seventh => 11,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets the <see cref="NonPerfectableSimpleIntervalNumber"/> of a major interval with the circle-of-fifths perfect
    /// unison-based index passed in.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="Index"/> did not indicate any major interval.
    /// </exception>
    /// <seealso cref="SimpleIntervalNumbers.CircleOfFifthsIndex(NonPerfectableSimpleIntervalNumber)"/>
    internal static NonPerfectableSimpleIntervalNumber FromCircleOfFifthsIndex(
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
        => new($"Undefined {nameof(NonPerfectableSimpleIntervalNumber)} value.");
}

/// <summary>
/// Represents the number of a non-perfectable simple interval.
/// </summary>
public enum NonPerfectableSimpleIntervalNumber : byte
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

/// <summary>
/// Represents the perfectability of an interval.
/// </summary>
public enum IntervalPerfectability : byte
{
    /// <summary>
    /// Represents perfectable intervals (i.e. a perfect fourth).
    /// </summary>
    Perfectable,

    /// <summary>
    /// Represents non-perfectable intervals (i.e. a major second).
    /// </summary>
    NonPerfectable,
}
#endregion

