using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Static functionality relating to notes.
/// </summary>
public static class Notes
{
    #region Builder
    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'A' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder A() => new(NoteLetter.A);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'B' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder B() => new(NoteLetter.B);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'C' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder C() => new(NoteLetter.C);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'D' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder D() => new(NoteLetter.D);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'E' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder E() => new(NoteLetter.E);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'F' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder F() => new(NoteLetter.F);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'G' <see cref="NoteSpelling"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteSpellingBuilder G() => new(NoteLetter.G);
    #endregion
}
