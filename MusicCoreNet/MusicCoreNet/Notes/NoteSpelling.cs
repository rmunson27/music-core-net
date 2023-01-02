using Rem.Core.Attributes;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the spelling of a note.
/// </summary>
/// <remarks>
/// The default value of this struct represents a 'C' natural spelling.
/// </remarks>
/// <param name="Letter">The letter of the note spelling to create.</param>
/// <param name="Accidental">The accidental of the note spelling to create.</param>
public readonly record struct NoteSpelling(NoteLetter Letter, Accidental Accidental = default)
{
    #region Properties And Fields
    /// <summary>
    /// Gets the circle of fifths index of this instance relative to C natural.
    /// </summary>
    internal int CircleOfFifthsIndexRelativeToC
        => Letter.CircleOfFifthsIndexRelativeToC + Accidental.Modification * NoteLetter.ValuesCount;

    /// <summary>
    /// Gets the number of half steps a note spelled with this instance is above the 'C' pitch in its octave.
    /// </summary>
    /// <remarks>
    /// This property will return a value greater than or equal to 12 if the accidental is sharp enough to raise the
    /// pitch of a non-'C' note above the 'C' pitch in the octave above, and will return a negative value if the
    /// accidental is flat enough to lower the pitch of a non-'C' note below the 'C' pitch in its octave.
    /// </remarks>
    public int HalfStepsAboveC => Letter.HalfStepsAboveC + Accidental.Modification;

    /// <summary>
    /// Gets the number of half steps a note spelled with this instance is below the 'C' pitch in the octave above.
    /// </summary>
    /// <remarks>
    /// This property will return a negative value if the accidental is sharp enough to raise the pitch of a non-'C'
    /// note above the 'C' pitch in the octave above, and will return a value greater than or equal to 12 if the
    /// accidental is flat enough to lower the pitch of a non-'C' note below the 'C' pitch in its octave.
    /// </remarks>
    public int HalfStepsBelowC => Letter.HalfStepsBelowC - Accidental.Modification;

    /// <summary>
    /// Gets the pitch class of this instance.
    /// </summary>
    public NotePitchClass PitchClass
        => NotePitchClass.FromSemitonesAboveC(
            Maths.FloorRem(Letter.HalfStepsAboveC + Accidental.Modification, NotePitchClass.ValuesCount));
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Gets the <see cref="NoteSpelling"/> with the pitch class passed in that has the simplest possible accidental
    /// (i.e. closest to natural), using the specified <see cref="ModifyingAccidentalKind"/> to assign accidentals
    /// if necessary.
    /// </summary>
    /// <param name="PitchClass">The pitch class of the result.</param>
    /// <param name="AccidentalKind">
    /// The kind of accidentals to assign in ambiguous cases.
    /// <para/>
    /// For example, if <see cref="NotePitchClass.GA"/> is passed in, the result will be G# if
    /// <paramref name="AccidentalKind"/> is set to <see cref="ModifyingAccidentalKind.Sharp"/> and Ab if
    /// it is set to <see cref="ModifyingAccidentalKind.Flat"/>.
    /// </param>
    /// <returns></returns>
    public static NoteSpelling SimplestWithPitchClass(
        NotePitchClass PitchClass, ModifyingAccidentalKind AccidentalKind)
    {
        var sharpResult = AccidentalKind.Value == ModifyingAccidentalKind.Values.Sharp;

        return PitchClass.Value switch
        {
            NotePitchClass.Values.AB => sharpResult ? Note.A.Sharp() : Note.B.Flat(),
            NotePitchClass.Values.CD => sharpResult ? Note.C.Sharp() : Note.D.Flat(),
            NotePitchClass.Values.DE => sharpResult ? Note.D.Sharp() : Note.E.Flat(),
            NotePitchClass.Values.FG => sharpResult ? Note.F.Sharp() : Note.G.Flat(),
            NotePitchClass.Values.GA => sharpResult ? Note.G.Sharp() : Note.A.Flat(),

            var x when x <= NotePitchClass.Values.E => new(new((NoteLetter.Values)((int)x >> 1))),
            var x => unchecked(new(new((NoteLetter.Values)(((int)x + 1) >> 1)))),
        };
    }
    #endregion

    #region Classification
    /// <summary>
    /// Determines whether or not this <see cref="NoteSpelling"/> is sharp, setting the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsSharp([NonNegative] out int Degree) => Accidental.IsSharp(out Degree);

    /// <summary>
    /// Determines whether or not this <see cref="NoteSpelling"/> is sharp.
    /// </summary>
    /// <returns></returns>
    public bool IsSharp() => Accidental.IsSharp();

    /// <summary>
    /// Determines whether or not this <see cref="NoteSpelling"/> is natural.
    /// </summary>
    /// <returns></returns>
    public bool IsNatural() => Accidental.IsNatural();

    /// <summary>
    /// Determines whether or not this <see cref="NoteSpelling"/> is flat, setting the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsFlat([NonNegative] out int Degree) => Accidental.IsFlat(out Degree);

    /// <summary>
    /// Determines whether or not this <see cref="NoteSpelling"/> is flat.
    /// </summary>
    /// <returns></returns>
    public bool IsFlat() => Accidental.IsFlat();
    #endregion

    #region Computation
    /// <summary>
    /// Gets a <see cref="NoteSpelling"/> enharmonically equivalent to the current instance with the accidental simplified
    /// as much as possible (i.e. calling the method on Cb will yield B).
    /// </summary>
    /// <remarks>
    /// The accidental type of the current instance will be used to resolve ambiguity if necessary.
    /// For example, calling this method on G#x will yield A#, whereas calling it on Cbb will yield Bb.
    /// </remarks>
    /// <returns></returns>
    public NoteSpelling SimplifyAccidental()
        => SimplestWithPitchClass(
            PitchClass,

            // Default this parameter if the accidental is natural - it will not be used
            Accidental.Kind.IsModifying(out var modifying) ? modifying : default);

    /// <summary>
    /// Gets a <see cref="NoteSpelling"/> equivalent to this one with the letter shifted by the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <seealso cref="NoteLetter.ShiftedBy(int)"/>
    public NoteSpelling LetterShiftedBy(int amount) => this with { Letter = Letter.ShiftedBy(amount) };
    #endregion

    #region Arithmetic
    /// <summary>
    /// Gets a <see cref="NoteSpelling"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleInterval"/> subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteSpelling operator -(NoteSpelling lhs, SimpleInterval rhs) => lhs + rhs.Inversion;

    /// <summary>
    /// Gets a <see cref="NoteSpelling"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleInterval"/> added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteSpelling operator +(NoteSpelling lhs, SimpleInterval rhs)
    {
        var newLetter = lhs.Letter.Plus(rhs.Number, out var differenceQuality);
        return new(
            newLetter,
            lhs.Accidental.ShiftedBy(
                rhs.Quality.PerfectOrMajorBasedIndex(rhs.Perfectability)
                    - differenceQuality.PerfectOrMajorBasedIndex(rhs.Perfectability)));
    }

    /// <summary>
    /// Gets the difference between the two <see cref="NoteSpelling"/> instances passed in as an instance
    /// of <see cref="SimpleInterval"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleInterval operator -(NoteSpelling lhs, NoteSpelling rhs)
        => lhs.Letter - rhs.Letter + (lhs.Accidental - rhs.Accidental);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NoteSpelling other) => Letter == other.Letter && Accidental == other.Accidental;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Letter, Accidental);

    /// <summary>
    /// Determines if this <see cref="NoteSpelling"/> is enharmonically equivalent to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnharmonicallyEquivalentTo(NoteSpelling other) => PitchClass == other.PitchClass;
    #endregion

    #region Conversion
    /// <summary>
    /// Gets the <see cref="Note"/> with the current <see cref="NoteSpelling"/> in the specified numbered octave.
    /// </summary>
    /// <param name="Octave"></param>
    /// <returns></returns>
    public Note WithOctave(int Octave) => new(this, Octave);

    /// <summary>
    /// Implicitly converts a <see cref="NoteLetter"/> to an instance of this type.
    /// </summary>
    /// <param name="letter"></param>
    public static implicit operator NoteSpelling(NoteLetter letter) => new(letter);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(NoteSpelling)} {{ Letter = {Letter}, Accidental = {Accidental} }}";

    /// <summary>
    /// Gets a musical notation string that represents this instance using ASCII characters.
    /// </summary>
    /// <returns></returns>
    /// <seealso cref="Accidental.ToASCIIMusicalNotationString"/>
    public string ToASCIIMusicalNotationString() => ToMusicalNotationString(Accidental.ToASCIIMusicalNotationString());

    /// <summary>
    /// Gets a musical notation string that represents this instance using unicode (UTF-16) characters.
    /// </summary>
    /// <param name="showNatural">Whether or not to show the natural if this instance is natural.</param>
    /// <returns></returns>
    /// <seealso cref="Accidental.ToUnicodeMusicalNotationString(bool)"/>
    public string ToUnicodeMusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(Accidental.ToUnicodeMusicalNotationString(showNatural));

    /// <summary>
    /// Gets a musical notation string that represents this instance using uTF32 (UTF-16) characters.
    /// </summary>
    /// <param name="showNatural">Whether or not to show the natural if this instance is natural.</param>
    /// <returns></returns>
    /// <seealso cref="Accidental.ToUTF32MusicalNotationString(bool)"/>
    public string ToUTF32MusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(Accidental.ToUTF32MusicalNotationString(showNatural));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string ToMusicalNotationString(string accidentalString) => $"{Letter}{accidentalString}";
    #endregion
    #endregion
}
