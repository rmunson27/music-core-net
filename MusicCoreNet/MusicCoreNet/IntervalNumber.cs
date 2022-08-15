using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the number of a perfectable interval.
/// </summary>
public enum PerfectableIntervalNumber : byte
{
    /// <summary>
    /// Represents a unison.
    /// </summary>
    Unison = 1,

    /// <summary>
    /// Represents a fourth.
    /// </summary>
    Fourth = 4,

    /// <summary>
    /// Represents a fifth.
    /// </summary>
    Fifth = 5,
}

/// <summary>
/// Represents the number of a non-perfectable interval.
/// </summary>
public enum NonPerfectableIntervalNumber : byte
{
    /// <summary>
    /// Represents a second.
    /// </summary>
    Second = 2,

    /// <summary>
    /// Represents a third.
    /// </summary>
    Third = 3,

    /// <summary>
    /// Represents a sixth.
    /// </summary>
    Sixth = 6,

    /// <summary>
    /// Represents a seventh.
    /// </summary>
    Seventh = 7,
}

