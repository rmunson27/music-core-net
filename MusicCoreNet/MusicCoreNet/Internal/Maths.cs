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
    /// Gets the ordinal suffix for the current <see cref="int"/>.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> was negative.</exception>
    public static string OrdinalSuffix(this int number) => number switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(number), number, "Number must be non-negative."),
        _ => (number % 100) switch
        {
            11 => "th",
            12 => "th",
            13 => "th",
            _ => (number % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            },
        },
    };

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
