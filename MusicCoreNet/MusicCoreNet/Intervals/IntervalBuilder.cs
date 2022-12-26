using Rem.Core.Attributes;
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
    #region SimpleIntervalBase
    #region Perfectable
    /// <summary>
    /// Creates a fourth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fourth()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Unison()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fifth()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Fifth);
    #endregion

    #region Imperfectable
    /// <summary>
    /// Creates a second with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Second()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Sixth()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Third()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the augmented quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Seventh()
        => new(IntervalQuality.Augmented(Degree), SimpleIntervalNumber.Seventh);
    #endregion
    #endregion

    #region Interval
    /// <summary>
    /// Creates an interval with the augmented quality represented by this instance and the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Number"/> was not positive.</exception>
    public Interval WithNumber([Positive] int Number)
        => WithNumber(new(Throw.IfArgNotPositive(Number, nameof(Number))));

    /// <summary>
    /// Creates an interval with the augmented quality represented by this instance and the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public Interval WithNumber(IntervalNumber Number)
        => new(new(IntervalQuality.Augmented(Degree), Number.Base), Number.AdditionalOctaves);
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
    #region SimpleIntervalBase
    #region Perfectable
    /// <summary>
    /// Creates a fourth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fourth()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a unison with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Unison()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a fifth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fifth()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Fifth);
    #endregion

    #region Imperfectable
    /// <summary>
    /// Creates a second with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Second()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a sixth with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Sixth()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a third with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Third()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a seventh with the diminished quality represented by this instance.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Seventh()
        => new(IntervalQuality.Diminished(Degree), SimpleIntervalNumber.Seventh);
    #endregion
    #endregion

    #region Interval
    /// <summary>
    /// Creates an interval with the diminished quality represented by this instance and the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Number"/> was not positive.</exception>
    public Interval WithNumber([Positive] int Number)
        => WithNumber(new(Throw.IfArgNotPositive(Number, nameof(Number))));

    /// <summary>
    /// Creates an interval with the diminished quality represented by this instance and the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    public Interval WithNumber(IntervalNumber Number)
        => new(new(IntervalQuality.Diminished(Degree), Number.Base), Number.AdditionalOctaves);
    #endregion
    #endregion
}

/// <summary>
/// Offers methods to simplify major interval construction.
/// </summary>
public readonly struct MajorIntervalBuilder
{
    #region SimpleIntervalBase
    /// <summary>
    /// Creates a major second.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Second()
        => new(IntervalQuality.Major, SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a major third.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Third()
        => new(IntervalQuality.Major, SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a major sixth.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Sixth()
        => new(IntervalQuality.Major, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a major seventh.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Seventh()
        => new(IntervalQuality.Major, SimpleIntervalNumber.Seventh);
    #endregion

    #region Interval
    /// <summary>
    /// Creates a major interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not an imperfectable interval number.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Number"/> was not positive.</exception>
    public Interval WithNumber([Positive] int Number)
        => WithNumber(new(Throw.IfArgNotPositive(Number, nameof(Number))));

    /// <summary>
    /// Creates a major interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not an imperfectable interval number.
    /// </exception>
    public Interval WithNumber(IntervalNumber Number)
    {
        if (Number.Perfectability != Imperfectable) throw Interval.PerfectabilityMismatch(Perfectable);
        return new(new(IntervalQuality.Major, Number.Base), Number.AdditionalOctaves);
    }
    #endregion
}

/// <summary>
/// Offers methods to simplify minor interval construction.
/// </summary>
public readonly struct MinorIntervalBuilder
{
    #region SimpleIntervalBase
    /// <summary>
    /// Creates a minor second.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Second()
        => new(IntervalQuality.Minor, SimpleIntervalNumber.Second);

    /// <summary>
    /// Creates a minor third.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Third()
        => new(IntervalQuality.Minor, SimpleIntervalNumber.Third);

    /// <summary>
    /// Creates a minor sixth.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Sixth()
        => new(IntervalQuality.Minor, SimpleIntervalNumber.Sixth);

    /// <summary>
    /// Creates a minor seventh.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Seventh()
        => new(IntervalQuality.Minor, SimpleIntervalNumber.Seventh);
    #endregion

    #region Interval
    /// <summary>
    /// Creates a minor interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not an imperfectable interval number.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Number"/> was not positive.</exception>
    public Interval WithNumber([Positive] int Number)
        => WithNumber(new(Throw.IfArgNotPositive(Number, nameof(Number))));

    /// <summary>
    /// Creates a minor interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not an imperfectable interval number.
    /// </exception>
    public Interval WithNumber(IntervalNumber Number)
    {
        if (Number.Perfectability != Imperfectable) throw Interval.PerfectabilityMismatch(Perfectable);
        return new(new(IntervalQuality.Minor, Number.Base), Number.AdditionalOctaves);
    }
    #endregion
}

/// <summary>
/// Offers methods to simplify perfect interval construction.
/// </summary>
public readonly struct PerfectIntervalBuilder
{
    #region SimpleIntervalBase
    /// <summary>
    /// Creates a perfect fourth.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fourth()
        => new(IntervalQuality.Perfect, SimpleIntervalNumber.Fourth);

    /// <summary>
    /// Creates a perfect unison.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Unison()
        => new(IntervalQuality.Perfect, SimpleIntervalNumber.Unison);

    /// <summary>
    /// Creates a perfect fifth.
    /// </summary>
    /// <returns></returns>
    public SimpleIntervalBase Fifth()
        => new(IntervalQuality.Perfect, SimpleIntervalNumber.Fifth);
    #endregion

    #region Interval
    /// <summary>
    /// Creates a perfect interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not a perfectable interval number.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Number"/> was not positive.</exception>
    public Interval WithNumber([Positive] int Number)
        => WithNumber(new(Throw.IfArgNotPositive(Number, nameof(Number))));

    /// <summary>
    /// Creates a perfect interval with the number passed in.
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="Number"/> was not a perfectable interval number.
    /// </exception>
    public Interval WithNumber(IntervalNumber Number)
    {
        if (Number.Perfectability != Perfectable) throw Interval.PerfectabilityMismatch(Imperfectable);
        return new(new(IntervalQuality.Perfect, Number.Base), Number.AdditionalOctaves);
    }
    #endregion
}

