using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="PerfectableIntervalQuality"/> and <see cref="ImperfectableIntervalQuality"/> structs.
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
            (IntervalQuality.Diminished(2), -3),
            (IntervalQuality.Diminished(), -2),
            (IntervalQuality.Minor, -1),
            (IntervalQuality.Perfect, 0),
            (IntervalQuality.Major, 1),
            (IntervalQuality.Augmented(), 2),
            (IntervalQuality.Augmented(2), 3),
        });

    /// <summary>
    /// Tests the <see cref="IntervalQuality.CircleOfFifthsIndex"/> property.
    /// </summary>
    [TestMethod]
    public void TestPerfectBasedIndex()
    {
        foreach (var (Quality, Index) in GeneralPerfectBasedIndexPairs)
        {
            Assert.AreEqual(
                Index, Quality.CircleOfFifthsIndex,
                $"Invalid {nameof(Quality.CircleOfFifthsIndex)} result from"
                    + $" ({Quality.Perfectability}) interval quality '{Quality}'.");
        }
    }

    /// <summary>
    /// Tests the <see cref="IntervalQuality.FromCircleOfFifthsIndex(int)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestFromPerfectBasedIndex()
    {
        foreach (var (Quality, Index) in GeneralPerfectBasedIndexPairs)
        {
            Assert.AreEqual(
                Quality, IntervalQuality.FromCircleOfFifthsIndex(Index),
                $"Invalid {nameof(IntervalQuality.FromCircleOfFifthsIndex)} result from index {Index}.");
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

        Assert.IsTrue(IntervalQuality.Perfect.IsPerfect());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);

        Assert.IsFalse(IntervalQuality.Perfect.IsDiminished());
        Assert.IsFalse(IntervalQuality.Perfect.IsAugmented());
    }
    #endregion

    #region Imperfectable
    /// <summary>
    /// Tests the factory method construction and characterization of
    /// <see cref="ImperfectableIntervalQuality"/> instances.
    /// </summary>
    [TestMethod, TestCategory(nameof(ImperfectableIntervalQuality))]
    public void TestImperfectableCharacterization()
    {
        var randomDegree = Random.Next(10) + 2;

        var randomDiminished = ImperfectableIntervalQuality.Diminished(randomDegree);
        var singleDiminished = ImperfectableIntervalQuality.Diminished();
        var singleAugmented = ImperfectableIntervalQuality.Augmented();
        var randomAugmented = ImperfectableIntervalQuality.Augmented(randomDegree);

        int diminishedDegree, augmentedDegree;

        Assert.IsTrue(singleDiminished.IsDiminished());
        Assert.IsTrue(singleDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(1, diminishedDegree);

        Assert.IsTrue(randomDiminished.IsDiminished());
        Assert.IsTrue(randomDiminished.IsDiminished(out diminishedDegree));
        Assert.AreEqual(randomDegree, diminishedDegree);

        Assert.IsTrue(IntervalQuality.Minor.IsMinor());
        Assert.IsTrue(IntervalQuality.Major.IsMajor());

        Assert.IsTrue(singleAugmented.IsAugmented());
        Assert.IsTrue(singleAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(1, augmentedDegree);

        Assert.IsTrue(randomAugmented.IsAugmented());
        Assert.IsTrue(randomAugmented.IsAugmented(out augmentedDegree));
        Assert.AreEqual(randomDegree, augmentedDegree);

        Assert.IsFalse(IntervalQuality.Major.IsDiminished());
        Assert.IsFalse(IntervalQuality.Major.IsAugmented());
        Assert.IsFalse(IntervalQuality.Minor.IsDiminished());
        Assert.IsFalse(IntervalQuality.Minor.IsAugmented());
    }

    /// <summary>
    /// Tests inversion of an imperfectable interval quality.
    /// </summary>
    [TestMethod, TestCategory(nameof(ImperfectableIntervalQuality))]
    public void TestImperfectableInversion()
    {
        Assert.AreEqual(IntervalQuality.Major, IntervalQuality.Minor.Inversion());
        Assert.AreEqual(IntervalQuality.Perfect, IntervalQuality.Perfect.Inversion());
        Assert.AreEqual(IntervalQuality.Minor, IntervalQuality.Major.Inversion());

        var randomDegree = Random.Next(1, 10);
        var randomDiminished = ImperfectableIntervalQuality.Diminished(randomDegree);
        var randomAugmented = ImperfectableIntervalQuality.Augmented(randomDegree);
        Assert.AreEqual(randomDiminished, randomAugmented.Inversion());
        Assert.AreEqual(randomAugmented, randomDiminished.Inversion());
    }
    #endregion
}
