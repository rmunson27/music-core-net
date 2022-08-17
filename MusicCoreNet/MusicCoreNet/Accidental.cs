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
    /// Gets an <see cref="int"/> value describing the current instance.
    /// </summary>
    /// <remarks>
    /// The value of this property will be positive if the current instance is sharp, zero if it is natural, and
    /// negative if it is flat.
    /// </remarks>
    public int AsInt => _intValue;

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
    /// Gets whether or not the current <see cref="Accidental"/> is sharp.
    /// </summary>
    /// <returns></returns>
    public bool IsSharp() => _intValue > 0;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is sharp, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsSharp([NonNegative] out int Degree)
    {
        if (_intValue > 0)
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
    /// Gets whether or not the current <see cref="Accidental"/> is natural.
    /// </summary>
    /// <returns></returns>
    public bool IsNatural() => _intValue == 0;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat.
    /// </summary>
    /// <returns></returns>
    public bool IsFlat() => _intValue < 0;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsFlat([NonNegative] out int Degree)
    {
        if (_intValue < 0)
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
    #endregion
    #endregion
}

/// <summary>
/// Represents the type of an accidental.
/// </summary>
public enum AccidentalType : sbyte
{
    Flat = -1,
    Natural = 0,
    Sharp = 1,
}
