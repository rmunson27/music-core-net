using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a musical note.
/// </summary>
/// <remarks>
/// The default value of this struct is a 'C' note in the zero octave.
/// </remarks>
/// <param name="Spelling">The spelling of the note (letter and accidental).</param>
/// <param name="Octave">The octave of the note.</param>
public readonly record struct Note(NoteSpelling Spelling, int Octave)
{
    #region Properties
    /// <summary>
    /// Gets the letter of this note.
    /// </summary>
    public NoteLetter Letter => Spelling.Letter;

    /// <summary>
    /// Gets the accidental of this note.
    /// </summary>
    public Accidental Accidental => Spelling.Accidental;

    /// <summary>
    /// Gets the pitch this note represents.
    /// </summary>
    public NotePitch Pitch
    {
        get
        {
            var octaveFixup
                = Maths.FloorDivRem(Spelling.HalfStepsAboveC, NotePitchClass.ValuesCount,
                                    out var cRelativeSpellingValue);
            return new(NotePitchClass.FromSemitonesAboveC(cRelativeSpellingValue), Octave + octaveFixup);
        }
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Gets the <see cref="Note"/> with the pitch passed in that has the simplest possible accidental
    /// (i.e. closest to natural), using the specified <see cref="NonNaturalAccidentalType"/> to assign accidentals
    /// if necessary.
    /// </summary>
    /// <param name="Pitch">The pitch the result represents.</param>
    /// <param name="AccidentalKind">
    /// The kind of accidental to assign in ambiguous cases.
    /// <para/>
    /// For example, if <see cref="NotePitchClass.GA"/> is passed in, the result will be G# if
    /// <paramref name="AccidentalKind"/> is set to <see cref="ModifyingAccidentalKind.Sharp"/> and Ab if
    /// it is set to <see cref="ModifyingAccidentalKind.Flat"/>.
    /// </param>
    /// <returns></returns>
    public static Note SimplestWithPitch(NotePitch Pitch, ModifyingAccidentalKind AccidentalKind)
        => new(NoteSpelling.SimplestWithPitchClass(Pitch.Class, AccidentalKind), Pitch.Octave);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Note other) => Octave == other.Octave && Accidental == other.Accidental;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Spelling, Octave);

    /// <summary>
    /// Determines if this <see cref="Note"/> is enharmonically equivalent to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnharmonicallyEquivalentTo(Note other) => Pitch == other.Pitch;
    #endregion

    #region Classification
    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is sharp, setting the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsSharp([NonNegative] out int Degree) => Accidental.IsSharp(out Degree);

    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is sharp.
    /// </summary>
    /// <returns></returns>
    public bool IsSharp() => Accidental.IsSharp();

    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is natural, setting the value in an <see langword="out"/>
    /// parameter if so.
    /// </summary>
    /// <param name="naturalNote"></param>
    /// <returns></returns>
    public bool IsNatural(out NaturalNote naturalNote)
        => IsNatural() ? Try.Success(out naturalNote, new(Letter, Octave)) : Try.Failure(out naturalNote);

    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is natural.
    /// </summary>
    /// <returns></returns>
    public bool IsNatural() => Accidental.IsNatural();

    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is flat, setting the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsFlat([NonNegative] out int Degree) => Accidental.IsFlat(out Degree);

    /// <summary>
    /// Determines whether or not this <see cref="Note"/> is flat.
    /// </summary>
    /// <returns></returns>
    public bool IsFlat() => Accidental.IsFlat();
    #endregion

    #region Arithmetic
    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to the note passed in with the <see cref="SignedInterval"/> passed
    /// in subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Note operator -(Note lhs, SignedInterval rhs)
        => rhs.Sign < 0 ? lhs + rhs._interval : lhs - rhs._interval;

    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to the note passed in with the <see cref="SignedInterval"/> passed
    /// in added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Note operator +(Note lhs, SignedInterval rhs)
        => rhs.Sign < 0 ? lhs - rhs._interval : lhs + rhs._interval;

    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to the note passed in with the <see cref="Interval"/> passed in subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Note operator -(Note lhs, in Interval rhs)
    {
        var newSpelling = lhs.Spelling - rhs.Base;

        // Spelling addition underflows if we have hit the next octave down
        // This would be less than 0, but subtract 1 since the number contributes 1 less than its value to the letter
        // when subtracting
        var spellingAdditionUnderflows = lhs.Letter.CBasedIndex - rhs.Base.Number < -1;

        var newOctave = lhs.Octave - rhs.AdditionalOctaves;
        if (spellingAdditionUnderflows) newOctave--;

        return new(newSpelling, newOctave);
    }

    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to the note passed in with the <see cref="Interval"/> passed in added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Note operator +(Note lhs, in Interval rhs)
    {
        var newSpelling = lhs.Spelling + rhs.Base;

        // Spelling addition overflows if we have hit the next octave up
        // This would be greater than or equal to the values count, but add 1 since the number contributes 1 less
        // than its value to the letter when adding
        var spellingAdditionOverflows = lhs.Letter.CBasedIndex + rhs.Base.Number > NoteLetter.ValuesCount;

        var newOctave = lhs.Octave + rhs.AdditionalOctaves;
        if (spellingAdditionOverflows) newOctave++;

        return new(newSpelling, newOctave);
    }

    /// <summary>
    /// Finds the difference between the <see cref="Note"/> values passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SignedInterval operator -(Note lhs, Note rhs)
    {
        var baseDiff = lhs.Spelling - rhs.Spelling;
        var diffAdditionalOctaves = lhs.Octave - rhs.Octave;

        // Will need to store whether or not the result is negative (as the additional octave difference could be 0
        // with a negative result)
        var isNegative = false;

        // If the left octave is greater than the right but the right letter is higher in its octave than the left
        // is in its octave, then the difference between them spans 1 less octave than indicated by the initially
        // computed difference
        if (diffAdditionalOctaves > 0 && lhs.Letter.CBasedIndex < rhs.Letter.CBasedIndex) diffAdditionalOctaves--;

        // If the octaves are the same but the right letter is higher in the octave than the left, then the right
        // note is higher than the left
        // Since NoteSpelling subtraction treats the left side as higher, the initially computed base must be inverted,
        // and the result will be negative
        else if (diffAdditionalOctaves == 0 && lhs.Letter.CBasedIndex < rhs.Letter.CBasedIndex)
        {
            baseDiff = -baseDiff;
            isNegative = true;
        }

        else if (diffAdditionalOctaves < 0)
        {
            // Since NoteSpelling subtraction treats the left side as higher, the initially computed base must be inverted 
            baseDiff = -baseDiff;

            // Since the inversion above allows the left side to be treated as lower, if the letter of the left side
            // is higher, the difference between the notes spans 1 less octave than indicated by the initially
            // computed difference
            if (lhs.Letter.CBasedIndex > rhs.Letter.CBasedIndex) diffAdditionalOctaves++;

            // Make the octave difference positive, and indicate a negative result (for construction of the result)
            diffAdditionalOctaves = -diffAdditionalOctaves;
            isNegative = true;
        }

        return isNegative
                ? SignedInterval.Negative(new(baseDiff, diffAdditionalOctaves))
                : SignedInterval.Positive(new(baseDiff, diffAdditionalOctaves));
    }
    #endregion

    #region Computation
    /// <summary>
    /// Gets a <see cref="Note"/> enharmonically equivalent to the current instance with the accidental simplified
    /// as much as possible (i.e. calling the method on a Cb note will yield a B note).
    /// </summary>
    /// <remarks>
    /// The accidental type of the current instance will be used to resolve ambiguity if necessary.
    /// For example, calling this method on a G#x note will yield an A# note, whereas calling it on a Cbb note will
    /// yield a Bb note.
    /// </remarks>
    /// <returns></returns>
    public Note SimplifyAccidental()
    {
        var simplifiedSpelling = Spelling.SimplifyAccidental();
        var accidentalDiff = simplifiedSpelling.Accidental.Modification - Accidental.Modification;
        var octave = Octave;

        if (accidentalDiff < 0) // Accidental is lower, so letter is higher
        {
            var octaveRelativeHalfSteps = Letter.HalfStepsBelowC + accidentalDiff;
            octave -= Maths.FloorDiv(octaveRelativeHalfSteps, NotePitchClass.ValuesCount);

            // If we went up to C (rather than already being at C) add an octave since we have moved up to the
            // next octave
            if (octaveRelativeHalfSteps % NotePitchClass.ValuesCount == 0 && Letter != NoteLetter.C) octave++;
        }
        else if (accidentalDiff > 0) // Accidental is higher, so letter is lower
        {
            var octaveRelativeHalfSteps = Letter.HalfStepsAboveC - accidentalDiff;
            octave += Maths.FloorDiv(octaveRelativeHalfSteps, NotePitchClass.ValuesCount);
        }

        return new(simplifiedSpelling, octave);
    }

    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to this one with the letter shifted by the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Note LetterShiftedBy(int amount)
    {
        var newLetter = Letter.ShiftedBy(amount, out var octaveDifference);
        return new(Spelling with { Letter = newLetter }, Octave + octaveDifference);
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Implicitly converts a <see cref="NaturalNote"/> to a <see cref="Note"/>.
    /// </summary>
    /// <param name="naturalNote"></param>
    public static implicit operator Note(NaturalNote naturalNote) => new(naturalNote.Letter, naturalNote.Octave);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToString(Letter, Accidental, Octave);

    /// <summary>
    /// Gets a string that represents the note with the specified components.
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="accidental"></param>
    /// <param name="octave"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ToString(NoteLetter letter, Accidental accidental, int octave)
        => $"{nameof(Note)} {{ Letter = {letter}, Accidental = {accidental}, Octave = {octave} }}";

    /// <summary>
    /// Gets a musical notation string that represents this instance using ASCII characters.
    /// </summary>
    /// <returns></returns>
    public string ToASCIIMusicalNotationString() => ToMusicalNotationString(Spelling.ToASCIIMusicalNotationString());

    /// <summary>
    /// Gets a musical notation string that represents this instance using unicode (UTF-16) characters.
    /// </summary>
    /// <param name="showNatural">Whether or not to show the natural if this instance is natural.</param>
    /// <returns></returns>
    /// <seealso cref="NoteSpelling.ToUnicodeMusicalNotationString(bool)"/>
    public string ToUnicodeMusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(Spelling.ToUnicodeMusicalNotationString(showNatural));

    /// <summary>
    /// Gets a musical notation string that represents this instance using UTF32 characters.
    /// </summary>
    /// <param name="showNatural">Whether or not to show the natural if this instance is natural.</param>
    /// <returns></returns>
    /// <seealso cref="NoteSpelling.ToUTF32MusicalNotationString(bool)"/>
    public string ToUTF32MusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(Spelling.ToUTF32MusicalNotationString(showNatural));

    /// <summary>
    /// Gets a musical notation string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string ToMusicalNotationString(string noteSpellingStr)
        => $"{noteSpellingStr}{(Octave < 0 ? $"({Octave})" : Octave)}";
    #endregion

    #region Letters
    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing an 'A' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter A => NoteLetter.A;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing a 'B' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter B => NoteLetter.B;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing a 'C' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter C => NoteLetter.C;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing a 'D' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter D => NoteLetter.D;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing an 'E' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter E => NoteLetter.E;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing an 'F' note.
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter F => NoteLetter.F;

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> representing a 'G' note.
    /// </summary>
    /// <returns></returns>
    public static NoteLetter G => NoteLetter.G;
    #endregion
    #endregion
}
