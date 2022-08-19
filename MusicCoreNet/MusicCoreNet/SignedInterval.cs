using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents an interval equipped with a sign.
/// </summary>
public sealed record class SignedInterval
{
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
