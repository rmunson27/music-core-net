using Rem.Core.ComponentModel;
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
public readonly record struct NotePitchInfo(NotePitchClass Class, int Octave)
{
    /// <summary>
    /// Pitch info for the standard concert pitch (A440).
    /// </summary>
    public static readonly NotePitchInfo ConcertPitch = new(NotePitchClass.A, 4);

    /// <summary>
    /// The frequency of the standard concert pitch (A440).
    /// </summary>
    private const double ConcertPitchFrequency = 440;

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
    /// Gets the difference between the pitches represented by the two <see cref="NotePitchInfo"/> instances passed in
    /// in half steps.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static int operator -(NotePitchInfo lhs, NotePitchInfo rhs)
        => lhs.Class.CRelativeIndex() - rhs.Class.CRelativeIndex() + (lhs.Octave - rhs.Octave) * 12;
}
