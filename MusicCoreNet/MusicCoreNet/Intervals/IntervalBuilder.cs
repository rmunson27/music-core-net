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
    public SimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Fifth);
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Creates a second with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Augmented(Degree), SimpleIntervalNumber.Seventh);
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
    public SimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Fifth);
    #endregion

    #region Non-Perfectable
    /// <summary>
    /// Creates a second with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Diminished(Degree), SimpleIntervalNumber.Seventh);
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
    public SimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Major, SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a major third.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Major, SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a major sixth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Major, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a major seventh.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Major, SimpleIntervalNumber.Seventh);
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
    public SimpleIntervalBase Second()
        => new(NonPerfectableIntervalQuality.Minor, SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a minor third.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Third()
        => new(NonPerfectableIntervalQuality.Minor, SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a minor sixth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Sixth()
        => new(NonPerfectableIntervalQuality.Minor, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a minor seventh.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Seventh()
        => new(NonPerfectableIntervalQuality.Minor, SimpleIntervalNumber.Seventh);
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
    public SimpleIntervalBase Fourth()
        => new(PerfectableIntervalQuality.Perfect, SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a perfect unison.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Unison()
        => new(PerfectableIntervalQuality.Perfect, SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a perfect fifth.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimpleIntervalBase Fifth()
        => new(PerfectableIntervalQuality.Perfect, SimpleIntervalNumber.Fifth);
}

