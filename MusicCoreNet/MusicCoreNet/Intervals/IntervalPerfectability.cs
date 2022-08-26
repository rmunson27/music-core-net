using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Represents the perfectability of an interval (either perfectable or imperfectable).
/// </summary>
public enum IntervalPerfectability : byte
{
    /// <summary>
    /// Represents perfectable intervals (i.e. a perfect fourth).
    /// </summary>
    Perfectable,

    /// <summary>
    /// Represents imperfectable intervals (i.e. a major second).
    /// </summary>
    Imperfectable,
}

