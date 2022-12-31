using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents an accidental of a musical note.
/// </summary>
/// <remarks>
/// The default instance of this struct represents a natural.
/// </remarks>
public readonly record struct Accidental
{
    #region Constants
    /// <summary>
    /// Contains string constants for representing accidentals.
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Contains string constants for representing accidentals in ASCII.
        /// </summary>
        public static class ASCII
        {
            /// <inheritdoc cref="UTF32.DoubleSharp"/>
            public const string DoubleSharp = "x";

            /// <inheritdoc cref="UTF32.Sharp"/>
            public const string Sharp = "#";

            /// <inheritdoc cref="UTF32.Flat"/>
            public const string Flat = "b";
        }

        /// <summary>
        /// Contains string constants for representing accidentals in unicode (UTF-16) encoding.
        /// </summary>
        public static class Unicode
        {
            /// <inheritdoc cref="UTF32.DoubleSharp"/>
            public const string DoubleSharp = "x";

            /// <inheritdoc cref="UTF32.Sharp"/>
            public const string Sharp = "\x266D";

            /// <inheritdoc cref="UTF32.Natural"/>
            public const string Natural = "\x266E";

            /// <inheritdoc cref="UTF32.Flat"/>
            public const string Flat = "\x266F";
        }

        /// <summary>
        /// Contains string constants for representing accidentals in UTF-32 encoding.
        /// </summary>
        public static class UTF32
        {
            /// <summary>
            /// A string representing a double sharp.
            /// </summary>
            public static readonly string DoubleSharp = Encoding.UTF32.GetString(new byte[] { 0x2A, 0xD1, 1, 0 });

            /// <summary>
            /// A string representing a sharp.
            /// </summary>
            public static readonly string Sharp = Encoding.UTF32.GetString(new byte[] { 0x6D, 0x26, 0, 0 });

            /// <summary>
            /// A string representing a natural.
            /// </summary>
            public static readonly string Natural = Encoding.UTF32.GetString(new byte[] { 0x6E, 0x26, 0, 0 });

            /// <summary>
            /// A string representing a flat.
            /// </summary>
            public static readonly string Flat = Encoding.UTF32.GetString(new byte[] { 0x6F, 0x26, 0, 0 });
        }
    }

    /// <summary>
    /// An accidental representing a natural.
    /// </summary>
    public static readonly Accidental Natural = new(0);
    #endregion

    #region Properties
    /// <summary>
    /// Gets the kind of this accidental (sharp, natural or flat).
    /// </summary>
    public AccidentalKind Kind => new(unchecked((AccidentalKind.Values)Math.Sign(Modification)));

    /// <summary>
    /// Gets an integer describing the modification this instance makes to a pitch of a note to which it is applied.
    /// </summary>
    /// <remarks>
    /// This will be positive if the current instance is sharp, zero if it is natural, and negative if it is flat,
    /// with the magnitude describing the degree of the modification.
    /// </remarks>
    public int Modification { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of this struct.
    /// </summary>
    /// <param name="Modification"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Accidental(int Modification) { this.Modification = Modification; }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="Accidental"/> representing the specified modification.
    /// </summary>
    /// <remarks>
    /// The value returned will be a sharp if <paramref name="Modification"/> is positive, natural if
    /// <paramref name="Modification"/> is zero, and flat if <paramref name="Modification"/> is negative.
    /// </remarks>
    /// <param name="Modification"></param>
    /// <returns></returns>
    public static Accidental FromModification(int Modification) => new(Modification);

    /// <summary>
    /// Creates a new <see cref="Accidental"/> representing a sharp with the given integer degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static Accidental Sharp([Positive] int Degree = 1) => new(Throw.IfArgNotPositive(Degree, nameof(Degree)));

    /// <summary>
    /// Creates a new <see cref="Accidental"/> representing a flat with the given integer degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was not positive.</exception>
    public static Accidental Flat([Positive] int Degree = 1) => new(-Throw.IfArgNotPositive(Degree, nameof(Degree)));
    #endregion

    #region Classification
    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is sharp, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsSharp([NonNegative] out int Degree)
        => IsSharp() ? Try.Success(out Degree, Modification) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is sharp.
    /// </summary>
    /// <returns></returns>
    public bool IsSharp() => Modification > 0;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is natural.
    /// </summary>
    /// <returns></returns>
    public bool IsNatural() => Modification == 0;

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat, returning the degree to which it is in an
    /// <see langword="out"/> parameter if so.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    public bool IsFlat([NonNegative] out int Degree)
        => IsFlat() ? Try.Success(out Degree, -Modification) : Try.Failure(out Degree);

    /// <summary>
    /// Gets whether or not the current <see cref="Accidental"/> is flat.
    /// </summary>
    /// <returns></returns>
    public bool IsFlat() => Modification < 0;
    #endregion

    #region Modifiers
    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance sharpened by the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was negative.</exception>
    public Accidental SharpenedBy([NonNegative] int Degree = 1)
        => new(Modification + Throw.IfArgNegative(Degree, nameof(Degree)));

    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance flattened by the given degree.
    /// </summary>
    /// <param name="Degree"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Degree"/> was negative.</exception>
    public Accidental FlattenedBy([NonNegative] int Degree = 1)
        => new(Modification - Throw.IfArgNegative(Degree, nameof(Degree)));

    /// <summary>
    /// Gets an <see cref="Accidental"/> equivalent to the current instance sharpened or flattened by a degree
    /// determined by the magnitude and sign of the integer passed in.
    /// </summary>
    /// <remarks>
    /// The returned value will be sharper than the current instance if <paramref name="Amount"/> is positive,
    /// and flatter if <paramref name="Amount"/> is negative.
    /// </remarks>
    /// <param name="Amount"></param>
    /// <returns></returns>
    public Accidental ShiftedBy(int Amount) => new(Modification + Amount);
    #endregion

    #region Equality
    /// <summary>
    /// Determines whether the current instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Accidental other) => Modification == other.Modification;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Modification;
    #endregion

    #region ToString
    /// <summary>
    /// Gets a <see cref="string"/> that represents the current <see cref="Accidental"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Modification switch
    {
        < 0 => $"Flat {{ Degree = {-Modification} }}",
        0 => "Natural",
        > 0 => $"Sharp {{ Degree = {Modification} }}",
    };

    /// <summary>
    /// Gets a musical notation string that represents this instance using ASCII characters.
    /// </summary>
    /// <remarks>
    /// This method chooses look-alikes for all the accidental characters, as they are only present in unicode
    /// (UTF-16) and wider.
    /// <para/>
    /// Unlike the <see cref="ToUnicodeMusicalNotationString(bool)"/> and
    /// <see cref="ToUTF32MusicalNotationString(bool)"/> methods, this method does not have a reasonable character
    /// to describe the <see cref="Natural"/> accidental, so the option to include it is not provided, and the method
    /// will return <see cref="string.Empty"/> if this instance is <see cref="Natural"/>.
    /// </remarks>
    /// <returns></returns>
    /// <seealso cref="Strings.ASCII"/>
    public string ToASCIIMusicalNotationString()
        => ToMusicalNotationString(
            flat: Strings.ASCII.Flat,
            natural: string.Empty, // No reasonable natural character
            sharp: Strings.ASCII.Sharp,
            doubleSharp: Strings.ASCII.DoubleSharp);

    /// <summary>
    /// Gets a musical notation string that represents this instance using unicode (UTF-16) characters.
    /// </summary>
    /// <remarks>
    /// This method can properly display all the accidental characters except for the double sharp character, for
    /// which 'x' is used.
    /// </remarks>
    /// <param name="showNatural">
    /// Whether or not to display the natural symbol if this instance is <see cref="Natural"/>.
    /// </param>
    /// <returns></returns>
    /// <seealso cref="Strings.Unicode"/>
    public string ToUnicodeMusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(
            flat: Strings.Unicode.Flat,
            natural: showNatural ? Strings.Unicode.Natural : string.Empty,
            sharp: Strings.Unicode.Sharp,
            doubleSharp: Strings.Unicode.DoubleSharp);

    /// <summary>
    /// Gets a musical notation string that represents this instance using UTF-32 characters.
    /// characters.
    /// </summary>
    /// <remarks>
    /// This is the only method that can properly display all the accidental characters without choosing look-alikes.
    /// </remarks>
    /// <param name="showNatural">
    /// Whether or not to display the natural symbol if this instance is <see cref="Natural"/>.
    /// </param>
    /// <returns></returns>
    /// <seealso cref="Strings.UTF32"/>
    public string ToUTF32MusicalNotationString(bool showNatural = false)
        => ToMusicalNotationString(
            flat: Strings.UTF32.Flat,
            natural: showNatural ? Strings.UTF32.Natural : string.Empty,
            sharp: Strings.UTF32.Sharp,
            doubleSharp: Strings.UTF32.DoubleSharp);

    /// <summary>
    /// Gets a musical notation string that represents the current <see cref="Accidental"/> using the given strings
    /// to indicate sharps, flats and double sharps.
    /// </summary>
    /// <returns></returns>
    private string ToMusicalNotationString(string flat, string natural, string sharp, string doubleSharp)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static StringBuilder AddRepeat(StringBuilder builder, string s, int count)
        {
            for (int i = 0; i < count; i++) builder.Append(s);
            return builder;
        }

        if (IsSharp(out int sharpDegree))
        {
            StringBuilder resultBuilder = new();
            if (sharpDegree % 2 != 0) resultBuilder.Append(sharp);
            return AddRepeat(resultBuilder, doubleSharp, sharpDegree / 2).ToString();
        }
        else if (IsFlat(out int flatDegree))
        {
            return AddRepeat(new StringBuilder(), flat, flatDegree).ToString();
        }
        else return natural;
    }
    #endregion
    #endregion
}

/// <summary>
/// Represents the kind of an accidental that actually modifies the note (sharp or flat).
/// </summary>
/// <remarks>
/// The default instance of this type represents sharps.
/// </remarks>
public readonly record struct ModifyingAccidentalKind
{
    /// <inheritdoc cref="Values.Sharp"/>
    /// <remarks>
    /// This is the default instance of its type.
    /// </remarks>
    public static readonly ModifyingAccidentalKind Sharp = new(Values.Sharp);

    /// <inheritdoc cref="Values.Flat"/>
    public static readonly ModifyingAccidentalKind Flat = new(Values.Flat);

    /// <summary>
    /// The offset added to the <see cref="AccidentalKind.Value"/> property of the equivalent
    /// <see cref="AccidentalKind"/> instance to compute <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// This makes <see cref="Sharp"/> the default.
    /// </remarks>
    internal const int ValueOffset = -(int)AccidentalKind.Values.Sharp;

    /// <summary>
    /// Uniquely represents this instance as an <see langword="enum"/> value.
    /// </summary>
    public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the given value.
    /// </summary>
    internal ModifyingAccidentalKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Explicitly converts an <see cref="AccidentalKind"/> to a <see cref="ModifyingAccidentalKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    /// <exception cref="InvalidCastException">The cast was invalid.</exception>
    public static explicit operator ModifyingAccidentalKind(AccidentalKind kind)
        => kind.Value == AccidentalKind.Values.Natural
            ? throw new InvalidCastException("The natural accidental does not modify notes.")
            : new((Values)unchecked((int)kind.Value + ValueOffset));

    /// <summary>
    /// Represents all possible values of type <see cref="ModifyingAccidentalKind"/>.
    /// </summary>
    public enum Values : sbyte
    {
        /// <inheritdoc cref="AccidentalKind.Values.Sharp"/>
        Sharp = AccidentalKind.Values.Sharp + ValueOffset,

        /// <inheritdoc cref="AccidentalKind.Values.Flat"/>
        Flat = AccidentalKind.Values.Flat + ValueOffset,
    }
}

/// <summary>
/// Represents the kind of an accidental (sharp, natural or flat).
/// </summary>
/// <remarks>
/// The default value of this struct represents the natural accidental.
/// </remarks>
public readonly record struct AccidentalKind
{
    /// <inheritdoc cref="Values.Sharp"/>
    public static readonly AccidentalKind Sharp = new(Values.Sharp);

    /// <inheritdoc cref="Values.Natural"/>
    /// <remarks>
    /// This is the default instance of its type.
    /// </remarks>
    public static readonly AccidentalKind Natural = new(Values.Natural);

    /// <inheritdoc cref="Values.Flat"/>
    public static readonly AccidentalKind Flat = new(Values.Flat);

    /// <summary>
    /// Uniquely represents this instance as an <see langword="enum"/> value.
    /// </summary>
    public Values Value { get; }

    /// <summary>
    /// Constructs a new instance with the given value.
    /// </summary>
    internal AccidentalKind([NameableEnum] Values Value) { this.Value = Value; }

    /// <summary>
    /// Gets a string representing this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Implicitly converts a <see cref="ModifyingAccidentalKind"/> to an <see cref="AccidentalKind"/>.
    /// </summary>
    /// <param name="kind"></param>
    public static implicit operator AccidentalKind(ModifyingAccidentalKind kind)
        => new((Values)unchecked((int)kind.Value - ModifyingAccidentalKind.ValueOffset));

    /// <param name="kind">
    /// An <see langword="out"/> parameter to set to the equivalent <see cref="ModifyingAccidentalKind"/> instance if
    /// this instance represents accidentals that modify notes.
    /// <para/>
    /// This will be set to <see langword="default"/> if this instance represents the natural accidental.
    /// </param>
    /// <inheritdoc cref="IsModifying()"/>
    public bool IsModifying(out ModifyingAccidentalKind kind)
        => IsModifying() ? Try.Success(out kind, AsModifyingUnsafe()) : Try.Failure(out kind);

    /// <summary>
    /// Gets whether or not this instance represents accidentals that actually modify notes (sharps or flats).
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this instance represents accidentals that modify notes, otherwise <see langword="false"/>.
    /// </returns>
    public bool IsModifying() => Value is not Values.Natural;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ModifyingAccidentalKind AsModifyingUnsafe()
        => new((ModifyingAccidentalKind.Values)unchecked((int)Value + ModifyingAccidentalKind.ValueOffset));

    /// <summary>
    /// Represents all possible values of type <see cref="AccidentalKind"/>.
    /// </summary>
    public enum Values : sbyte
    {
        /// <summary>
        /// Represents flat accidentals.
        /// </summary>
        Flat = -1,

        /// <summary>
        /// Represents the natural accidental.
        /// </summary>
        Natural = 0,

        /// <summary>
        /// Represents sharp accidentals.
        /// </summary>
        Sharp = 1,
    }
}
