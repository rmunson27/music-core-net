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
    public static NotePitchClass ToPitchClass(this NoteLetter letter) => letter switch
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
    /// Finds the difference between the current <see cref="NoteLetter"/> instance and
    /// another <see cref="NoteLetter"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// Either the current instance or <paramref name="rhs"/> was an unnamed enum value.
    /// </exception>
    public static SimpleIntervalBase Minus(this NoteLetter lhs, NoteLetter rhs)
    {
        Throw.IfEnumArgUnnamed(lhs, nameof(lhs));
        Throw.IfEnumArgUnnamed(rhs, nameof(rhs));

        var halfSteps = (lhs.ARelativeHalfSteps() - rhs.ARelativeHalfSteps() + 12) % 12;
        return SimpleIntervalBase.SimplestQualityWithHalfSteps(halfSteps) switch
        {
            null => lhs switch
            {
                B => Intervals.Augmented().Fourth(),
                F => Intervals.Diminished().Fifth(),
                _ => throw new Exception("Bug - should never happen."),
            },
            var sib => sib,
        };
    }

    /// <summary>
    /// Gets the number of half steps from the A note below or equal to the natural note represented by the current
    /// <see cref="NoteLetter"/> instance to the current instance.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    private static int ARelativeHalfSteps(this NoteLetter letter) => letter switch
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

