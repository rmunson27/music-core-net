using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

using static PerfectableSimpleIntervalNumber;
using static NonPerfectableSimpleIntervalNumber;

/// <summary>
/// Tests of the simple interval number types included in the library.
/// </summary>
[TestClass]
public class SimpleIntervalNumberTest
{
    /// <summary>
    /// Ensures that the default instance of <see cref="SimpleIntervalNumber"/> is as advertised in the documentation.
    /// </summary>
    [TestMethod]
    public void TestDefault()
    {
        // First make sure they compare as equal
        Assert.AreEqual(SimpleIntervalNumber.Unison, default(SimpleIntervalNumber));

        // Second ensure that the default actually stores the expected values
        Assert.IsTrue(default(SimpleIntervalNumber).IsPerfectable(out var defaultPNumber));
        Assert.AreEqual(SimpleIntervalNumber.Unison, defaultPNumber);
    }
}
