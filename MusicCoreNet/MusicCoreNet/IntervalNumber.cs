using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the number of a perfectable interval.
/// </summary>
/// <remarks>
/// The underlying values of this type are stored in circle-of-fifths order with respect to <see cref="Unison"/> being
/// set to 0.
/// </remarks>
public enum PerfectableIntervalNumber : sbyte
{
    /// <summary>
    /// Represents a unison.
    /// </summary>
    Unison = 0,

    /// <summary>
    /// Represents a fourth.
    /// </summary>
    Fourth = -1,

    /// <summary>
    /// Represents a fifth.
    /// </summary>
    Fifth = 1,
}

/// <summary>
/// Represents the number of a non-perfectable interval.
/// </summary>
/// <remarks>
/// The underlying values of this type are stored in circle-of-fifths order with respect to
/// <see cref="PerfectableIntervalNumber.Unison"/> being set to 0.
/// </remarks>
public enum NonPerfectableIntervalNumber : sbyte
{
    /// <summary>
    /// Represents a second.
    /// </summary>
    Second = 2,

    /// <summary>
    /// Represents a third.
    /// </summary>
    Third = 4,

    /// <summary>
    /// Represents a sixth.
    /// </summary>
    Sixth = 3,

    /// <summary>
    /// Represents a seventh.
    /// </summary>
    Seventh = 5,
}

