using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of the <see cref="IntervalQuality"/>, <see cref="PerfectableIntervalQuality"/> and
/// <see cref="ImperfectableIntervalQuality"/> structs.
/// </summary>
[TestClass]
public class IntervalQualityTest
{
    #region Properties And Fields
    private static readonly Random Random = new();
    #endregion

    #region General
    /// <summary>
    /// Ensures that the default values are as advertised in the doc comments.
    /// </summary>
    [TestMethod]
    public void TestDefaults()
    {
        Assert.AreEqual(IntervalQuality.Perfect, default);
        Assert.AreEqual(PerfectableIntervalQuality.Perfect, default);
        Assert.AreEqual(ImperfectableIntervalQuality.Major, default);
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
                $"Invalid {nameof(Quality.CircleOfFifthsIndex)} cast from"
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
                $"Invalid {nameof(IntervalQuality.FromCircleOfFifthsIndex)} cast from index {Index}.");
        }
    }

    /// <summary>
    /// Tests methods for constructing an interval from a predefined quality and a supplied interval number.
    /// </summary>
    [TestMethod]
    public void TestWithNumber()
    {
        // All should pass
        // The augmented and diminished should work no matter what the perfectability of the number passed in is, as
        // mismatches between perfectabilities is not possible
        Assert.AreEqual(Interval.Augmented().Third, Interval.Augmented().WithNumber(3));
        Assert.AreEqual(Interval.Augmented().Fourth, Interval.Augmented().WithNumber(4));
        Assert.AreEqual(Interval.Major.Third, Interval.Major.WithNumber(3));
        Assert.AreEqual(Interval.Minor.Third, Interval.Minor.WithNumber(3));
        Assert.AreEqual(Interval.Perfect.Unison, Interval.Perfect.WithNumber(1));
        Assert.AreEqual(Interval.Diminished().Third, Interval.Diminished().WithNumber(3));
        Assert.AreEqual(Interval.Diminished().Fourth, Interval.Diminished().WithNumber(4));

        // These should fail due to mismatches
        Assert.ThrowsException<ArgumentException>(() => Interval.Major.WithNumber(4));
        Assert.ThrowsException<ArgumentException>(() => Interval.Perfect.WithNumber(3));
        Assert.ThrowsException<ArgumentException>(() => Interval.Minor.WithNumber(4));

        // These should fail due to the number being 0 or negative
        Assert.ThrowsException<InvalidCastException>(() => Interval.Major.WithNumber(0));
        Assert.ThrowsException<InvalidCastException>(() => Interval.Major.WithNumber(-1));
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
        Assert.AreEqual(IntervalQuality.Major, IntervalQuality.Minor.Inversion);
        Assert.AreEqual(IntervalQuality.Perfect, IntervalQuality.Perfect.Inversion);
        Assert.AreEqual(IntervalQuality.Minor, IntervalQuality.Major.Inversion);

        var randomDegree = Random.Next(1, 10);
        var randomDiminished = ImperfectableIntervalQuality.Diminished(randomDegree);
        var randomAugmented = ImperfectableIntervalQuality.Augmented(randomDegree);
        Assert.AreEqual(randomDiminished, randomAugmented.Inversion);
        Assert.AreEqual(randomAugmented, randomDiminished.Inversion);
    }
    #endregion

    #region Conversion
    /// <summary>
    /// Tests conversion between various interval quality types.
    /// </summary>
    [TestMethod]
    public void TestConversion()
    {
        foreach (var (quality,
                      perfectableOrNull, imperfectableOrNull,
                      centralOrNull, peripheralOrNull)
                 in ConversionTests)
        {
            #region Perfectable
            {
                if (perfectableOrNull is PerfectableIntervalQuality perfectable)
                {
                    AssertValidCast(quality, q => (PerfectableIntervalQuality)q, perfectable);
                    AssertIs(quality, quality.IsPerfectable, perfectable);
                    AssertIs<IntervalQuality, PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                        quality, quality.IsPerfectable, perfectable);

                    if (imperfectableOrNull is ImperfectableIntervalQuality imperfectable)
                    {
                        AssertValidCast(perfectable, p => (ImperfectableIntervalQuality)p, imperfectable);
                        AssertValidCast(imperfectable, i => (PerfectableIntervalQuality)i, perfectable);

                        AssertIs(perfectable, perfectable.IsImperfectable, imperfectable);
                        AssertIs(imperfectable, imperfectable.IsPerfectable, perfectable);
                    }
                    else
                    {
                        AssertInvalidCast(perfectable, p => (ImperfectableIntervalQuality)p);

                        AssertIsNot<PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                            perfectable, perfectable.IsImperfectable);
                    }

                    if (centralOrNull is CentralIntervalQuality central)
                    {
                        AssertValidCast(perfectable, p => (CentralIntervalQuality)p, central);
                        AssertValidCast(central, i => (PerfectableIntervalQuality)i, perfectable);

                        AssertIs(perfectable, perfectable.IsCentral, central);
                        AssertIs<PerfectableIntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                            perfectable, perfectable.IsCentral, central);

                        AssertIs(central, central.IsPerfectable, perfectable);
                        AssertIs<CentralIntervalQuality, PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                            central, central.IsPerfectable, perfectable);
                    }
                    else
                    {
                        AssertInvalidCast(perfectable, p => (CentralIntervalQuality)p);

                        AssertIsNot<PerfectableIntervalQuality, CentralIntervalQuality>(
                            perfectable, perfectable.IsCentral);

                        AssertIsNot<PerfectableIntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                            perfectable, perfectable.IsCentral,
                            (PeripheralIntervalQuality)peripheralOrNull!); // Test is malformed if null
                    }

                    if (peripheralOrNull is PeripheralIntervalQuality peripheral)
                    {
                        AssertValidCast(perfectable, p => (PeripheralIntervalQuality)p, peripheral);
                        AssertValidCast(peripheral, i => (PerfectableIntervalQuality)i, perfectable);

                        AssertIs(perfectable, perfectable.IsPeripheral, peripheral);
                        AssertIs<PerfectableIntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                            perfectable, perfectable.IsPeripheral, peripheral);
                    }
                    else
                    {
                        AssertInvalidCast(perfectable, p => (PeripheralIntervalQuality)p);

                        AssertIsNot<PerfectableIntervalQuality, PeripheralIntervalQuality>(
                            perfectable, perfectable.IsPeripheral);

                        AssertIsNot<PerfectableIntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                            perfectable, perfectable.IsPeripheral,
                            (CentralIntervalQuality)centralOrNull!); // Test is malformed if null
                    }
                }
                else
                {
                    // Must be imperfectable or the test is malformed
                    var imperfectable = (ImperfectableIntervalQuality)imperfectableOrNull!;

                    // Ensure failed casts from the general quality
                    AssertInvalidCast(quality, q => (PerfectableIntervalQuality)q);

                    AssertIsNot<IntervalQuality, PerfectableIntervalQuality>(quality, quality.IsPerfectable);

                    AssertIsNot<IntervalQuality, PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                        quality, quality.IsPerfectable, imperfectable);

                    // Ensure failed casts from the imperfectable quality
                    AssertInvalidCast(imperfectable, i => (PerfectableIntervalQuality)i);

                    AssertIsNot<ImperfectableIntervalQuality, PerfectableIntervalQuality>(
                        imperfectable, imperfectable.IsPerfectable);

                    if (centralOrNull is CentralIntervalQuality central)
                    {
                        AssertInvalidCast(central, c => (PerfectableIntervalQuality)c);

                        AssertIsNot<CentralIntervalQuality, PerfectableIntervalQuality>(
                            central, central.IsPerfectable);

                        AssertIsNot<CentralIntervalQuality, PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                            central, central.IsPerfectable, imperfectable);
                    }

                    // Peripheral interval qualities are always perfectable, so is not peripheral or the test
                    // is malformed
                }
            }
            #endregion

            #region Imperfectable
            {
                if (imperfectableOrNull is ImperfectableIntervalQuality imperfectable)
                {
                    AssertValidCast(quality, q => (ImperfectableIntervalQuality)q, imperfectable);
                    AssertIs(quality, quality.IsImperfectable, imperfectable);
                    AssertIs<IntervalQuality, ImperfectableIntervalQuality, PerfectableIntervalQuality>(
                        quality, quality.IsImperfectable, imperfectable);

                    if (centralOrNull is CentralIntervalQuality central)
                    {
                        AssertValidCast(imperfectable, p => (CentralIntervalQuality)p, central);
                        AssertValidCast(central, i => (ImperfectableIntervalQuality)i, imperfectable);

                        AssertIs(imperfectable, imperfectable.IsCentral, central);
                        AssertIs<ImperfectableIntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                            imperfectable, imperfectable.IsCentral, central);

                        AssertIs(central, central.IsImperfectable, imperfectable);
                        AssertIs<CentralIntervalQuality, ImperfectableIntervalQuality, PerfectableIntervalQuality>(
                            central, central.IsImperfectable, imperfectable);
                    }
                    else
                    {
                        AssertInvalidCast(imperfectable, p => (CentralIntervalQuality)p);

                        AssertIsNot<ImperfectableIntervalQuality, CentralIntervalQuality>(
                            imperfectable, imperfectable.IsCentral);

                        AssertIsNot<ImperfectableIntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                            imperfectable, imperfectable.IsCentral,
                            (PeripheralIntervalQuality)peripheralOrNull!); // Test is malformed if null
                    }

                    if (peripheralOrNull is PeripheralIntervalQuality peripheral)
                    {
                        AssertValidCast(imperfectable, p => (PeripheralIntervalQuality)p, peripheral);
                        AssertValidCast(peripheral, i => (ImperfectableIntervalQuality)i, imperfectable);

                        AssertIs(imperfectable, imperfectable.IsPeripheral, peripheral);
                        AssertIs<ImperfectableIntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                            imperfectable, imperfectable.IsPeripheral, peripheral);
                    }
                    else
                    {
                        AssertInvalidCast(imperfectable, p => (PeripheralIntervalQuality)p);

                        AssertIsNot<ImperfectableIntervalQuality, PeripheralIntervalQuality>(
                            imperfectable, imperfectable.IsPeripheral);

                        AssertIsNot<ImperfectableIntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                            imperfectable, imperfectable.IsPeripheral,
                            (CentralIntervalQuality)centralOrNull!); // Test is malformed if null
                    }
                }
                else
                {
                    // Must be perfectable or the test is malformed
                    var perfectable = (PerfectableIntervalQuality)perfectableOrNull!;

                    // Ensure failed conversions from the general quality
                    AssertInvalidCast(quality, q => (ImperfectableIntervalQuality)q);

                    AssertIsNot<IntervalQuality, ImperfectableIntervalQuality>(quality, quality.IsImperfectable);

                    AssertIsNot<IntervalQuality, ImperfectableIntervalQuality, PerfectableIntervalQuality>(
                        quality, quality.IsImperfectable, perfectable);

                    // Ensure failed conversions from perfectable quality
                    AssertInvalidCast(perfectable, i => (ImperfectableIntervalQuality)i);

                    AssertIsNot<PerfectableIntervalQuality, ImperfectableIntervalQuality>(
                        perfectable, perfectable.IsImperfectable);

                    if (centralOrNull is CentralIntervalQuality central)
                    {
                        AssertInvalidCast(central, c => (ImperfectableIntervalQuality)c);

                        AssertIsNot<CentralIntervalQuality, ImperfectableIntervalQuality>(
                            central, central.IsImperfectable);

                        AssertIsNot<CentralIntervalQuality, ImperfectableIntervalQuality, PerfectableIntervalQuality>(
                            central, central.IsImperfectable, perfectable);
                    }

                    // Peripheral interval qualities are always imperfectable, so is not peripheral or the test
                    // is malformed
                }
            }
            #endregion

            #region Central / Peripheral
            {
                if (centralOrNull is CentralIntervalQuality central)
                {
                    // Ensure valid conversions to central
                    AssertValidCast(quality, q => (CentralIntervalQuality)q, central);
                    AssertIs(quality, quality.IsCentral, central);
                    AssertIs<IntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                        quality, quality.IsCentral, central);

                    // Ensure invalid conversions to peripheral
                    AssertInvalidCast(quality, q => (PeripheralIntervalQuality)q);

                    AssertIsNot<IntervalQuality, PeripheralIntervalQuality>(quality, quality.IsPeripheral);

                    AssertIsNot<IntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                        quality, quality.IsPeripheral, central);
                }
                else
                {
                    // Must be peripheral or the test is malformed
                    var peripheral = (PeripheralIntervalQuality)peripheralOrNull!;

                    // Ensure valid conversions to peripheral
                    AssertValidCast(quality, q => (PeripheralIntervalQuality)q, peripheral);
                    AssertIs(quality, quality.IsPeripheral, peripheral);
                    AssertIs<IntervalQuality, PeripheralIntervalQuality, CentralIntervalQuality>(
                        quality, quality.IsPeripheral, peripheral);

                    // Ensure invalid conversions to central
                    AssertInvalidCast(quality, q => (CentralIntervalQuality)q);

                    AssertIsNot<IntervalQuality, CentralIntervalQuality>(quality, quality.IsCentral);

                    AssertIsNot<IntervalQuality, CentralIntervalQuality, PeripheralIntervalQuality>(
                        quality, quality.IsCentral, peripheral);
                }
            }
            #endregion
        }
    }

    private static readonly ImmutableArray<ConversionTest> ConversionTests
        = ImmutableArray.CreateRange(new ConversionTest[]
        {
            ConversionTest.Diminished(3),
            ConversionTest.Diminished(2),
            ConversionTest.Diminished(),

            new(Quality: IntervalQuality.Minor,
                Perfectable: null, Imperfectable: ImperfectableIntervalQuality.Minor,
                Central: CentralIntervalQuality.Minor, Peripheral: null),

            new(Quality: IntervalQuality.Perfect,
                Perfectable: PerfectableIntervalQuality.Perfect, Imperfectable: null,
                Central: CentralIntervalQuality.Perfect, Peripheral: null),

            new(Quality: IntervalQuality.Major,
                Perfectable: null, Imperfectable: ImperfectableIntervalQuality.Major,
                Central: CentralIntervalQuality.Major, Peripheral: null),

            ConversionTest.Augmented(),
            ConversionTest.Augmented(2),
            ConversionTest.Augmented(3),
        });

    #region Helpers
    private static void AssertIs<TValue, TExpected>(
        TValue value, IsConversion<TExpected> conversion, TExpected expected)
    {
        Assert.IsTrue(conversion(out var actual), NoConversionMsg(typeof(TExpected), value));
        Assert.AreEqual(expected, actual, ConversionMismatchMsg(value, expected, actual));
    }

    private static void AssertIs<TValue, TExpected, TNotExpected>(
        TValue value, IsConversion<TExpected, TNotExpected> conversion, TExpected expected)
    {
        Assert.IsTrue(conversion(out var actual, out _), NoConversionMsg(typeof(TExpected), value));
        Assert.AreEqual(expected, actual, ConversionMismatchMsg(value, expected, actual));
    }

    private static void AssertValidCast<TValue, TExpected>(
        TValue value, Func<TValue, TExpected> cast, TExpected expected)
    {
        try
        {
            var actual = cast(value);
            Assert.AreEqual(expected, actual, ConversionMismatchMsg(value, expected, actual));
        }
        catch (InvalidCastException)
        {
            Assert.Fail(NoConversionMsg(typeof(TExpected), value));
            throw null!; // Should never happen
        }
    }

    private static string NoConversionMsg<TQuality>(Type expectedType, TQuality actual)
        => $"{typeof(TQuality)} value {actual} could not be converted to {expectedType}.";

    private static InvalidCastException AssertInvalidCast<TValue, TNotExpected>(
        TValue value, Func<TValue, TNotExpected> conversion)
        => Assert.ThrowsException<InvalidCastException>(() => conversion(value),
                                                        ConversionExistedMsg(typeof(TNotExpected), value));
    private static void AssertIsNot<TValue, TNotExpected>(TValue value, IsConversion<TNotExpected> conversion)
        => Assert.IsFalse(conversion(out _), ConversionExistedMsg(typeof(TNotExpected), value));

    private static void AssertIsNot<TValue, TNotExpected, TExpected>(
        TValue value, IsConversion<TNotExpected, TExpected> conversion, TExpected expected)
    {
        var errMsg = ConversionExistedMsg(typeof(TNotExpected), value);
        Assert.IsFalse(conversion(out _, out var actual), ConversionExistedMsg(typeof(TNotExpected), value));
        Assert.AreEqual(expected, actual, ConversionMismatchMsg(value, expected, actual));
    }

    private static string ConversionExistedMsg<TValue>(Type notExpectedType, TValue value)
        => $"{typeof(TValue)} value {value} was unexpectedly convertible to {notExpectedType}.";

    private static string ConversionMismatchMsg<TInitial, TConverted>(
        TInitial initial, TConverted expected, TConverted actual)
        => $"{typeof(TInitial)} value {initial} conversion to {typeof(TConverted)} was expected to yield " +
            $"{expected}, but yielded {actual}.";
    #endregion

    #region Types
    /// <summary>
    /// A test case for the conversion testing.
    /// </summary>
    /// <param name="Quality"></param>
    /// <param name="Perfectable"></param>
    /// <param name="Imperfectable"></param>
    /// <param name="Central"></param>
    /// <param name="Peripheral"></param>
    private sealed record class ConversionTest(
        IntervalQuality Quality,
        PerfectableIntervalQuality? Perfectable, ImperfectableIntervalQuality? Imperfectable,
        CentralIntervalQuality? Central, PeripheralIntervalQuality? Peripheral)
    {
        public static ConversionTest Diminished(int Degree = 1)
            => new(Quality: IntervalQuality.Diminished(Degree),
                   Perfectable: PerfectableIntervalQuality.Diminished(Degree),
                   Imperfectable: ImperfectableIntervalQuality.Diminished(Degree),
                   Central: null,
                   Peripheral: PeripheralIntervalQuality.Diminished(Degree));

        public static ConversionTest Augmented(int Degree = 1)
            => new(Quality: IntervalQuality.Augmented(Degree),
                   Perfectable: PerfectableIntervalQuality.Augmented(Degree),
                   Imperfectable: ImperfectableIntervalQuality.Augmented(Degree),
                   Central: null,
                   Peripheral: PeripheralIntervalQuality.Augmented(Degree));
    }

    private delegate bool IsConversion<TResult>(out TResult converted);
    private delegate bool IsConversion<TResult, TComplement>(out TResult converted, out TComplement notConverted);
    #endregion
    #endregion
}
