using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Music;

/// <summary>
/// Offers methods to simplify augmented interval construction.
/// </summary>
public readonly struct AugmentedIntervalBuilder
{
    #region Properties
    /// <summary>
    /// The degree of the augmented interval to construct.
    /// </summary>
    private int Degree { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="AugmentedIntervalBuilder"/> struct with the integer degree
    /// passed in.
    /// </summary>
    /// <param name="Degree"></param>
    internal AugmentedIntervalBuilder(int Degree) { this.Degree = Degree; }
    #endregion

    #region Builder Methods
    #region Perfectable
    /// <summary>
    /// Creates a fourth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Augmented(Degree), PerfectableSimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Augmented(Degree), PerfectableSimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Augmented(Degree), PerfectableSimpleIntervalNumber.Fifth);
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Creates a second with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), NonPerfectableSimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), NonPerfectableSimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), NonPerfectableSimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), NonPerfectableSimpleIntervalNumber.Seventh);
    #endregion
    #endregion
}

/// <summary>
/// Offers methods to simplify diminished interval construction.
/// </summary>
public readonly struct DiminishedIntervalBuilder
{
    #region Properties
    /// <summary>
    /// The degree of the diminished interval to construct.
    /// </summary>
    private int Degree { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="DiminishedIntervalBuilder"/> struct with the integer degree
    /// passed in.
    /// </summary>
    /// <param name="Degree"></param>
    internal DiminishedIntervalBuilder(int Degree) { this.Degree = Degree; }
    #endregion

    #region Builder Methods
    #region Perfectable
    /// <summary>
    /// Creates a fourth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Diminished(Degree), PerfectableSimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Diminished(Degree), PerfectableSimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Diminished(Degree), PerfectableSimpleIntervalNumber.Fifth);
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Creates a second with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), NonPerfectableSimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), NonPerfectableSimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), NonPerfectableSimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), NonPerfectableSimpleIntervalNumber.Seventh);
    #endregion
    #endregion
}

/// <summary>
/// Offers methods to simplify major interval construction.
/// </summary>
public readonly struct MajorIntervalBuilder
{
    /// <summary>
    /// Creates a major second.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Major, NonPerfectableSimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a major third.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Major, NonPerfectableSimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a major sixth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Major, NonPerfectableSimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a major seventh.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Major, NonPerfectableSimpleIntervalNumber.Seventh);
}

/// <summary>
/// Offers methods to simplify minor interval construction.
/// </summary>
public readonly struct MinorIntervalBuilder
{
    /// <summary>
    /// Creates a minor second.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Minor, NonPerfectableSimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a minor third.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Minor, NonPerfectableSimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a minor sixth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Minor, NonPerfectableSimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a minor seventh.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NonPerfectableSimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Minor, NonPerfectableSimpleIntervalNumber.Seventh);
}

/// <summary>
/// Offers methods to simplify perfect interval construction.
/// </summary>
public readonly struct PerfectIntervalBuilder
{
    /// <summary>
    /// Creates a perfect fourth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Perfect, PerfectableSimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a perfect unison.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Perfect, PerfectableSimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a perfect fifth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PerfectableSimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Perfect, PerfectableSimpleIntervalNumber.Fifth);
}

