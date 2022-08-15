using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music.Internal;

/// <summary>
/// Internal mathematics helpers.
/// </summary>
internal static class Maths
{
    /// <summary>
    /// Computes the floor division of the numerator and denominator passed in, returning the floor remainder in
    /// an <see langword="out"/> parameter.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="d"></param>
    /// <param name="remainder"></param>
    /// <returns></returns>
    public static int FloorDivRem(int n, int d, out int remainder)
    {
        remainder = n % d;
        var quot = n / d;
        if (n < 0 && remainder != 0) { remainder += d; quot--; }
        return quot;
    }
}
