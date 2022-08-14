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
    public static NotePitchClass ToPitchClass(this NoteLetter letter) => letter switch
    {
        A => NotePitchClass.A,
        B => NotePitchClass.B,
        C => NotePitchClass.C,
        D => NotePitchClass.D,
        E => NotePitchClass.E,
        F => NotePitchClass.F,
        G => NotePitchClass.G,
        _ => throw new InvalidEnumArgumentException($"Undefined {nameof(NoteLetter)} value."),
    };
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

