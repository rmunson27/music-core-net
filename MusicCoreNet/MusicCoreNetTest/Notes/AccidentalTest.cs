using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="Accidental"/> struct.
/// </summary>
[TestClass]
public class AccidentalTest
{
    /// <summary>
    /// Tests the <see cref="Accidental.ToASCIIMusicalNotationString"/> method.
    /// </summary>
    /// <remarks>
    /// Since <see cref="Accidental.ToUnicodeMusicalNotationString(bool)"/> and
    /// <see cref="Accidental.ToUTF32MusicalNotationString(bool)"/> use the same internal method to compute their
    /// results, this method suffices to test those methods as well.
    /// </remarks>
    [TestMethod]
    public void TestToASCIIMusicalNotationString()
    {
        Assert.AreEqual("#x", Accidental.Sharp(3).ToASCIIMusicalNotationString());
        Assert.AreEqual("x", Accidental.Sharp(2).ToASCIIMusicalNotationString());
        Assert.AreEqual("#", Accidental.Sharp().ToASCIIMusicalNotationString());
        Assert.AreEqual(string.Empty, Accidental.Natural.ToASCIIMusicalNotationString());
        Assert.AreEqual("b", Accidental.Flat().ToASCIIMusicalNotationString());
        Assert.AreEqual("bb", Accidental.Flat(2).ToASCIIMusicalNotationString());
    }
}
