using Rem.Core.ComponentModel;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents information for a pitch that can be represented by a note.
/// </summary>
/// <param name="Class">The class of the represented pitch.</param>
/// <param name="Octave">The octave of the represented pitch.</param>
/// <exception cref="InvalidEnumArgumentException"><paramref name="Class"/> was an unnamed enum value.</exception>
public readonly record struct NotePitchInfo(NotePitchClass Class, int Octave) : IComparable<NotePitchInfo>
{
    #region Constants
    /// <summary>
    /// Pitch info for the lowest pitch in the zero octave.
    /// </summary>
    /// <remarks>
    /// As indicated by the name of this field, the class of this pitch is <see cref="NotePitchClass.C"/>.
    /// </remarks>
    public static readonly NotePitchInfo C0 = new(NotePitchClass.C, 0);

    /// <summary>
    /// Pitch info for the standard concert pitch (A440).
    /// </summary>
    public static readonly NotePitchInfo ConcertPitch = new(NotePitchClass.A, 4);

    /// <summary>
    /// The frequency of the standard concert pitch (A440).
    /// </summary>
    private const double ConcertPitchFrequency = 440;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or initializes the class of the represented pitch.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    public NotePitchClass Class
    {
        get => _class;
        init => _class = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly NotePitchClass _class = Throw.IfEnumArgUnnamed(Class, nameof(Class));

    /// <summary>
    /// Gets the frequency of the represented pitch.
    /// </summary>
    public double Frequency => ConcertPitchFrequency * Math.Pow(2, (this - ConcertPitch) / 12.0);

    /// <summary>
    /// Gets an integer index for this instance relative to the pitch <see cref="C0"/>, the lowest pitch in the
    /// zero octave.
    /// </summary>
    public int C0Index => Class.CRelativeIndex() + Octave * 12;
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="NotePitchInfo"/> from the corresponding index relative to <see cref="C0"/>.
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    /// <seealso cref="C0"/>
    /// <seealso cref="C0Index"/>
    public static NotePitchInfo FromC0Index(int Index)
    {
        var octave = Maths.FloorDivRem(Index, 12, out var classCRelativeIndex);
        return new(NotePitchClasses.FromCRelativeIndex(classCRelativeIndex), octave);
    }
    #endregion

    #region Arithmetic
    /// <summary>
    /// Adds the number of semitones passed in to the <see cref="NotePitchInfo"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NotePitchInfo operator +(int lhs, NotePitchInfo rhs) => rhs + lhs;

    /// <summary>
    /// Subtracts the number of semitones passed in to the <see cref="NotePitchInfo"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NotePitchInfo operator -(NotePitchInfo lhs, int rhs) => lhs + (-rhs);

    /// <summary>
    /// Adds the number of semitones passed in to the <see cref="NotePitchInfo"/> passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NotePitchInfo operator +(NotePitchInfo lhs, int rhs) => FromC0Index(lhs.C0Index + rhs);

    /// <summary>
    /// Gets the difference between the pitches represented by the two supplied <see cref="NotePitchInfo"/> instances
    /// in semitones.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static int operator -(NotePitchInfo lhs, NotePitchInfo rhs) => lhs.C0Index - rhs.C0Index;
    #endregion

    #region Comparison
    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(NotePitchInfo lhs, NotePitchInfo rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(NotePitchInfo lhs, NotePitchInfo rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(NotePitchInfo lhs, NotePitchInfo rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(NotePitchInfo lhs, NotePitchInfo rhs) => lhs.CompareTo(rhs) > 0;

    /// <inheritdoc/>
    public int CompareTo(NotePitchInfo other) => Octave - other.Octave switch
    {
        < 0 => -1,
        0 => Math.Sign(Class.CRelativeIndex() - other.Class.CRelativeIndex()),
        > 0 => 1,
    };
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Pitch {{ Class = {Class}, Octave = {Octave} }}";
    #endregion
    #endregion
}
