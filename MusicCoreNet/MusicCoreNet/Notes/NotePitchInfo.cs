using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents information for a pitch that can be represented by a note.
/// </summary>
/// <param name="Class">The class of the represented pitch.</param>
/// <param name="Octave">The octave of the represented pitch.</param>
/// <exception cref="InvalidEnumArgumentException"><paramref name="Class"/> was an unnamed enum value.</exception>
public readonly record struct NotePitchInfo(NotePitchClass Class, int Octave)
{
    /// <summary>
    /// Gets or initializes the class of the represented pitch.
    /// </summary>
    /// <exception cref="InvalidEnumPropertySetException">
    /// This property was initialized to an unnamed enum value.
    /// </exception>
    public NotePitchClass Class
    {
        get => _class;
        init => _class = Throw.IfEnumPropSetUnnamed(value);
    }
    private readonly NotePitchClass _class = Throw.IfEnumArgUnnamed(Class, nameof(Class));
}
