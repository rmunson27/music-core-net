using Rem.Core.Attributes;
using Rem.Core.Utilities;
using Rem.Music.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
/// The default value of the struct represents a 'C' note letter.
/// </remarks>
public readonly record struct NoteLetter
{
    #region Constants
    /// <summary>
    /// The number of distinct values of this type.
    /// </summary>
    public const int ValuesCount = 7;

    /// <summary>
    /// The maximum numerical value of instances of type <see cref="Values"/>.
    /// </summary>
    private const int MaxNumericalValue = ValuesCount - 1;

    /// <summary>
    /// Represents a 'C' note.
    /// </summary>
    public static readonly NoteLetter C = new(Values.C);

    /// <summary>
    /// Represents a 'D' note.
    /// </summary>
    public static readonly NoteLetter D = new(Values.D);

    /// <summary>
    /// Represents an 'E' note.
    /// </summary>
    public static readonly NoteLetter E = new(Values.E);

    /// <summary>
    /// Represents an 'F' note.
    /// </summary>
    public static readonly NoteLetter F = new(Values.F);

    /// <summary>
    /// Represents a 'G' note.
    /// </summary>
    public static readonly NoteLetter G = new(Values.G);

    /// <summary>
    /// Represents an 'A' note.
    /// </summary>
    public static readonly NoteLetter A = new(Values.A);

    /// <summary>
    /// Represents a 'B' note.
    /// </summary>
    public static readonly NoteLetter B = new(Values.B);
    #endregion

    #region Properties
    /// <summary>
    /// Gets an <see langword="enum"/> value uniquely identifying the current instance.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    /// <summary>
    /// Gets an index representing the position of the current instance in a C-based octave relative to the C-note
    /// in the octave.
    /// </summary>
    [NonNegative] public int CBasedIndex => (int)Value;

    /// <summary>
    /// Gets an index representing the position of a natural note spelling described by the current instance relative
    /// to the C natural note spelling.
    /// </summary>
    internal int CircleOfFifthsIndexRelativeToC
        => unchecked(Value <= Values.E ? (int)Value << 1 : ((int)Value << 1) - ValuesCount);

    /// <summary>
    /// Gets the number of half steps that the natural note spelling described by this letter is above the nearest
    /// lesser or equal 'C' note spelling.
    /// </summary>
    public int HalfStepsAboveC => PitchClass.SemitonesAboveC;

    /// <summary>
    /// Gets the number of half steps that the natural note spelling described by this letter is below the nearest
    /// greater or equal 'C' note spelling.
    /// </summary>
    public int HalfStepsBelowC => PitchClass.SemitonesBelowC;

    /// <summary>
    /// Gets the pitch class associated with the natural note spelling described by this letter.
    /// </summary>
    public NotePitchClass PitchClass
        => new(unchecked((NotePitchClass.Values)(Value <= Values.E ? (int)Value << 1 : ((int)Value << 1) - 1)));
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="Value"></param>
    internal NoteLetter([NameableEnum] Values Value) { this.Value = Value; }
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

    #region Arithmetic
    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to this one shifted by the specified amount.
    /// </summary>
    /// <remarks>
    /// If the supplied amount is positive, this will shift forward in the alphabet; for example,
    /// <c>A.ShiftedBy(1)</c> will yield <c>B</c>.
    /// </remarks>
    /// <param name="amount">The amount to shift this instance by.</param>
    /// <returns>A <see cref="NoteLetter"/> equivalent to this one shifted by <paramref name="amount"/>.</returns>
    public NoteLetter ShiftedBy(int amount)
    {
        var shiftedBase = (int)Value + amount;
        if (shiftedBase < 0 || shiftedBase > MaxNumericalValue) shiftedBase = Maths.FloorRem(shiftedBase, ValuesCount);
        return new((Values)shiftedBase);
    }

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to this one shifted by the specified amount, setting the octave
    /// overflow or underflow in an <see langword="out"/> parameter.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="octaveDifference"></param>
    /// <returns></returns>
    internal NoteLetter ShiftedBy(int amount, out int octaveDifference)
    {
        var shiftedBase = (int)Value + amount;
        octaveDifference = Maths.FloorDivRem(shiftedBase, ValuesCount, out shiftedBase);
        return new((Values)shiftedBase);
    }

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
        => new((Values)((lhs.CBasedIndex + rhs.NumericalValue - 1) % ValuesCount));

    /// <summary>
    /// Finds the difference between the two <see cref="NoteLetter"/> instances passed in.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleIntervalBase operator -(NoteLetter lhs, NoteLetter rhs)
    {
        var halfSteps = (lhs.HalfStepsAboveC - rhs.HalfStepsAboveC + NotePitchClass.ValuesCount)
                            % NotePitchClass.ValuesCount;
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

        var halfSteps = newLetter.HalfStepsAboveC - HalfStepsAboveC + NotePitchClass.ValuesCount;
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
    public enum Values : byte
    {
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

        /// <summary>
        /// Represents an 'A' note.
        /// </summary>
        A,

        /// <summary>
        /// Represents a 'B' note.
        /// </summary>
        B,
    }
    #endregion
}

