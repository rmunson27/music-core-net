using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

using static PerfectableSimpleIntervalNumber;
using static NonPerfectableSimpleIntervalNumber;

/// <summary>
/// Helper methods for dealing with the circle of fifths.
/// </summary>
public static class CircleOfFifths
{
    #region Number
    internal static int UnisonBasedNumberIndex(this SimpleIntervalBase siBase)
#pragma warning disable CS8509 // This should handle everything
        => Throw.IfArgNull(siBase, nameof(siBase)) switch
#pragma warning restore CS8509
        {
            PerfectableSimpleIntervalBase(_, var number) => number.UnisonBasedPerfectIndex(),
            NonPerfectableSimpleIntervalBase(_, var number) => number.UnisonBasedMajorIndex(),
        };

    {

    #endregion
}
