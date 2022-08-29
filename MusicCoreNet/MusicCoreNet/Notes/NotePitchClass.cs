using Rem.Core.Attributes;
using Rem.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the pitch class of a note, up to octave difference.
/// </summary>
/// <remarks>
/// The default value of this struct represents a 'C' pitch class.
/// </remarks>
public readonly record struct NotePitchClass
{
    #region Constants
    /// <summary>
    /// Represents notes that are enharmonically equivalent to an 'A' note.
    /// </summary>
    public static readonly NotePitchClass A = new(Values.A);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to an 'A#' or 'Bb' note.
    /// </summary>
    public static readonly NotePitchClass AB = new(Values.AB);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'B' note.
    /// </summary>
    public static readonly NotePitchClass B = new(Values.B);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'C' note.
    /// </summary>
    public static readonly NotePitchClass C = new(Values.C);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'C#' or 'Db' note.
    /// </summary>
    public static readonly NotePitchClass CD = new(Values.CD);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'D' note.
    /// </summary>
    public static readonly NotePitchClass D = new(Values.D);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'D#' or 'Eb' note.
    /// </summary>
    public static readonly NotePitchClass DE = new(Values.DE);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to an 'E' note.
    /// </summary>
    public static readonly NotePitchClass E = new(Values.E);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to an 'F' note.
    /// </summary>
    public static readonly NotePitchClass F = new(Values.F);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'F#' or 'Gb' note.
    /// </summary>
    public static readonly NotePitchClass FG = new(Values.FG);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'G' note.
    /// </summary>
    public static readonly NotePitchClass G = new(Values.G);

    /// <summary>
    /// Represents notes that are enharmonically equivalent to a 'G#' or 'Ab' note.
    /// </summary>
    public static readonly NotePitchClass GA = new(Values.GA);
    #endregion

    #region Properties
    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely identifying the current instance.
    /// </summary>
    [NamedEnum] public Values Value { get; }

    /// <summary>
    /// Gets the number of semitones pitches described by this instance are above the nearest lesser or equal
    /// C pitch.
    /// </summary>
    public int SemitonesAboveC => (int)Value;

    /// <summary>
    /// Gets the number of semitones pitches described by this instance are below the nearest greater or equal
    /// C pitch.
    /// </summary>
    public int SemitonesBelowC =>
        // Subtract from 12 (so are going down instead of up) then mod by 12 (to put the C case in range at 0)
        (12 - (int)Value) % 12;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of this struct.
    /// </summary>
    /// <param name="Value"></param>
    private NotePitchClass([NamedEnum] Values Value) { this.Value = Value; }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="NotePitchClass"/> that represents pitches that are the given number of semitones
    /// above the nearest lesser or equal C pitch.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static NotePitchClass FromSemitonesAboveC([NonNegative, LessThanInteger(12)] int index)
    {
        Throw.IfArgNegative(index, nameof(index));
        Throw.IfArgGreaterThanOrEqualTo(12, index, nameof(index));
        return new((Values)index);
    }

    /// <summary>
    /// Creates a new <see cref="NotePitchClass"/> that represents pitches that are the given number of semitones
    /// below the nearest greater or equal C pitch.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static NotePitchClass FromSemitonesBelowC([NonNegative, LessThanInteger(12)] int index)
    {
        Throw.IfArgNegative(index, nameof(index));
        Throw.IfArgGreaterThanOrEqualTo(12, index, nameof(index));

        // Subtract from 12 (so are going up instead of down) then mod by 12 (to put the C case in range at 0)
        return new((Values)((12 - index) % 12));
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines whether the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NotePitchClass other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value.GetHashCode();
    #endregion

    #region Conversion
    /// <summary>
    /// Gets the <see cref="NotePitchInfo"/> equivalent to the current instance equipped with the supplied
    /// octave number.
    /// </summary>
    public NotePitchInfo WithOctave(int Octave) => new(this, Octave);

    /// <summary>
    /// Explicitly converts a <see cref="NotePitchClass"/> to a <see cref="byte"/> (this cast cannot fail).
    /// </summary>
    /// <param name="letter"></param>
    public static explicit operator byte(NotePitchClass letter) => (byte)letter.Value;

    /// <summary>
    /// Explicitly converts a <see cref="byte"/> to a <see cref="NotePitchClass"/>.
    /// </summary>
    /// <param name="letter"></param>
    /// <exception cref="InvalidCastException">
    /// The <see cref="byte"/> did not represent a <see cref="NotePitchClass"/> value.
    /// </exception>
    public static explicit operator NotePitchClass(byte b)
        => Enums.IsDefined((Values)b)
            ? new((Values)b)
            : throw new InvalidCastException($"Argument did not represent a {nameof(NotePitchClass)}.");

    /// <summary>
    /// Implicitly converts a named <see cref="Values"/> instance to a <see cref="NotePitchClass"/>.
    /// </summary>
    /// <param name="Value"></param>
    /// <exception cref="InvalidCastException"><paramref name="Value"/> was an unnamed enum value.</exception>
    public static implicit operator NotePitchClass([NamedEnum] Values Value)
        => new(
            Enums.IsDefined(Value)
                ? Value
                : throw new InvalidCastException($"Argument was an unnamed enum value."));
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// Represents the possible values of this struct.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'C' note.
        /// </summary>
        C,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'C#' or 'Db' note.
        /// </summary>
        CD,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'D' note.
        /// </summary>
        D,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'D#' or 'Eb' note.
        /// </summary>
        DE,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to an 'E' note.
        /// </summary>
        E,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to an 'F' note.
        /// </summary>
        F,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'F#' or 'Gb' note.
        /// </summary>
        FG,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'G' note.
        /// </summary>
        G,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'G#' or 'Ab' note.
        /// </summary>

        GA,
        /// <summary>
        /// Represents notes that are enharmonically equivalent to an 'A' note.
        /// </summary>
        A,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to an 'A#' or 'Bb' note.
        /// </summary>
        AB,

        /// <summary>
        /// Represents notes that are enharmonically equivalent to a 'B' note.
        /// </summary>
        B,
    }
    #endregion
}

