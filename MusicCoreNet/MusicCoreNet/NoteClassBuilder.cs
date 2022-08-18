using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// A struct that can be used to quickly build <see cref="NoteClass"/> instances.
/// </summary>
public readonly struct NoteClassBuilder
{
    /// <summary>
    /// Stores the letter of the <see cref="NoteClass"/> that is to be constructed.
    /// </summary>
    private NoteLetter Letter { get; }

    internal NoteClassBuilder(NoteLetter Letter)
    {
        this.Letter = Letter;
    }

    /// <summary>
    /// Creates the <see cref="NoteClass"/> representing a natural with the note letter represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteClass Natural() => new(Letter, Accidental.Natural);

    /// <summary>
    /// Creates the <see cref="NoteClass"/> representing a sharp with the note letter represented by this instance and
    /// the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteClass Sharp([Positive] int Degree = 1)
        => new(Letter, Accidental.Sharp(Throw.IfArgNotPositive(Degree, nameof(Degree))));

    /// <summary>
    /// Creates the <see cref="NoteClass"/> representing a flat with the note letter represented by this instance and
    /// the degree passed in.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NoteClass Flat([Positive] int Degree = 1)
        => new(Letter, Accidental.Flat(Throw.IfArgNotPositive(Degree, nameof(Degree))));
}
