using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// A struct that can be used to quickly build <see cref="Note"/> and <see cref="NoteSpelling"/> instances.
/// </summary>
public readonly struct NoteBuilder
{
    /// <summary>
    /// Stores the letter of the <see cref="NoteSpelling"/> that is to be constructed.
    /// </summary>
    private NoteLetter Letter { get; }

    internal NoteBuilder(NoteLetter Letter)
    {
        this.Letter = Letter;
    }

    /// <summary>
    /// Creates the <see cref="NoteSpelling"/> representing a natural with the note letter represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteSpelling Natural() => new(Letter, Accidental.Natural);

    /// <summary>
    /// Creates the <see cref="NoteSpelling"/> representing a sharp with the note letter represented by this instance and
    /// the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteSpelling Sharp([Positive] int Degree = 1)
        => new(Letter, Accidental.Sharp(Throw.IfArgNotPositive(Degree, nameof(Degree))));

    /// <summary>
    /// Creates the <see cref="NoteSpelling"/> representing a flat with the note letter represented by this instance and
    /// the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteSpelling Flat([Positive] int Degree = 1)
        => new(Letter, Accidental.Flat(Throw.IfArgNotPositive(Degree, nameof(Degree))));

    /// <summary>
    /// Creates the <see cref="Note"/> representing a natural with the note letter represented by this instance and the
    /// octave passed in.
    /// </summary>
    /// <param name="Octave"></param>
    /// <returns></returns>
    public Note WithOctave(int Octave) => new(new(Letter), Octave);

    /// <summary>
    /// Implicitly converts a <see cref="NoteBuilder"/> instance to the natural <see cref="NoteSpelling"/> represented
    /// by its state.
    /// </summary>
    /// <param name="builder"></param>
    public static implicit operator NoteSpelling(NoteBuilder builder) => new(builder.Letter);
}
