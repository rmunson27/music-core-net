using Rem.Core.Attributes;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var octaveFixup = Maths.FloorDivRem(Spelling.HalfStepsAboveC, 12, out var cRelativeSpellingValue);
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
    /// <param name="AccidentalType">
    /// An accidental type to use to assign accidentals in ambiguous cases.
    /// <para/>
    /// For example, if <see cref="NotePitchClass.GA"/> is passed in, the result will be G# if
    /// <paramref name="AccidentalType"/> is set to <see cref="NonNaturalAccidentalType.Sharp"/> and Ab if
    /// it is set to <see cref="NonNaturalAccidentalType.Flat"/>.
    /// </param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="AccidentalType"/> was an unnamed enum value.
    /// </exception>
    public static Note SimplestWithPitch(NotePitch Pitch, [NameableEnum] NonNaturalAccidentalType AccidentalType)
        => new(NoteSpelling.SimplestWithPitchClass(Pitch.Class, AccidentalType), Pitch.Octave);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Note other) => Octave == other.Octave && Spelling == other.Spelling;

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
        // This would be greater than 6, but add 1 since the number contributes 1 less than its value to the letter
        // when adding
        var spellingAdditionOverflows = lhs.Letter.CBasedIndex + rhs.Base.Number > 7;

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
        var accidentalDiff = simplifiedSpelling.Accidental.IntValue - Accidental.IntValue;
        var octave = Octave;

        if (accidentalDiff < 0) // Accidental is lower, so letter is higher
        {
            var octaveRelativeHalfSteps = Letter.HalfStepsBelowC + accidentalDiff;
            octave -= Maths.FloorDiv(octaveRelativeHalfSteps, 12);

            // If we went up to C (rather than already being at C) add an octave since we have moved up to the
            // next octave
            if (octaveRelativeHalfSteps % 12 == 0 && Letter != NoteLetter.C) octave++;
        }
        else if (accidentalDiff > 0) // Accidental is higher, so letter is lower
        {
            var octaveRelativeHalfSteps = Letter.HalfStepsAboveC - accidentalDiff;
            octave += Maths.FloorDiv(octaveRelativeHalfSteps, 12);
        }

        return new(simplifiedSpelling, octave);
    }
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => $"{nameof(Note)} {{ Letter = {Letter}, Accidental = {Accidental}, Octave = {Octave} }}";

    /// <summary>
    /// Gets a musical notation string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public string ToMusicalNotationString()
        => $"{Spelling.ToMusicalNotationString()}{(Octave < 0 ? $"({Octave})" : Octave)}";
    #endregion

    #region Builder
    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'A' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder A() => new(NoteLetter.A);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'B' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder B() => new(NoteLetter.B);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'C' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder C() => new(NoteLetter.C);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'D' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder D() => new(NoteLetter.D);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'E' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder E() => new(NoteLetter.E);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'F' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder F() => new(NoteLetter.F);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'G' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteBuilder G() => new(NoteLetter.G);
    #endregion
    #endregion
}
