using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music.MIDI;

/// <summary>
/// Static functionality relating to conversions between note and pitch types in this library and the standard
/// Musical Instrument Digital Interface (MIDI).
/// </summary>
public static class MIDINotes
{
    #region Constants
    /// <summary>
    /// The maximum value of the MIDI number range.
    /// </summary>
    public const int MaxNumber = 127;

    /// <summary>
    /// A <see cref="NotePitchInfo"/> representing the highest pitch with a valid MIDI number.
    /// </summary>
    public static readonly NotePitchInfo MaxPitch = new(NotePitchClass.G, 9);

    /// <summary>
    /// The minimum value of the MIDI number range.
    /// </summary>
    public const int MinNumber = 0;

    /// <summary>
    /// A <see cref="NotePitchInfo"/> representing the lowest pitch with a valid MIDI number.
    /// </summary>
    public static readonly NotePitchInfo MinPitch = new(NotePitchClass.C, -1);
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

    #region NotePitchInfo
    /// <summary>
    /// Gets the MIDI number of the current instance if it is in the MIDI range.
    /// </summary>
    /// <remarks>
    /// Even if the instance is not in the MIDI range, the same algorithm will be used to set the value
    /// of <paramref name="number"/>.
    /// </remarks>
    /// <param name="info"></param>
    /// <param name="number"></param>
    /// <returns>
    /// Whether or not the current instance was in the MIDI range.
    /// </returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool TryGetMIDINumberInRange(this NotePitchInfo info, out int number)
    {
        number = info.MIDINumber();
        return IsNumberInRange(number);
    }

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="MIDINumber(NotePitchInfo)"/>, this method throws an exception if the result is not in the
    /// MIDI range.
    /// </remarks>
    /// <param name="info"></param>
    /// <returns>
    /// A MIDI number greater than or equal to <see cref="MinNumber"/> and less than or equal
    /// to <see cref="MaxNumber"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The result was not in the MIDI range.
    /// </exception>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static int MIDINumberInRange(this NotePitchInfo info)
    {
        var result = info.MIDINumber();
        return IsNumberInRange(result)
                ? result
                : throw new InvalidOperationException($"Pitch is not in the MIDI range.");
    }

    /// <summary>
    /// Determines whether or not the current instance is in the MIDI range.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static bool IsInMIDIRange(this NotePitchInfo info) => IsNumberInRange(info.MIDINumber());

    /// <summary>
    /// Gets the MIDI number of the current instance.
    /// </summary>
    /// <remarks>
    /// This will still return an integer outside of the MIDI range if the current instance is outside of the range.
    /// </remarks>
    /// <param name="info"></param>
    /// <returns></returns>
    /// <seealso cref="MaxPitch"/>
    /// <seealso cref="MinPitch"/>
    public static int MIDINumber(this NotePitchInfo info) => info - MinPitch;
    #endregion
    #endregion

    #region Factories
    /// <summary>
    /// Converts a MIDI number to the equivalent <see cref="NotePitchInfo"/>.
    /// </summary>
    /// <remarks>
    /// This method will treat numbers not in the MIDI range as well as numbers that are.
    /// </remarks>
    /// <param name="Number"></param>
    /// <returns></returns>
    public static NotePitchInfo PitchFromNumber(int Number) => NotePitchInfo.FromC0Index(Number + MinPitch.C0Index);
    #endregion

    #region Helpers
    /// <summary>
    /// Determines if the integer passed in is in the MIDI number range.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <seealso cref="MaxNumber"/>
    /// <seealso cref="MinNumber"/>
    public static bool IsNumberInRange(int n) => n >= MinNumber && n <= MaxNumber;
    #endregion
}
