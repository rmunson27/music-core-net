using Rem.Core.Attributes;
using Rem.Music.Internal;
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
    #region Constants
    private const string OctaveAbbreviation = "8ve";

    /// <summary>
    /// Represents an octave.
    /// </summary>
    public static readonly IntervalNumber Octave = new(SimpleIntervalNumber.Unison, 1);
    #endregion

    #region Properties
    /// <summary>
    /// Gets the integer value of this number.
    /// </summary>
    [Positive] public int Value => SimpleBase.NumericalValue + AdditionalOctaves * SimpleIntervalNumber.ValuesCount;

    /// <summary>
    /// Gets the perfectability of this instance.
    /// </summary>
    public IntervalPerfectability Perfectability => SimpleBase.Perfectability;

    /// <summary>
    /// Gets whether or not this instance is perfectable.
    /// </summary>
    public bool IsPerfectable => SimpleBase.IsPerfectable();

    /// <summary>
    /// Gets whether or not this instance is imperfectable.
    /// </summary>
    public bool IsImperfectable => SimpleBase.IsImperfectable();

    /// <summary>
    /// Gets an abbreviation representing this instance.
    /// </summary>
    /// <remarks>
    /// Unisons cannot be abbreviated, so the string "Unison" will be returned when this property is accessed.
    /// </remarks>
    public string Abbreviation
    {
        get
        {
            if (AdditionalOctaves == 0) return SimpleBase.Abbreviation; // Simple interval number
            else if (SimpleBase == SimpleIntervalNumber.Unison) // Octave multiple
            {
                return AdditionalOctaves == 1 ? OctaveAbbreviation : $"{AdditionalOctaves} * {OctaveAbbreviation}"; 
            }
            else
            {
                var numericalValue = NumericalValue;
                return $"{numericalValue}{numericalValue.OrdinalSuffix()}";
            }
        }
    }

    /// <summary>
    /// Gets the numerical value of this instance.
    /// </summary>
    public int NumericalValue => SimpleBase.NumericalValue + 7 * AdditionalOctaves;

    /// <summary>
    /// Gets the <see cref="SimpleIntervalNumber"/> base that this instance adds octaves to.
    /// </summary>
    public SimpleIntervalNumber SimpleBase { get; }

    /// <summary>
    /// Gets the number of additional octaves added onto <see cref="SimpleBase"/>.
    /// </summary>
    [NonNegative] public int AdditionalOctaves { get; }
    #endregion

    #region Constructors And Factory Methods
    /// <summary>
    /// Creates a new <see cref="IntervalNumber"/> representing the specified number of octaves.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> was negative.</exception>
    public static IntervalNumber Octaves([NonNegative] int count) => new(SimpleIntervalNumber.Unison, count);

    /// <summary>
    /// Constructs a new instance of the <see cref="IntervalNumber"/> struct representing a simple interval number
    /// with a non-negative number of octaves added.
    /// </summary>
    /// <param name="SimpleBase"></param>
    /// <param name="AdditionalOctaves"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="AdditionalOctaves"/> was negative.</exception>
    public IntervalNumber(SimpleIntervalNumber SimpleBase, [NonNegative] int AdditionalOctaves = 0)
    {
        this.SimpleBase = SimpleBase;
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
        var valueMinus1 = Value - 1;
        SimpleBase = SimpleIntervalNumber.FromNumericalValue(valueMinus1 % SimpleIntervalNumber.ValuesCount + 1);
        AdditionalOctaves = valueMinus1 / SimpleIntervalNumber.ValuesCount;
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
            Number = SimpleBase;
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
        => AdditionalOctaves == other.AdditionalOctaves && SimpleBase == other.SimpleBase;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(SimpleBase, AdditionalOctaves);

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
    public static implicit operator IntervalNumber(PerfectableSimpleIntervalNumber number)
        => new(number);

    /// <summary>
    /// Implicitly converts a named <see cref="ImperfectableSimpleIntervalNumber"/> to an instance of this struct.
    /// </summary>
    /// <exception cref="InvalidEnumArgumentException">
    /// The <see cref="ImperfectableSimpleIntervalNumber"/> to convert was an unnamed enum value.
    /// </exception>
    /// <param name="number"></param>
    public static implicit operator IntervalNumber(ImperfectableSimpleIntervalNumber number)
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

