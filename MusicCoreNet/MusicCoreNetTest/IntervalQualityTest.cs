using Rem.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="PerfectableIntervalQuality"/> and <see cref="NonPerfectableIntervalQuality"/> structs.
/// </summary>
[TestClass]
public class IntervalQualityTest
{
    #region Properties And Fields
    private static readonly Random Random = new();
    #endregion

    #region Perfectable
    /// <summary>
    /// Tests the factory method construction and characterization of
    /// <see cref="PerfectableIntervalQuality"/> instances.
    /// </summary>
    [TestMethod, TestCategory(nameof(PerfectableIntervalQuality))]
    public void TestPerfectableCharacterization()
    {
        var randomDegree = Random.Next(10) + 2;

        var randomDiminished = PerfectableIntervalQuality.Diminished(randomDegree);
        var singleDiminished = PerfectableIntervalQuality.Diminished();
        var singleAugmented = PerfectableIntervalQuality.Augmented();
        var randomAugmented = PerfectableIntervalQuality.Augmented(randomDegree);

        int diminishedDegree, augmentedDegree;

        Assert.IsTrue(singleDiminished.IsDiminished());
        Assert.IsTrue(singleDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(1, diminishedDegree);

        Assert.IsTrue(randomDiminished.IsDiminished());
        Assert.IsTrue(randomDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(randomDegree, diminishedDegree);

        Assert.IsTrue(PerfectableIntervalQuality.Perfect.IsPerfect());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);
    }
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Tests the factory method construction and characterization of
    /// <see cref="NonPerfectableIntervalQuality"/> instances.
    /// </summary>
    [TestMethod, TestCategory(nameof(NonPerfectableIntervalQuality))]
    public void TestNonPerfectableCharacterization()
    {
        var randomDegree = Random.Next(10) + 2;

        var randomDiminished = NonPerfectableIntervalQuality.Diminished(randomDegree);
        var singleDiminished = NonPerfectableIntervalQuality.Diminished();
        var singleAugmented = NonPerfectableIntervalQuality.Augmented();
        var randomAugmented = NonPerfectableIntervalQuality.Augmented(randomDegree);

        int diminishedDegree, augmentedDegree;

        Assert.IsTrue(singleDiminished.IsDiminished());
        Assert.IsTrue(singleDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(1, diminishedDegree);

        Assert.IsTrue(randomDiminished.IsDiminished());
        Assert.IsTrue(randomDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(randomDegree, diminishedDegree);

        Assert.IsTrue(NonPerfectableIntervalQuality.Minor.IsMinor());
        Assert.IsTrue(NonPerfectableIntervalQuality.Major.IsMajor());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);
    }
    #endregion
}
