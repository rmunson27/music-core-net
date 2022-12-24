using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

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
    [Positive] public int Value => Base.NumericalValue + AdditionalOctaves * 7;

    /// <summary>
    /// Gets the perfectability of this instance.
    /// </summary>
    public IntervalPerfectability Perfectability => Base.Perfectability;

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
        Base = SimpleIntervalNumber.FromNumericalValue(base7Value % 7 + 1);
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
    /// Implicitly converts a named <see cref="PerfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <param name="number"></param>
    /// <exception cref="InvalidEnumArgumentException">
    /// The <see cref="PerfectableSimpleIntervalNumber"/> to convert was an unnamed enum value.
    /// </exception>
    public static implicit operator IntervalNumber([NamedEnum] PerfectableSimpleIntervalNumber number) => new(number);

    /// <summary>
    /// Implicitly converts a named <see cref="ImperfectableSimpleIntervalNumber"/> to an instance of this struct.
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
    public static explicit operator IntervalNumber([Positive] int i)
        => i <= 0 ? throw new InvalidCastException("Cannot cast non-positive value to interval number.") : new(i);
    #endregion
    #endregion
}

