using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents a musical note.
/// </summary>
/// <param name="Class">The class of the note (letter and accidental).</param>
/// <param name="Octave">The octave of the note.</param>
public readonly record struct Note(NoteClass Class, int Octave)
{
    #region Properties
    /// <summary>
    /// Gets the letter of this note.
    /// </summary>
    public NoteLetter Letter => Class.Letter;

    /// <summary>
    /// Gets the accidental of this note.
    /// </summary>
    public Accidental Accidental => Class.Accidental;

    /// <summary>
    /// Gets info for the pitch this note represents.
    /// </summary>
    public NotePitchInfo Pitch
    {
        get
        {
            var octaveFixup = Maths.FloorDivRem(
                                Class.Letter.CRelativeHalfSteps() + Class.Accidental.IntValue, 12,
                                out var cRelativeClassValue);
            return new(NotePitchClasses.FromCRelativeIndex(cRelativeClassValue), Octave + octaveFixup);
        }
    }
    #endregion

    #region Methods
    #region Equality
    /// <summary>
    /// Determines if the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Note other) => Octave == other.Octave && Class == other.Class;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Class, Octave);

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
        var newClass = lhs.Class - rhs.Base;

        // Class addition underflows if we have hit the next octave down
        // This would be less than 0, but subtract 1 since the number contributes 1 less than its value to the letter
        // when subtracting
        var classAdditionUnderflows = lhs.Letter.CBasedIndex() - rhs.Base.Number < -1;

        var newOctave = lhs.Octave - rhs.AdditionalOctaves;
        if (classAdditionUnderflows) newOctave--;

        return new(newClass, newOctave);
    }

    /// <summary>
    /// Gets a <see cref="Note"/> equivalent to the note passed in with the <see cref="Interval"/> passed in added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Note operator +(Note lhs, in Interval rhs)
    {
        var newClass = lhs.Class + rhs.Base;

        // Class addition overflows if we have hit the next octave up
        // This would be greater than 6, but add 1 since the number contributes 1 less than its value to the letter
        // when adding
        var classAdditionOverflows = lhs.Letter.CBasedIndex() + rhs.Base.Number > 7;

        var newOctave = lhs.Octave + rhs.AdditionalOctaves;
        if (classAdditionOverflows) newOctave++;

        return new(newClass, newOctave);
    }

    /// <summary>
    /// Finds the difference between the <see cref="Note"/> values passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SignedInterval operator -(Note lhs, Note rhs)
    {
        var baseDiff = lhs.Class - rhs.Class;
        var diffAdditionalOctaves = lhs.Octave - rhs.Octave;

        // Will need to store whether or not the result is negative (as the additional octave difference could be 0
        // with a negative result)
        var isNegative = false;

        // If the left octave is greater than the right but the right letter is higher in its octave than the left
        // is in its octave, then the difference between them spans 1 less octave than indicated by the initially
        // computed difference
        if (diffAdditionalOctaves > 0 && lhs.Letter.CBasedIndex() < rhs.Letter.CBasedIndex()) diffAdditionalOctaves--;

        // If the octaves are the same but the right letter is higher in the octave than the left, then the right
        // note is higher than the left
        // Since NoteClass subtraction treats the left side as higher, the initially computed base must be inverted,
        // and the result will be negative
        else if (diffAdditionalOctaves == 0 && lhs.Letter.CBasedIndex() < rhs.Letter.CBasedIndex())
        {
            baseDiff = -baseDiff;
            isNegative = true;
        }

        else if (diffAdditionalOctaves < 0)
        {
            // Since NoteClass subtraction treats the left side as higher, the initially computed base must be inverted 
            baseDiff = -baseDiff;

            // Since the inversion above allows the left side to be treated as lower, if the letter of the left side
            // is higher, the difference between the notes spans 1 less octave than indicated by the initially
            // computed difference
            if (lhs.Letter.CBasedIndex() > rhs.Letter.CBasedIndex()) diffAdditionalOctaves++;

            // Make the octave difference positive, and indicate a negative result (for construction of the result)
            diffAdditionalOctaves = -diffAdditionalOctaves;
            isNegative = true;
        }

        return isNegative
                ? SignedInterval.Negative(new(baseDiff, diffAdditionalOctaves))
                : SignedInterval.Positive(new(baseDiff, diffAdditionalOctaves));
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
        => $"{Class.ToMusicalNotationString()}{(Octave < 0 ? $"({Octave})" : Octave)}";
    #endregion
    #endregion
}
