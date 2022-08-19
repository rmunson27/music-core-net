using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    #region General
    /// <summary>
    /// Ensures that the default value of the <see cref="IntervalQuality"/> struct is the value advertised.
    /// </summary>
    [TestMethod]
    public void TestGeneralDefault()
    {
        Assert.AreEqual(default(IntervalQuality), PerfectableIntervalQuality.Perfect);
    }

    /// <summary>
    /// Pairs of interval qualities and integers with the relationship as follows:
    /// <para/>
    /// <c>Quality.PerfectBasedIndex == Index;</c>
    /// <para/>
    /// <c>IntervalQuality.FromPerfectBasedIndex(Index) == Quality;</c>
    /// </summary>
    /// <remarks>
    /// This should be enough test cases for a pattern to be established.
    /// </remarks>
    private static readonly ImmutableArray<(IntervalQuality Quality, int Index)> GeneralPerfectBasedIndexPairs
        = ImmutableArray.CreateRange(new (IntervalQuality, int)[]
        {
            (NonPerfectableIntervalQuality.Diminished(2), -5),
            (PerfectableIntervalQuality.Diminished(2), -4),
            (NonPerfectableIntervalQuality.Diminished(), -3),
            (PerfectableIntervalQuality.Diminished(), -2),
            (NonPerfectableIntervalQuality.Minor, -1),
            (PerfectableIntervalQuality.Perfect, 0),
            (NonPerfectableIntervalQuality.Major, 1),
            (PerfectableIntervalQuality.Augmented(), 2),
            (NonPerfectableIntervalQuality.Augmented(), 3),
            (PerfectableIntervalQuality.Augmented(2), 4),
            (NonPerfectableIntervalQuality.Augmented(2), 5),
        });

    /// <summary>
    /// Tests the <see cref="IntervalQuality.PerfectBasedIndex"/> property.
    /// </summary>
    [TestMethod]
    public void TestPerfectBasedIndex()
    {
        foreach (var (Quality, Index) in GeneralPerfectBasedIndexPairs)
        {
            Assert.AreEqual(
                Index, Quality.PerfectBasedIndex,
                $"Invalid {nameof(Quality.PerfectBasedIndex)} result from"
                    + $" ({(Quality.IsPerfectable() ? "Perfectable" : "Non-Perfectable")})"
                    + $" interval quality '{Quality}'.");
        }
    }

    /// <summary>
    /// Tests the <see cref="IntervalQuality.FromPerfectBasedIndex(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromPerfectBasedIndex()
    {
        foreach (var (Quality, Index) in GeneralPerfectBasedIndexPairs)
        {
            Assert.AreEqual(
                Quality, IntervalQuality.FromPerfectBasedIndex(Index),
                $"Invalid {nameof(IntervalQuality.FromPerfectBasedIndex)} result from index {Index}.");
        }
    }
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
