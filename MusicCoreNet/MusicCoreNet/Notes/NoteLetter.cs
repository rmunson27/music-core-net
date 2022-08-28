using Rem.Core.Attributes;
using Rem.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the letter of a note in a note spelling.
/// </summary>
/// <remarks>
/// All possible values of this struct are present as <see langword="static"/> <see langword="readonly"/> fields
/// on the struct.
/// <para/>
/// The default value of the struct represents an 'A' note letter.
/// </remarks>
public readonly record struct NoteLetter
{
    #region Constants
    /// <summary>
    /// Represents an 'A' note.
    /// </summary>
    public static readonly NoteLetter A = Values.A;

    /// <summary>
    /// Represents a 'B' note.
    /// </summary>
    public static readonly NoteLetter B = Values.B;

    /// <summary>
    /// Represents a 'C' note.
    /// </summary>
    public static readonly NoteLetter C = Values.C;

    /// <summary>
    /// Represents a 'D' note.
    /// </summary>
    public static readonly NoteLetter D = Values.D;

    /// <summary>
    /// Represents an 'E' note.
    /// </summary>
    public static readonly NoteLetter E = Values.E;

    /// <summary>
    /// Represents an 'F' note.
    /// </summary>
    public static readonly NoteLetter F = Values.F;

    /// <summary>
    /// Represents a 'G' note.
    /// </summary>
    public static readonly NoteLetter G = Values.G;
    #endregion

    #region Properties
    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely identifying the current instance.
    /// </summary>
    [NamedEnum] public Values Value { get; }

    /// <summary>
    /// Gets an index representing the position of the current instance in a C-based octave relative to the C-note
    /// in the octave.
    /// </summary>
    [NonNegative] public int CBasedIndex => Value switch
    {
        Values.C => 0,
        Values.D => 1,
        Values.E => 2,
        Values.F => 3,
        Values.G => 4,
        Values.A => 5,
        _ => 6, // Values.B
    };

    /// <summary>
    /// Gets an index representing the position of a natural note spelling described by the current instance relative
    /// to the C natural note spelling.
    /// </summary>
    internal int CircleOfFifthsIndexRelativeToC => Value switch
    {
        Values.C => 0,
        Values.D => 2,
        Values.E => 4,
        Values.F => -1,
        Values.G => 1,
        Values.A => 3,
        _ => 5,
    };

    /// <summary>
    /// Gets the number of half steps from a natural note spelling described by this letter down to the nearest C note
    /// below or equal to it.
    /// </summary>
    public int HalfStepsDownToC => PitchClass.SemitonesDownToC();

    /// <summary>
    /// Gets the number of half steps from a natural note spelling described by this letter up to the nearest C note
    /// above or equal to it.
    /// </summary>
    public int HalfStepsUpToC => PitchClass.SemitonesUpToC();

    /// <summary>
    /// Gets the number of half steps from a natural note spelling described by this letter down to the nearest A note
    /// below or equal to it.
    /// </summary>
    internal int HalfStepsDownToA => (int)PitchClass;

    /// <summary>
    /// Gets the pitch class associated with the natural note spelling described by this letter.
    /// </summary>
    public NotePitchClass PitchClass => Value switch
    {
        Values.A => NotePitchClass.A,
        Values.B => NotePitchClass.B,
        Values.C => NotePitchClass.C,
        Values.D => NotePitchClass.D,
        Values.E => NotePitchClass.E,
        Values.F => NotePitchClass.F,
        _ => NotePitchClass.G, // Values.G
    };
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="Value"></param>
    private NoteLetter(Values Value) { this.Value = Value; }
    #endregion

    #region Methods
    #region Equality
    /// <summary>
    /// Determines whether the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NoteLetter other) => Value == other.Value;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value.GetHashCode();
    #endregion

    #region Conversion
    /// <summary>
    /// Explicitly converts a <see cref="NoteLetter"/> to a <see cref="byte"/> (this cast cannot fail).
    /// </summary>
    /// <param name="letter"></param>
    public static explicit operator byte(NoteLetter letter) => (byte)letter.Value;

    /// <summary>
    /// Explicitly converts a <see cref="byte"/> to a <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="letter"></param>
    /// <exception cref="InvalidCastException">
    /// The <see cref="byte"/> did not represent a <see cref="NoteLetter"/> value.
    /// </exception>
    public static explicit operator NoteLetter(byte b)
        => Enums.IsDefined((Values)b)
            ? (Values)b
            : throw new InvalidCastException($"Argument did not represent a {nameof(NoteLetter)}.");

    /// <summary>
    /// Implicitly converts a named <see cref="NoteLetter.Values"/> instance to a <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="Value"></param>
    /// <exception cref="InvalidCastException"><paramref name="Value"/> was an unnamed enum value.</exception>
    public static implicit operator NoteLetter([NamedEnum] Values Value)
        => new(
            Enums.IsDefined(Value)
                ? Value
                : throw new InvalidCastException($"Argument was an unnamed enum value."));
    #endregion

    #region Arithmetic
    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the value passed in with the <see cref="SimpleIntervalNumber"/>
    /// passed in subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteLetter operator -(NoteLetter lhs, SimpleIntervalNumber rhs)
        => lhs + rhs.Inversion();

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the value passed in with the <see cref="SimpleIntervalNumber"/>
    /// passed in added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static NoteLetter operator +(NoteLetter lhs, SimpleIntervalNumber rhs)
        => (NoteLetter)(((int)lhs + rhs.Value - 1) % 7);

    /// <summary>
    /// Finds the difference between the two <see cref="NoteLetter"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleIntervalBase operator -(NoteLetter lhs, NoteLetter rhs)
    {
        var halfSteps = (lhs.HalfStepsDownToA - rhs.HalfStepsDownToA + 12) % 12;
        return SimpleIntervalBase.SimplestWithHalfSteps(halfSteps) switch
        {
            null => lhs == B ? Interval.Augmented().Fourth() : Interval.Diminished().Fifth(),
            SimpleIntervalBase sib => sib,
        };
    }

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalNumber"/> subtracted, returning the quality of the difference between the current
    /// instance and the result in an <see langword="out"/> parameter.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="number"></param>
    /// <param name="differenceQuality"></param>
    /// <returns></returns>
    internal NoteLetter Minus(SimpleIntervalNumber number, out IntervalQuality differenceQuality)
    {
        var sum = Plus(number.Inversion(), out differenceQuality);
        differenceQuality = differenceQuality.Inversion(); // Invert since we are going down instead of up
        return sum;
    }

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalNumber"/> added, returning the quality of the difference between the current
    /// instance and the result in an <see langword="out"/> parameter.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="number"></param>
    /// <param name="differenceQuality"></param>
    /// <returns></returns>
    internal NoteLetter Plus(SimpleIntervalNumber number, out IntervalQuality differenceQuality)
    {
        var newLetter = this + number;

        var halfSteps = newLetter.HalfStepsDownToA - HalfStepsDownToA + 12;
        differenceQuality = IntervalQuality.OfSimplestIntervalWithHalfSteps(halfSteps)
                                ?? (this == F
                                        ? PerfectableIntervalQuality.Augmented()
                                        : PerfectableIntervalQuality.Diminished());

        return newLetter;
    }
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
    /// Represents the potential values of this struct.
    /// </summary>
    public enum Values
    {
        /// <summary>
        /// Represents an 'A' note.
        /// </summary>
        A,

        /// <summary>
        /// Represents a 'B' note.
        /// </summary>
        B,

        /// <summary>
        /// Represents a 'C' note.
        /// </summary>
        C,

        /// <summary>
        /// Represents a 'D' note.
        /// </summary>
        D,

        /// <summary>
        /// Represents an 'E' note.
        /// </summary>
        E,

        /// <summary>
        /// Represents an 'F' note.
        /// </summary>
        F,

        /// <summary>
        /// Represents a 'G' note.
        /// </summary>
        G,
    }
    #endregion
}

