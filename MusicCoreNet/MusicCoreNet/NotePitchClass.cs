using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the pitch class of a note, up to octave difference.
/// </summary>
public enum NotePitchClass
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
