using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the class of a written note, independent of octave.
/// </summary>
/// <remarks>
/// The default value of this struct represents A natural.
/// </remarks>
/// <param name="Letter">The letter of the note class to create.</param>
/// <param name="Accidental">The accidental of the note class to create.</param>
/// <exception cref="InvalidEnumArgumentException"><paramref name="Letter"/> was an unnamed enum value.</exception>
public readonly record struct NoteClass(NoteLetter Letter, Accidental Accidental = default)
{
    #region Properties And Fields
    /// <summary>
    /// Gets or initializes the letter of this note class.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    public NoteLetter Letter
    {
        get => _letter;
        init => _letter = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly NoteLetter _letter = Throw.IfEnumArgUnnamed(Letter, nameof(Letter));
    #endregion

    #region Methods
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

    #region Arithmetic
    /// <summary>
    /// Gets the difference between the two <see cref="NoteClass"/> instances passed in as an instance
    /// of <see cref="SimpleIntervalBase"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static SimpleIntervalBase operator -(NoteClass lhs, NoteClass rhs)
    {
#pragma warning disable CS8509 // This should handle everything
        return lhs.Letter.Minus(rhs.Letter) switch
#pragma warning restore CS8509
        {
            PerfectableSimpleIntervalBase pi
                => pi with { Quality = pi.Quality.Shift(lhs.Accidental.IntValue - rhs.Accidental.IntValue) },
            NonPerfectableSimpleIntervalBase npi
                => npi with { Quality = npi.Quality.Shift(lhs.Accidental.IntValue - rhs.Accidental.IntValue) },
        };
    }
    #endregion
    #endregion
}
