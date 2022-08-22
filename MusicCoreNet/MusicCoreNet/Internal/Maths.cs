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
        var quotient = n / d;
        if (n < 0 && remainder != 0) { remainder += d; quotient--; }
        return quotient;
    }

    /// <summary>
    /// Computes the floor quotient of the numerator and denominator passed in.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static int FloorDiv(int n, int d)
    {
        var quotient = n / d;
        if (n < 0 && n % d != 0) quotient--;
        return quotient;
    }

    /// <summary>
    /// Computes the floor remainder of the numerator and denominator passed in.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static int FloorRem(int n, int d)
    {
        var remainder = n % d;
        if (n < 0 && remainder != 0) remainder += d;
        return remainder;
    }
}
