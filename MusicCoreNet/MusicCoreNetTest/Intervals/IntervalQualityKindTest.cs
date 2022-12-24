using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Music;

/// <summary>
/// Tests of interval quality kind structs.
/// </summary>
[TestClass]
public class IntervalQualityKindTest
{
    /// <summary>
    /// Tests conversions between various types.
    /// </summary>
    [TestMethod]
    public void TestConversions()
    {
        foreach (var (kind, perfectable, imperfectable, central, peripheral) in ConversionTestCases)
        {
            #region Perfectable
            {
                if (perfectable is PerfectableIntervalQualityKind perfectableValue)
                {
                    Assert.IsTrue(kind.IsPerfectable(out var otherPerfectable), kind.ToString());
                    Assert.AreEqual(perfectableValue, otherPerfectable, kind.ToString());
                    Assert.IsTrue(kind.IsPerfectable(out otherPerfectable, out _), kind.ToString());
                    Assert.AreEqual(perfectableValue, otherPerfectable, kind.ToString());
                    Assert.AreEqual(perfectableValue, (PerfectableIntervalQualityKind)kind, kind.ToString());

                    #region Imperfectable Also
                    if (imperfectable is ImperfectableIntervalQualityKind imperfectableValue)
                    {
                        Assert.IsTrue(perfectableValue.IsImperfectable(out var otherImperfectable), kind.ToString());
                        Assert.AreEqual(imperfectableValue, otherImperfectable, kind.ToString());
                        Assert.AreEqual(imperfectableValue, (ImperfectableIntervalQualityKind)perfectableValue,
                                        kind.ToString());

                        Assert.IsTrue(imperfectableValue.IsPerfectable(out otherPerfectable), kind.ToString());
                        Assert.AreEqual(perfectableValue, otherPerfectable, kind.ToString());
                        Assert.AreEqual(perfectableValue, (PerfectableIntervalQualityKind)imperfectableValue,
                                        kind.ToString());
                    }
                    else
                    {
                        Assert.IsFalse(perfectableValue.IsImperfectable(out _));
                        Assert.ThrowsException<InvalidCastException>(
                                () => (ImperfectableIntervalQualityKind)perfectableValue, kind.ToString());
                    }
                    #endregion

                    #region Central Also
                    if (central is CentralIntervalQualityKind centralValue)
                    {
                        Assert.IsTrue(perfectableValue.IsCentral(out var otherCentral));
                        Assert.AreEqual(centralValue, otherCentral);
                        Assert.IsTrue(perfectableValue.IsCentral(out otherCentral, out _));
                        Assert.AreEqual(centralValue, otherCentral);
                        Assert.AreEqual(centralValue, (CentralIntervalQualityKind)perfectableValue);

                        Assert.IsTrue(centralValue.IsPerfectable(out otherPerfectable));
                        Assert.AreEqual(perfectableValue, otherPerfectable);
                        Assert.IsTrue(centralValue.IsPerfectable(out otherPerfectable, out _));
                        Assert.AreEqual(perfectableValue, otherPerfectable);
                        Assert.AreEqual(perfectableValue, (PerfectableIntervalQualityKind)centralValue);
                    }
                    else
                    {
                        Assert.IsFalse(perfectableValue.IsCentral(out _));
                        Assert.IsFalse(perfectableValue.IsCentral(out _, out var otherPeripheral));
                        Assert.AreEqual(peripheral, otherPeripheral);
                        Assert.ThrowsException<InvalidCastException>(
                                () => (CentralIntervalQualityKind)perfectableValue);
                    }
                    #endregion

                    #region Peripheral Also
                    if (peripheral is PeripheralIntervalQualityKind peripheralValue)
                    {
                        Assert.IsTrue(perfectableValue.IsPeripheral(out var otherPeripheral));
                        Assert.AreEqual(peripheralValue, otherPeripheral);
                        Assert.IsTrue(perfectableValue.IsPeripheral(out otherPeripheral, out _));
                        Assert.AreEqual(peripheralValue, otherPeripheral);
                        Assert.AreEqual(peripheralValue, (PeripheralIntervalQualityKind)perfectableValue);

                        Assert.AreEqual(perfectableValue, (PerfectableIntervalQualityKind)peripheralValue);
                    }
                    else
                    {
                        Assert.IsFalse(perfectableValue.IsPeripheral(out _));
                        Assert.IsFalse(perfectableValue.IsPeripheral(out _, out var otherCentral));
                        Assert.AreEqual(central, otherCentral);
                        Assert.ThrowsException<InvalidCastException>(
                            () => (PeripheralIntervalQualityKind)perfectableValue);
                    }
                    #endregion
                }
                else
                {
                    Assert.IsFalse(kind.IsPerfectable(out _));
                    Assert.IsFalse(kind.IsPerfectable(out _, out var otherImperfectable));
                    Assert.AreEqual(imperfectable, otherImperfectable);
                    Assert.ThrowsException<InvalidCastException>(() => (PerfectableIntervalQualityKind)kind);
                }
            }
            #endregion

            #region Imperfectable
            {
                if (imperfectable is ImperfectableIntervalQualityKind imperfectableValue)
                {
                    Assert.IsTrue(kind.IsImperfectable(out var otherImperfectable), kind.ToString());
                    Assert.AreEqual(imperfectableValue, otherImperfectable, kind.ToString());
                    Assert.IsTrue(kind.IsImperfectable(out otherImperfectable, out _), kind.ToString());
                    Assert.AreEqual(imperfectableValue, otherImperfectable, kind.ToString());
                    Assert.AreEqual(imperfectableValue, (ImperfectableIntervalQualityKind)kind, kind.ToString());

                    #region Central Also
                    if (central is CentralIntervalQualityKind centralValue)
                    {
                        Assert.IsTrue(imperfectableValue.IsCentral(out var otherCentral), kind.ToString());
                        Assert.AreEqual(centralValue, otherCentral, kind.ToString());
                        Assert.IsTrue(imperfectableValue.IsCentral(out otherCentral, out _), kind.ToString());
                        Assert.AreEqual(centralValue, otherCentral, kind.ToString());
                        Assert.AreEqual(centralValue, (CentralIntervalQualityKind)imperfectableValue,
                                        kind.ToString());

                        Assert.IsTrue(centralValue.IsImperfectable(out otherImperfectable), kind.ToString());
                        Assert.AreEqual(imperfectableValue, otherImperfectable, kind.ToString());
                        Assert.IsTrue(centralValue.IsImperfectable(out otherImperfectable, out _), kind.ToString());
                        Assert.AreEqual(imperfectableValue, otherImperfectable, kind.ToString());
                        Assert.AreEqual(imperfectableValue, (ImperfectableIntervalQualityKind)centralValue,
                                        kind.ToString());
                    }
                    else
                    {
                        Assert.IsFalse(imperfectableValue.IsCentral(out _), kind.ToString());
                        Assert.IsFalse(imperfectableValue.IsCentral(out _, out var otherPeripheral), kind.ToString());
                        Assert.AreEqual(peripheral, otherPeripheral, kind.ToString());
                        Assert.ThrowsException<InvalidCastException>(
                            () => (CentralIntervalQualityKind)imperfectableValue, kind.ToString());
                    }
                    #endregion

                    #region Peripheral Also
                    if (peripheral is PeripheralIntervalQualityKind peripheralValue)
                    {
                        Assert.IsTrue(imperfectableValue.IsPeripheral(out var otherPeripheral), kind.ToString());
                        Assert.AreEqual(peripheralValue, otherPeripheral);
                        Assert.IsTrue(imperfectableValue.IsPeripheral(out otherPeripheral, out _), kind.ToString());
                        Assert.AreEqual(peripheralValue, otherPeripheral);
                        Assert.AreEqual(peripheralValue, (PeripheralIntervalQualityKind)imperfectableValue,
                                        kind.ToString());

                        Assert.AreEqual(imperfectableValue, (ImperfectableIntervalQualityKind)peripheralValue,
                                        kind.ToString());
                    }
                    else
                    {
                        Assert.IsFalse(imperfectableValue.IsPeripheral(out _), kind.ToString());
                        Assert.IsFalse(imperfectableValue.IsPeripheral(out _, out var otherCentral), kind.ToString());
                        Assert.AreEqual(central, otherCentral, kind.ToString());
                        Assert.ThrowsException<InvalidCastException>(
                            () => (PeripheralIntervalQualityKind)imperfectableValue, kind.ToString());
                    }
                    #endregion
                }
                else
                {
                    Assert.IsFalse(kind.IsImperfectable(out _), kind.ToString());
                    Assert.IsFalse(kind.IsImperfectable(out _, out var otherPerfectable), kind.ToString());
                    Assert.AreEqual(perfectable, otherPerfectable, kind.ToString());
                    Assert.ThrowsException<InvalidCastException>(() => (ImperfectableIntervalQualityKind)kind,
                                                                 kind.ToString());
                }
            }
            #endregion

            #region Central / Peripheral
            {
                if (central is CentralIntervalQualityKind centralValue)
                {
                    Assert.IsTrue(kind.IsCentral(out var otherCentral), kind.ToString());
                    Assert.AreEqual(centralValue, otherCentral, kind.ToString());
                    Assert.IsTrue(kind.IsCentral(out otherCentral, out _), kind.ToString());
                    Assert.AreEqual(centralValue, otherCentral, kind.ToString());
                    Assert.AreEqual(centralValue, (CentralIntervalQualityKind)kind, kind.ToString());

                    // All central values are not peripheral
                    Assert.IsFalse(kind.IsPeripheral(out var _), kind.ToString());
                    Assert.IsFalse(kind.IsPeripheral(out var _, out otherCentral), kind.ToString());
                    Assert.AreEqual(centralValue, otherCentral, kind.ToString());
                    Assert.ThrowsException<InvalidCastException>(() => (PeripheralIntervalQualityKind)kind,
                                                                 kind.ToString());
                }
                else // Peripheral
                {
                    Assert.IsFalse(kind.IsCentral(out var _), kind.ToString());
                    Assert.IsFalse(kind.IsCentral(out var _, out var otherPeripheral), kind.ToString());
                    Assert.AreEqual(peripheral, otherPeripheral, kind.ToString());
                    Assert.ThrowsException<InvalidCastException>(() => (CentralIntervalQualityKind)kind,
                                                                 kind.ToString());

                    // All peripheral values are not central
                    Assert.IsTrue(kind.IsPeripheral(out otherPeripheral), kind.ToString());
                    Assert.AreEqual(peripheral, otherPeripheral, kind.ToString());
                    Assert.IsTrue(kind.IsPeripheral(out otherPeripheral, out _), kind.ToString());
                    Assert.AreEqual(peripheral, otherPeripheral, kind.ToString());
                    Assert.AreEqual(peripheral, (PeripheralIntervalQualityKind)kind, kind.ToString());
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// A series of conversion test cases, one for each value of the <see cref="IntervalQualityKind"/> type.
    /// </summary>
    private static readonly ImmutableArray<ConversionTestCase> ConversionTestCases
        = ImmutableArray.CreateRange(new ConversionTestCase[]
        {
            new(Kind: IntervalQualityKind.Diminished,
                PerfectableKind: PerfectableIntervalQualityKind.Diminished,
                ImperfectableKind: ImperfectableIntervalQualityKind.Diminished,
                CentralKind: null,
                PeripheralKind: PeripheralIntervalQualityKind.Diminished),

            new(Kind: IntervalQualityKind.Minor,
                PerfectableKind: null,
                ImperfectableKind: ImperfectableIntervalQualityKind.Minor,
                CentralKind: CentralIntervalQualityKind.Minor,
                PeripheralKind: null),

            new(Kind: IntervalQualityKind.Perfect,
                PerfectableKind: PerfectableIntervalQualityKind.Perfect,
                ImperfectableKind: null,
                CentralKind: CentralIntervalQualityKind.Perfect,
                PeripheralKind: null),

            new(Kind: IntervalQualityKind.Major,
                PerfectableKind: null,
                ImperfectableKind: ImperfectableIntervalQualityKind.Major,
                CentralKind: CentralIntervalQualityKind.Major,
                PeripheralKind: null),

            new(Kind: IntervalQualityKind.Augmented,
                PerfectableKind: PerfectableIntervalQualityKind.Augmented,
                ImperfectableKind: ImperfectableIntervalQualityKind.Augmented,
                CentralKind: null,
                PeripheralKind: PeripheralIntervalQualityKind.Augmented),
        });

    /// <summary>
    /// Represents a test of the conversions between different interval quality kind values.
    /// </summary>
    /// <param name="Kind"></param>
    /// <param name="PerfectableKind"></param>
    /// <param name="ImperfectableKind"></param>
    /// <param name="CentralKind"></param>
    /// <param name="PeripheralKind"></param>
    private sealed record class ConversionTestCase(
        IntervalQualityKind Kind,
        PerfectableIntervalQualityKind? PerfectableKind,
        ImperfectableIntervalQualityKind? ImperfectableKind,
        CentralIntervalQualityKind? CentralKind,
        PeripheralIntervalQualityKind? PeripheralKind);
}
