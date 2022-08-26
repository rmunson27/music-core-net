using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents an accidental of a musical note.
/// </summary>
/// <remarks>
/// The default instance of this struct represents a natural.
/// </remarks>
public readonly record struct Accidental
{
    #region Constants
    /// <summary>
    /// An accidental representing a natural.
    /// </summary>
    public static readonly Accidental Natural = new(0);
    #endregion

    #region Properties
    /// <summary>
    /// Gets the type of this accidental.
    /// </summary>
    public AccidentalType Type => _intValue switch
    {
        < 0 => AccidentalType.Flat,
        0 => AccidentalType.Natural,
        > 0 => AccidentalType.Sharp,
    };

    /// <summary>
    /// Gets an <see cref="int"/> value describing the current instance.
    /// </summary>
    /// <remarks>
    /// The value of this property will be positive if the current instance is sharp, zero if it is natural, and
    /// negative if it is flat.
    /// </remarks>
    public int IntValue => _intValue;

    /// <summary>
    /// Internally represents the value of the accidental.
    /// </summary>
    private readonly int _intValue;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of this struct.
    /// </summary>
    /// <param name="IntValue"></param>
    private Accidental(int IntValue) { this._intValue = IntValue; }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="Accidental"/> described by the magnitude and sign of the <see cref="int"/> passed in.
    /// </summary>
    /// <remarks>
    /// The value returned will be a sharp if <paramref name="IntValue"/> is positive, natural if
    /// <paramref name="IntValue"/> is zero, and flat if <paramref name="IntValue"/> is negative.
    /// </remarks>
    /// <param name="IntValue"></param>
    /// <returns></returns>
    public static Accidental FromIntValue(int IntValue) => new(IntValue);

    /// <summary>
    /// Creates a new <see cref="Accidental"/> representing a sharp with the given integer degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static Accidental Sharp([Positive] int Degree = 1) => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="Accidental"/> representing a flat with the given integer degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static Accidental Flat([Positive] int Degree = 1) => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)));
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is sharp, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsSharp([NonNegative] out int Degree)
    {
        if (IsSharp())
        {
            Degree = _intValue;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is sharp.
    /// </summary>
    /// <returns></returns>
    public bool IsSharp() => Type == AccidentalType.Sharp;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is natural.
    /// </summary>
    /// <returns></returns>
    public bool IsNatural() => Type == AccidentalType.Natural;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsFlat([NonNegative] out int Degree)
    {
        if (IsFlat())
        {
            Degree = -_intValue;
            return true;
        }
        else
        {
            Degree = default;
            return false;
        }
    }

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat.
    /// </summary>
    /// <returns></returns>
    public bool IsFlat() => Type == AccidentalType.Flat;
    #endregion

    #region Modifiers
    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance sharpened by the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public Accidental Sharpen([NonNegative] int Degree = 1)
        => new(_intValue + Throw.IfArgNegative(Degree, nameof(Degree)));

    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance flattened by the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public Accidental Flatten([NonNegative] int Degree = 1)
        => new(_intValue - Throw.IfArgNegative(Degree, nameof(Degree)));

    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance sharpened or flattened by a degree
    /// determined by the magnitude and sign of the integer passed in.
    /// </summary>
    /// <remarks>
    /// The returned value will be sharper than the current instance if <paramref name="Amount"/> is positive,
    /// and flatter if <paramref name="Amount"/> is negative.
    /// </remarks>
    /// <param name="Amount"></param>
    /// <returns></returns>
    public Accidental Shift(int Amount) => new(_intValue + Amount);
    #endregion

    #region Equality
    /// <summary>
    /// Determines whether the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Accidental other) => IntValue == other.IntValue;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => IntValue;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a <see cref="string"/> that represents the current <see cref="Accidental"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _intValue switch
    {
        < 0 => $"Flat {{ Degree = {-_intValue} }}",
        0 => "Natural",
        > 0 => $"Sharp {{ Degree = {_intValue} }}",
    };

    /// <summary>
    /// Gets a musical notation string that represents the current <see cref="Accidental"/>.
    /// </summary>
    /// <returns></returns>
    public string ToMusicalNotationString()
    {
        if (IsSharp(out int sharpDegree))
        {
            var result = new string('x', sharpDegree / 2);
            if (sharpDegree % 2 != 0) result = '#' + result;
            return result;
        }
        else if (IsFlat(out int flatDegree))
        {
            return new string('b', flatDegree);
        }
        else return string.Empty;
    }
    #endregion
    #endregion
}

/// <summary>
/// Extension methods and other static functionality for the <see cref="AccidentalType"/> enum and other
/// related enums.
/// </summary>
public static class AccidentalTypes
{
    /// <summary>
    /// Converts the current instance to an <see cref="AccidentalType"/>.
    /// </summary>
    /// <remarks>
    /// This can be treated as an implicit conversion - all (named) instances of
    /// <see cref="NonNaturalAccidentalType"/> are representable as named instances
    /// of <see cref="AccidentalType"/>.
    /// </remarks>
    /// <param name="type"></param>
    /// <returns></returns>
    public static AccidentalType ToAccidentalType(this NonNaturalAccidentalType type) => (AccidentalType)type;
}

/// <summary>
/// Represents the type of a non-natural accidental.
/// </summary>
public enum NonNaturalAccidentalType : sbyte
{
    /// <summary>
    /// Represents flat accidentals.
    /// </summary>
    Flat = AccidentalType.Flat,

    /// <summary>
    /// Represents sharp accidentals.
    /// </summary>
    Sharp = AccidentalType.Sharp,
}

/// <summary>
/// Represents the type of an accidental.
/// </summary>
public enum AccidentalType : sbyte
{
    /// <summary>
    /// Represents flat accidentals.
    /// </summary>
    Flat = -1,

    /// <summary>
    /// Represents the natural accidental.
    /// </summary>
    Natural = 0,

    /// <summary>
    /// Represents sharp accidentals.
    /// </summary>
    Sharp = 1,
}
