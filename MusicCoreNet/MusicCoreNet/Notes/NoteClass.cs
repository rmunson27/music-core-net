using Rem.Core.Attributes;
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

    /// <summary>
    /// Gets the pitch class of this instance.
    /// </summary>
    public NotePitchClass PitchClass
        => (NotePitchClass)Maths.FloorRem(Letter.HalfStepsDownToA() + Accidental.IntValue, 12);
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Gets the <see cref="NoteClass"/> with the pitch class passed in that has the simplest possible accidental
    /// (i.e. closest to natural), using the specified <see cref="NonNaturalAccidentalType"/> to assign accidentals
    /// if necessary.
    /// </summary>
    /// <param name="pitchClass">The pitch class of the result.</param>
    /// <param name="nonNaturalAccidentalType">
    /// An accidental type to use to assign accidentals in ambiguous cases.
    /// <para/>
    /// For example, if <see cref="NotePitchClass.GA"/> is passed in, the result will be G# if
    /// <paramref name="nonNaturalAccidentalType"/> is set to <see cref="NonNaturalAccidentalType.Sharp"/> and Ab if
    /// it is set to <see cref="NonNaturalAccidentalType.Flat"/>.
    /// </param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// Either <paramref name="pitchClass"/> or <paramref name="nonNaturalAccidentalType"/> was an unnamed enum value.
    /// </exception>
    public static NoteClass SimplestWithPitchClass(
        [NamedEnum] NotePitchClass pitchClass, [NamedEnum] NonNaturalAccidentalType nonNaturalAccidentalType)
    {
        Throw.IfEnumArgUnnamed(pitchClass, nameof(pitchClass));

        var sharpResult = Throw.IfEnumArgUnnamed(nonNaturalAccidentalType, nameof(nonNaturalAccidentalType))
                            == NonNaturalAccidentalType.Sharp;

        return pitchClass switch
        {
            NotePitchClass.A => NoteLetter.A,
            NotePitchClass.AB => sharpResult ? Notes.A().Sharp() : Notes.B().Flat(),
            NotePitchClass.B => NoteLetter.B,
            NotePitchClass.C => NoteLetter.C,
            NotePitchClass.CD => sharpResult ? Notes.C().Sharp() : Notes.D().Flat(),
            NotePitchClass.D => NoteLetter.D,
            NotePitchClass.DE => sharpResult ? Notes.D().Sharp() : Notes.E().Flat(),
            NotePitchClass.E => NoteLetter.E,
            NotePitchClass.F => NoteLetter.F,
            NotePitchClass.FG => sharpResult ? Notes.F().Sharp() : Notes.G().Flat(),
            NotePitchClass.G => NoteLetter.G,
            _ => sharpResult ? Notes.G().Sharp() : Notes.A().Flat(),
        };
    }
    #endregion

    #region Computation
    /// <summary>
    /// Gets a <see cref="NoteClass"/> enharmonically equivalent to the current instance with the accidental simplified
    /// as much as possible (i.e. calling the method on Cb will yield B).
    /// </summary>
    /// <remarks>
    /// The accidental type of the current instance will be used to resolve ambiguity if necessary.
    /// For example, calling this method on G#x will yield A#, whereas calling it on Cbb will yield Bb.
    /// </remarks>
    /// <returns></returns>
    public NoteClass SimplifyAccidental()
        => SimplestWithPitchClass(
            PitchClass,

            // Default this parameter if the accidental is natural - it will not be used
            Accidental.IsNatural() ? NonNaturalAccidentalType.Flat : (NonNaturalAccidentalType)Accidental.Type);
    #endregion

    #region Arithmetic
    /// <summary>
    /// Gets a <see cref="NoteClass"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalBase"/> subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteClass operator -(NoteClass lhs, SimpleIntervalBase rhs) => lhs + rhs.Inversion();

    /// <summary>
    /// Gets a <see cref="NoteClass"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalBase"/> added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteClass operator +(NoteClass lhs, SimpleIntervalBase rhs)
    {
        var newLetter = lhs._letter.Plus(rhs.Number, out var differenceQuality);
        return new(
            newLetter,
            lhs.Accidental.Shift(rhs.Quality.PerfectOrMajorBasedIndex - differenceQuality.PerfectOrMajorBasedIndex));
    }

    /// <summary>
    /// Gets the difference between the two <see cref="NoteClass"/> instances passed in as an instance
    /// of <see cref="SimpleIntervalBase"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleIntervalBase operator -(NoteClass lhs, NoteClass rhs)
    {
        var letterDifference = lhs.Letter.Minus(rhs.Letter);
        return new(
                letterDifference.Quality.Shift(lhs.Accidental.IntValue - rhs.Accidental.IntValue),
                letterDifference.Number);
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

    /// <summary>
    /// Determines if this <see cref="NoteClass"/> is enharmonically equivalent to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEnharmonicallyEquivalentTo(NoteClass other) => PitchClass == other.PitchClass;
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
