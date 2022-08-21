using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    #region Properties And Fields
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
    /// Gets the circle of fifths index of this instance relative to C natural.
    /// </summary>
    internal int CircleOfFifthsIndexRelativeToC => Letter switch
    {
        NoteLetter.A => 3,
        NoteLetter.B => 5,
        NoteLetter.C => 0,
        NoteLetter.D => 2,
        NoteLetter.E => 4,
        NoteLetter.F => -1,
        _ => 1,
    } + Accidental.IntValue * 7;
    #endregion

    #region Methods
    #region Arithmetic
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
                => pi with { Quality = pi.Quality.Shift(lhs.Accidental.IntValue - rhs.Accidental.IntValue) },
            NonPerfectableSimpleIntervalBase npi
                => npi with { Quality = npi.Quality.Shift(lhs.Accidental.IntValue - rhs.Accidental.IntValue) },
        };
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NoteClass other) => Letter == other.Letter && Accidental == other.Accidental;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Letter, Accidental);
    #endregion

    #region Conversion
    /// <summary>
    /// Gets the <see cref="Note"/> with the current <see cref="NoteClass"/> in the specified numbered octave.
    /// </summary>
    /// <param name="Octave"></param>
    /// <returns></returns>
    public Note WithOctave(int Octave) => new(this, Octave);

    /// <summary>
    /// Implicitly converts a <see cref="NoteLetter"/> to an instance of this type.
    /// </summary>
    /// <param name="letter"></param>
    public static implicit operator NoteClass(NoteLetter letter) => new(letter);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(NoteClass)} {{ Letter = {Letter}, Accidental = {Accidental} }}";

    /// <summary>
    /// Gets a musical notation string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public string ToMusicalNotationString() => $"{Letter}{Accidental.ToMusicalNotationString()}";
    #endregion
    #endregion
}
