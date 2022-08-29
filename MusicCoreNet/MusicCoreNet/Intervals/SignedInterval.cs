using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents an interval equipped with a sign.
/// </summary>
/// <remarks>
/// The default instance of this struct represents a perfect unison.
/// <para/>
/// All unisons are represented as positive in order to resolve the ambiguity between unison values.
/// Given this fact, all signed intervals are represented exactly once by this struct (without distinct equivalents).
/// </remarks>
public readonly record struct SignedInterval
{
    #region Constants
    /// <summary>
    /// A <see cref="SignedInterval"/> representing a perfect octave.
    /// </summary>
    public static readonly SignedInterval PerfectOctave = Interval.PerfectOctave;

    /// <summary>
    /// A <see cref="SignedInterval"/> representing a perfect unison.
    /// </summary>
    public static readonly SignedInterval PerfectUnison = Interval.PerfectUnison;

    /// <summary>
    /// A <see cref="SignedInterval"/> representing a negative perfect octave.
    /// </summary>
    public static readonly SignedInterval NegativePerfectOctave = Negative(Interval.PerfectOctave);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the interval this instance represents.
    /// </summary>
    public Interval Interval => _interval;
    internal readonly Interval _interval;

    /// <summary>
    /// Gets the sign of this instance.
    /// </summary>
    public int Sign => _sign == 0 ? 1 : _sign;
    private readonly int _sign;
    #endregion

    #region Constructor
    private SignedInterval(in Interval Interval, int Sign)
    {
        _interval = Interval;
        _sign = Sign;

        // If the interval is a unison, we need sign fixups to handle potential ambiguity (i.e. a diminished unison in
        // the positive direction is equivalent to an augmented unison in the negative direction)
        if (_interval.Number == 1)
        {
            // If the sign is negative, invert the base of the underlying interval and represent as a unison
            // with positive sign
            if (_sign < 0) _interval = _interval with { Base = -_interval.Base };
            _sign = 0;
        }
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="SignedInterval"/> representing the interval passed in in the positive direction.
    /// </summary>
    /// <param name="Interval"></param>
    /// <returns></returns>
    public static SignedInterval Positive(in Interval Interval) => new(in Interval, 1);

    /// <summary>
    /// Creates a new <see cref="SignedInterval"/> representing the interval passed in in the negative direction.
    /// </summary>
    /// <param name="Interval"></param>
    /// <returns></returns>
    public static SignedInterval Negative(in Interval Interval) => new(in Interval, -1);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SignedInterval other) => _sign == other._sign && Interval == other.Interval;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(_sign, _interval);
    #endregion

    #region Arithmetic
    /// <summary>
    /// Computes the difference between the two <see cref="SignedInterval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SignedInterval operator -(in SignedInterval lhs, in SignedInterval rhs)
        => lhs.Sign == rhs.Sign
            ? SubtractRelativeToLeftHandSign(lhs, rhs)
            : new(lhs._interval + rhs._interval, lhs.Sign);

    /// <summary>
    /// Computes the sum of the two <see cref="SignedInterval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SignedInterval operator +(in SignedInterval lhs, in SignedInterval rhs)
        => lhs.Sign == rhs.Sign
            ? new(lhs._interval + rhs._interval, lhs.Sign)
            : SubtractRelativeToLeftHandSign(in lhs, in rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SignedInterval SubtractRelativeToLeftHandSign(in SignedInterval lhs, in SignedInterval rhs)
    {
        Interval.SubtractInPlace(in lhs._interval, in rhs._interval, out var newBase, out var newOctaves);

        if (newOctaves < 0)
        {
            // The final base is the inversion of the original (since we are treating it as an interval down from a
            // given note, rather than an interval up from the note an octave below)
            var finalBase = -newBase;

            // The negative range of values starts at a unison (exclusive) and decreases in octave count by 1
            // every octave
            // For example, the values treated as in the -1 octave range by the SubtractInPlace method are really in
            // the 0 octave range, but with a negative sign EXCEPT the -1 octave itself
            // Correct the range unless the base value is an octave multiple
            var finalOctaves = -newOctaves;
            if (finalBase.Number != 1) finalOctaves--;

            return new(new(finalBase, finalOctaves), -lhs.Sign);
        }
        else return new(new(newBase, newOctaves), lhs.Sign);
    }

    /// <summary>
    /// Negates the <see cref="SignedInterval"/> instance passed in.
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static SignedInterval operator -(in SignedInterval interval)
    {
        return new(in interval._interval, -interval.Sign);
    }
    #endregion

    #region Computation
    /// <summary>
    /// Gets a <see cref="SignedInterval"/> equivalent to the current instance with the quality shifted by the degree
    /// passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public SignedInterval WithQualityShift(int Degree) => new(_interval.WithQualityShift(Degree), Sign);
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="Music.Interval"/> to a positive <see cref="SignedInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator SignedInterval(in Interval interval) => new(in interval, 1);

    /// <summary>
    /// Implicitly converts a <see cref="SimpleIntervalBase"/> to a positive <see cref="SignedInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator SignedInterval(SimpleIntervalBase interval) => new(interval, 1);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var result = _interval.ToString();
        if (_sign < 0) result = "-" + result;
        return result;
    }
    #endregion
    #endregion
}
