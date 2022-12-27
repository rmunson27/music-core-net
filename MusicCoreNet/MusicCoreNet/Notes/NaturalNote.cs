using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a note written as a natural.
/// </summary>
/// <param name="Letter">The letter of the note.</param>
/// <param name="Octave">The octave of the note.</param>
public readonly record struct NaturalNote(NoteLetter Letter, int Octave) : IEquatable<Note>
{
    /// <summary>
    /// Gets the spelling of this note.
    /// </summary>
    /// <remarks>
    /// This will always have a natural accidental.
    /// </remarks>
    public NoteSpelling Spelling => Letter;

    /// <inheritdoc cref="Note.Pitch"/>
    public NotePitch Pitch => new(Letter.PitchClass, Octave);

    /// <summary>
    /// Determines if this instance is equal to another <see cref="NaturalNote"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NaturalNote other) => Octave == other.Octave && Letter == other.Letter;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Letter, Octave);

    /// <summary>
    /// Determines if this instance is equal to another <see cref="Note"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Note other)
        => Octave == other.Octave && Letter == other.Letter && other.Accidental == Accidental.Natural;

    /// <summary>
    /// Determines if this <see cref="NaturalNote"/> is enharmonically equivalent to another <see cref="Note"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnharmonicallyEquivalentTo(Note other) => Pitch == other.Pitch;

    /// <summary>
    /// Explicitly converts a <see cref="Note"/> to a <see cref="NaturalNote"/>.
    /// </summary>
    /// <param name="note"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator NaturalNote(Note note)
        => note.IsNatural(out var naturalNote) ? naturalNote : throw new InvalidCastException("Note is not natural.");
}
