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

        Assert.IsTrue(IntervalQualities.Perfect.IsPerfect());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);

        Assert.IsFalse(IntervalQualities.Perfect.IsDiminished());
        Assert.IsFalse(IntervalQualities.Perfect.IsAugmented());
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

        Assert.IsTrue(IntervalQualities.Minor.IsMinor());
        Assert.IsTrue(IntervalQualities.Major.IsMajor());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);

        Assert.IsFalse(IntervalQualities.Major.IsDiminished());
        Assert.IsFalse(IntervalQualities.Major.IsAugmented());
        Assert.IsFalse(IntervalQualities.Minor.IsDiminished());
        Assert.IsFalse(IntervalQualities.Minor.IsAugmented());
    }

    /// <summary>
    /// Tests inversion of a non-perfectable interval quality.
    /// </summary>
    [TestMethod, TestCategory(nameof(NonPerfectableIntervalQuality))]
    public void TestNonPerfectableInversion()
    {
        Assert.AreEqual(IntervalQualities.Major, IntervalQualities.Minor.Inversion());
        Assert.AreEqual(IntervalQualities.Perfect, IntervalQualities.Perfect.Inversion());
        Assert.AreEqual(IntervalQualities.Minor, IntervalQualities.Major.Inversion());

        var randomDegree = Random.Next(1, 10);
        var randomDiminished = NonPerfectableIntervalQuality.Diminished(randomDegree);
        var randomAugmented = NonPerfectableIntervalQuality.Augmented(randomDegree);
        Assert.AreEqual(randomDiminished, randomAugmented.Inversion());
        Assert.AreEqual(randomAugmented, randomDiminished.Inversion());
    }
    #endregion
}
