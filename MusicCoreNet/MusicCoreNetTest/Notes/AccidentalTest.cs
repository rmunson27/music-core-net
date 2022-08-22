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
    /// Tests the <see cref="Accidental.ToMusicalNotationString"/> method.
    /// </summary>
    [TestMethod]
    public void TestToMusicalNotationString()
    {
        Assert.AreEqual("#x", Accidental.Sharp(3).ToMusicalNotationString());
        Assert.AreEqual("x", Accidental.Sharp(2).ToMusicalNotationString());
        Assert.AreEqual("#", Accidental.Sharp().ToMusicalNotationString());
        Assert.AreEqual(string.Empty, Accidental.Natural.ToMusicalNotationString());
        Assert.AreEqual("b", Accidental.Flat().ToMusicalNotationString());
        Assert.AreEqual("bb", Accidental.Flat(2).ToMusicalNotationString());
    }
}
