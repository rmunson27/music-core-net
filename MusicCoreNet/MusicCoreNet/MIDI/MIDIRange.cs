using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music.MIDI;

/// <summary>
/// Extensions and other static functionality relating to conversions between types in this library and the numerical
/// representation used in the standard Musical Instrument Digital Interface (MIDI).
/// </summary>
public static class MIDIRange
{
    #region Constants
    /// <summary>
    /// The highest pitch with a valid MIDI number.
    /// </summary>
    public static readonly NotePitch MaxPitch = new(NotePitchClass.G, 9);

    /// <summary>
    /// The lowest pitch with a valid MIDI number.
    /// </summary>
    public static readonly NotePitch MinPitch = new(NotePitchClass.C, -1);

    /// <summary>
    /// The maximum value of the MIDI number range.
    /// </summary>
    public const int MaxNumber = 127;

    /// <summary>
    /// The minimum value of the MIDI number range.
    /// </summary>
    public const int MinNumber = 0;
    #endregion

    #region Extensions
    #region Note
    /// <summary>
    /// Gets the MIDI number of the current instance if it is in the MIDI range.
    /// </summary>
    /// <remarks>
    /// Even if the instance is not in the MIDI range, the same algorithm will be used to set the value
    /// of <paramref name="number"/>.
    /// </remarks>
    /// <param name="note"></param>
    /// <param name="number"></param>
    /// <returns>
    /// Whether or not the current instance was in the MIDI range.
    /// </returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool TryGetMIDINumberInRange(this Note note, out int number)
        => TryGetMIDINumberInRange(note.Pitch, out number);

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="MIDINumber(Note)"/>, this method throws an exception if the result is not in the
    /// MIDI range.
    /// </remarks>
    /// <param name="note"></param>
    /// <returns>
    /// A MIDI number greater than or equal to <see cref="MinNumber"/> and less than or equal
    /// to <see cref="MaxNumber"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The current instance was not in the MIDI range.
    /// </exception>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    [return: GreaterThanOrEqualToInteger(MinNumber), LessThanOrEqualToInteger(MaxNumber)]
    public static int MIDINumberInRange(this Note note) => note.Pitch.MIDINumberInRange();

    /// <summary>
    /// Determines whether or not the current instance is in the MIDI range.
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool IsInMIDIRange(this Note note) => IsInMIDIRange(note.Pitch);

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// This will still return an integer outside of the MIDI range if the current instance is outside of the range.
    /// </remarks>
    /// <param name="note"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static int MIDINumber(this Note note) => note.Pitch.MIDINumber();
    #endregion

    #region NotePitch
    /// <summary>
    /// Gets the MIDI number of the current instance in an <see langword="out"/> parameter, returning whether or not
    /// it is in the MIDI range.
    /// </summary>
    /// <remarks>
    /// Even if the instance is not in the MIDI range, the same algorithm will be used to set the value
    /// of <paramref name="number"/>.
    /// </remarks>
    /// <param name="pitch"></param>
    /// <param name="number"></param>
    /// <returns>
    /// Whether or not the current instance was in the MIDI range.
    /// </returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool TryGetMIDINumberInRange(this NotePitch pitch, out int number)
    {
        number = pitch.MIDINumber();
        return IsInRange(number);
    }

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="MIDINumber(NotePitch)"/>, this method throws an exception if the result is not in the
    /// MIDI range.
    /// </remarks>
    /// <param name="pitch"></param>
    /// <returns>
    /// A MIDI number greater than or equal to <see cref="MinNumber"/> and less than or equal
    /// to <see cref="MaxNumber"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The result was not in the MIDI range.
    /// </exception>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    [return: GreaterThanOrEqualToInteger(MinNumber), LessThanOrEqualToInteger(MaxNumber)]
    public static int MIDINumberInRange(this NotePitch pitch)
    {
        var result = pitch.MIDINumber();
        return IsInRange(result)
                ? result
                : throw new InvalidOperationException($"Pitch is not in the MIDI range.");
    }

    /// <summary>
    /// Determines whether or not the current instance is in the MIDI range.
    /// </summary>
    /// <param name="pitch"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool IsInMIDIRange(this NotePitch pitch) => IsInRange(pitch.MIDINumber());

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// This will still return an integer outside of the MIDI range if the current instance is outside of the range.
    /// </remarks>
    /// <param name="pitch"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static int MIDINumber(this NotePitch pitch) => pitch - MinPitch;
    #endregion
    #endregion

    #region Factories
    /// <summary>
    /// Converts a MIDI number to the equivalent <see cref="NotePitch"/>.
    /// </summary>
    /// <remarks>
    /// This method will treat numbers not in the MIDI range as well as numbers that are.
    /// </remarks>
    /// <param name="Number"></param>
    /// <returns></returns>
    public static NotePitch PitchFromNumber(int Number) => NotePitch.FromC0Index(Number + MinPitch.C0Index);
    #endregion

    #region Helpers
    /// <summary>
    /// Determines if the integer passed in is in the MIDI number range.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <seealso cref="MaxNumber"/>
    /// <seealso cref="MinNumber"/>
    public static bool IsInRange(int n) => n >= MinNumber && n <= MaxNumber;
    #endregion
}
