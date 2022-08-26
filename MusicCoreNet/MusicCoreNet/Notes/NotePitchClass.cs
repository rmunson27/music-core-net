using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Extension methods and other static functionality for the <see cref="NotePitchClass"/> enum.
/// </summary>
public static class NotePitchClasses
{
    /// <summary>
    /// Gets the number of semitones from the current <see cref="NotePitchClass"/> instance up
    /// to <see cref="NotePitchClass.C"/>.
    /// </summary>
    /// <remarks>
    /// This is important for computing octave information.
    /// </remarks>
    /// <param name="pitchClass"></param>
    /// <returns></returns>
    internal static int SemitonesUpToC(this NotePitchClass pitchClass)
        // Convert from A relative: Subtract from 15 (12 so are going up instead of down, then subtract -3 so C is at 0)
        // Then mod by 12 (so is in range)
        => (15 - (int)pitchClass) % 12;

    /// <summary>
    /// Gets the number of semitones from the current <see cref="NotePitchClass"/> instance down
    /// to <see cref="NotePitchClass.C"/>.
    /// </summary>
    /// <remarks>
    /// This is important for computing octave information.
    /// </remarks>
    /// <param name="pitchClass"></param>
    /// <returns></returns>
    internal static int SemitonesDownToC(this NotePitchClass pitchClass)
        // Convert from A relative: +9 (-3 so C is at 0, +12 so is positive), %12 (so is in range)
        => ((int)pitchClass + 9) % 12;

    /// <summary>
    /// Gets a <see cref="NotePitchClass"/> from a number of semitones down to <see cref="NotePitchClass.C"/>.
    /// </summary>
    /// <remarks>
    /// This is important for computing octave information.
    /// </remarks>
    /// <param name="index"></param>
    /// <returns></returns>
    internal static NotePitchClass FromSemitonesDownToC(int index)
        // Convert to A relative: +15 (+3 so A is at 0, +12 so is positive), %12 (so is in range)
        => (NotePitchClass)((index + 15) % 12);

    /// <summary>
    /// Gets the <see cref="NotePitchInfo"/> equivalent to the current <see cref="NotePitchClass"/> equipped with
    /// the supplied octave number.
    /// </summary>
    /// <param name="pitchClass"></param>
    /// <param name="Octave"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException">The current instance was an unnamed enum value.</exception>
    public static NotePitchInfo WithOctave([NamedEnum] this NotePitchClass pitchClass, int Octave)
        => new(Throw.IfEnumArgUnnamed(pitchClass, nameof(pitchClass)), Octave);
}

/// <summary>
/// Represents the pitch class of a note, up to octave difference.
/// </summary>
public enum NotePitchClass : byte
{
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
}
