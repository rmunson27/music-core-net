using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
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
public sealed record class SignedInterval
{
    #region Constants
    /// <summary>
    /// A <see cref="SignedInterval"/> representing a perfect octave.
    /// </summary>
    public static readonly SignedInterval PerfectOctave = Interval.PerfectOctave;

    /// <summary>
    /// A <see cref="SignedInterval"/> representing a perfect unison.
    /// </summary>
    public static readonly SignedInterval PerfectUnison = Positive(SimpleIntervalBase.PerfectUnison);

    /// <summary>
    /// A <see cref="SignedInterval"/> representing a negative perfect octave.
    /// </summary>
    public static readonly SignedInterval NegativePerfectOctave = Negative(Interval.PerfectOctave);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the interval this instance represents.
    /// </summary>
    [NonDefaultableStruct] public Interval Interval { get; }

    /// <summary>
    /// Gets the sign of this instance.
    /// </summary>
    public int Sign => _sign;
    private readonly sbyte _sign;
    #endregion

    #region Constructor
    private SignedInterval([NonDefaultableStruct] Interval Interval, sbyte Sign)
    {
        // If the interval is a unison, we need sign fixups to handle potential ambiguity (i.e. a diminished unison in
        // the positive direction is equivalent to an augmented unison in the negative direction)
        if (Interval.Number == 1)
        {
            // If the sign is negative, invert the base of the underlying interval and represent as a unison
            // with positive sign
            if (Sign < 0) Interval = Interval with { Base = -Interval.Base };
            Sign = 1;
        }

        this.Interval = Interval;
        _sign = Sign;
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="SignedInterval"/> representing the interval passed in in the positive direction.
    /// </summary>
    /// <param name="Interval"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="Interval"/> was the default.</exception>
    public static SignedInterval Positive([NonDefaultableStruct] Interval Interval)
        => new(Throw.IfStructArgDefault(Interval, nameof(Interval)), 1);

    /// <summary>
    /// Creates a new <see cref="SignedInterval"/> representing the interval passed in in the negative direction.
    /// </summary>
    /// <param name="Interval"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="Interval"/> was the default.</exception>
    public static SignedInterval Negative([NonDefaultableStruct] Interval Interval)
        => new(Throw.IfStructArgDefault(Interval, nameof(Interval)), -1);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SignedInterval? other) => other is not null
                                                    && _sign == other._sign
                                                    && Interval == other.Interval;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(_sign, Interval);
    #endregion

    #region Arithmetic
    /// <summary>
    /// Computes the difference between the two <see cref="SignedInterval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either <paramref name="lhs"/> or <paramref name="rhs"/> was <see langword="null"/>.
    /// </exception>
    public static SignedInterval operator -(SignedInterval lhs, SignedInterval rhs)
    {
        Throw.IfArgNull(lhs, nameof(lhs));
        Throw.IfArgNull(rhs, nameof(rhs));

        return lhs._sign == rhs._sign
                ? SubtractRelativeToLeftHandSign(lhs, rhs)
                : new(lhs.Interval + rhs.Interval, lhs._sign);
    }

    /// <summary>
    /// Computes the sum of the two <see cref="SignedInterval"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either <paramref name="lhs"/> or <paramref name="rhs"/> was <see langword="null"/>.
    /// </exception>
    public static SignedInterval operator +(SignedInterval lhs, SignedInterval rhs)
    {
        Throw.IfArgNull(lhs, nameof(lhs));
        Throw.IfArgNull(rhs, nameof(rhs));

        return lhs._sign == rhs._sign
                ? new(lhs.Interval + rhs.Interval, lhs._sign)
                : SubtractRelativeToLeftHandSign(lhs, rhs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SignedInterval SubtractRelativeToLeftHandSign(SignedInterval lhs, SignedInterval rhs)
    {
        Interval.SubtractInPlace(lhs.Interval, rhs.Interval, out var newBase, out var newOctaves);

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
            if (finalBase.NumberValue != 1) finalOctaves--;

            return new(new(finalBase, finalOctaves), (sbyte)-lhs._sign);
        }
        else return new(new(newBase, newOctaves), lhs._sign);
    }

    /// <summary>
    /// Negates the <see cref="SignedInterval"/> instance passed in.
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="interval"/> was <see langword="null"/>.</exception>
    public static SignedInterval operator -(SignedInterval interval)
    {
        Throw.IfArgNull(interval, nameof(interval));
        return new(interval.Interval, (sbyte)-interval._sign);
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="Music.Interval"/> to a positive <see cref="SignedInterval"/>.
    /// </summary>
    /// <param name="interval"></param>
    public static implicit operator SignedInterval(Interval interval) => new(interval, 1);

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
        var result = Interval.ToString();
        if (_sign < 0) result = "-" + result;
        return result;
    }
    #endregion
    #endregion
}
