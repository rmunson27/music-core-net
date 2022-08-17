using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the class of a written note, independent of octave.
/// </summary>
/// <remarks>
/// The default value of this struct represents A natural.
/// </remarks>
/// <param name="Letter">The letter of the note class to create.</param>
/// <param name="Accidental">The accidental of the note class to create.</param>
/// <exception cref="InvalidEnumArgumentException"><paramref name="Letter"/> was an unnamed enum value.</exception>
public readonly record struct NoteClass(NoteLetter Letter, Accidental Accidental = default)
{
    /// <summary>
    /// Gets or initializes the letter of this note class.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    public NoteLetter Letter
    {
        get => _letter;
        init => _letter = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly NoteLetter _letter = Throw.IfEnumArgUnnamed(Letter, nameof(Letter));

    /// <summary>
    /// Gets the difference between the two <see cref="NoteClass"/> instances passed in as an instance
    /// of <see cref="SimpleIntervalBase"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleIntervalBase operator -(NoteClass lhs, NoteClass rhs)
    {
#pragma warning disable CS8509 // This should handle everything
        return lhs.Letter.Minus(rhs.Letter) switch
#pragma warning restore CS8509
        {
            PerfectableSimpleIntervalBase pi
                => pi with { Quality = pi.Quality.Shift(lhs.Accidental.AsInt - rhs.Accidental.AsInt) },
            NonPerfectableSimpleIntervalBase npi
                => npi with { Quality = npi.Quality.Shift(lhs.Accidental.AsInt - rhs.Accidental.AsInt) },
        };
    }
}
