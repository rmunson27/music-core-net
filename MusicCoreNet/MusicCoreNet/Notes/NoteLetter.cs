using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using static NoteLetter;

/// <summary>
/// Static functionality for the <see cref="NoteLetter"/> enum.
/// </summary>
public static class NoteLetters
{
    /// <summary>
    /// Gets the <see cref="NotePitchClass"/> equivalent to the current <see cref="NoteLetter"/> instance.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    internal static NotePitchClass GetPitchClass(this NoteLetter letter) => letter switch
    {
        A => NotePitchClass.A,
        B => NotePitchClass.B,
        C => NotePitchClass.C,
        D => NotePitchClass.D,
        E => NotePitchClass.E,
        F => NotePitchClass.F,
        G => NotePitchClass.G,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalNumber"/> subtracted.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static NoteLetter Minus([NamedEnum] this NoteLetter lhs, SimpleIntervalNumber rhs)
        => lhs.Plus(rhs.Inversion());

    /// <summary>
    /// Gets a <see cref="NoteLetter"/> equivalent to the current instance with the supplied
    /// <see cref="SimpleIntervalNumber"/> added.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static NoteLetter Plus([NamedEnum] this NoteLetter lhs, SimpleIntervalNumber rhs)
    {
        Throw.IfEnumArgUnnamed(lhs, nameof(lhs));
        return (NoteLetter)(((int)lhs + rhs.Value - 1) % 7);
    }

    /// <summary>
    /// Finds the difference between the current <see cref="NoteLetter"/> instance and
    /// another <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// Either the current instance or <paramref name="other"/> was an unnamed enum value.
    /// </exception>
    public static SimpleIntervalBase Minus([NamedEnum] this NoteLetter letter, [NamedEnum] NoteLetter other)
    {
        Throw.IfEnumArgUnnamed(letter, nameof(letter));
        Throw.IfEnumArgUnnamed(other, nameof(other));

        var halfSteps = (letter.ARelativeHalfSteps() - other.ARelativeHalfSteps() + 12) % 12;
        return SimpleIntervalBase.SimplestQualityWithHalfSteps(halfSteps) switch
        {
            null => letter switch
            {
                B => Intervals.Augmented().Fourth(),
                F => Intervals.Diminished().Fifth(),
                _ => throw new Exception("Bug - should never happen."),
            },
            SimpleIntervalBase sib => sib,
        };
    }

    /// <summary>
    /// Gets the number of half steps from the C note below or equal to the natural note represented by the current
    /// <see cref="NoteLetter"/> instance to the current instance.
    /// </summary>
    /// <remarks>
    /// This is important for computing octave information.
    /// </remarks>
    /// <param name="letter"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    internal static int CRelativeHalfSteps([NamedEnum] this NoteLetter letter)
        => letter.GetPitchClass().CRelativeIndex();

    /// <summary>
    /// Gets the number of half steps from the A note below or equal to the natural note represented by the current
    /// <see cref="NoteLetter"/> instance to the current instance.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    internal static int ARelativeHalfSteps([NamedEnum] this NoteLetter letter) => letter switch
    {
        A => 0,
        B => 2,
        C => 3,
        D => 5,
        E => 7,
        F => 8,
        G => 10,
        _ => throw Undefined,
    };

    /// <summary>
    /// Gets an index representing the position of the current instance relative to <see cref="C"/>.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    [return: NonNegative]
    public static int CBasedIndex(this NoteLetter letter) => letter switch
    {
        C => 0,
        D => 1,
        E => 2,
        F => 3,
        G => 4,
        A => 5,
        B => 6,
        _ => throw Undefined,
    };

    private static InvalidEnumArgumentException Undefined => new($"Undefined {nameof(NoteLetter)} value.");
}

/// <summary>
/// Represents the classification of a musical note using a letter of the alphabet, from A to G.
/// </summary>
public enum NoteLetter : byte
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

