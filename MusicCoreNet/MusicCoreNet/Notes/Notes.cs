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
    /// Gets a builder object that can be used to quickly create an 'A' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder A() => new(NoteLetter.A);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'B' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder B() => new(NoteLetter.B);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'C' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder C() => new(NoteLetter.C);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'D' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder D() => new(NoteLetter.D);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'E' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder E() => new(NoteLetter.E);

    /// <summary>
    /// Gets a builder object that can be used to quickly create an 'F' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder F() => new(NoteLetter.F);

    /// <summary>
    /// Gets a builder object that can be used to quickly create a 'G' <see cref="NoteClass"/> with a
    /// given accidental.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NoteClassBuilder G() => new(NoteLetter.G);
    #endregion
}
