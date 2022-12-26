// See https://aka.ms/new-console-template for more information

using Rem.Music;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

var types = new[]
{
    LibraryTypeInfo.Create<ImperfectableIntervalQuality>(),
    LibraryTypeInfo.Create<PerfectableIntervalQuality>(),
    LibraryTypeInfo.Create<IntervalQuality>(),
    
    LibraryTypeInfo.Create<PerfectableSimpleIntervalNumber>(),
    LibraryTypeInfo.Create<ImperfectableSimpleIntervalNumber>(),
    LibraryTypeInfo.Create<SimpleIntervalNumber>(),

    LibraryTypeInfo.Create<SimpleIntervalBase>(),

    LibraryTypeInfo.Create<Interval>(),
    LibraryTypeInfo.Create<SignedInterval>(),

    LibraryTypeInfo.Create<NoteLetter>(),
    LibraryTypeInfo.Create<NoteSpelling>(),
    LibraryTypeInfo.Create<Note>(),

    LibraryTypeInfo.Create<NotePitchClass>(),
    LibraryTypeInfo.Create<NotePitch>(),
}.ToImmutableArray();

foreach (var typeInfo in types)
{
    Console.WriteLine($"Size of type {typeInfo.Type}: {typeInfo.Size}");
}

/// <summary>
/// A class containing info for <see cref="Rem.Music"/> library types.
/// </summary>
sealed class LibraryTypeInfo
{
    /// <summary>
    /// Gets the type this instance contains info for.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the size of instances of type <see cref="Type"/>.
    /// </summary>
    public int Size { get; }

    private LibraryTypeInfo(Type type, int size)
    {
        Type = type;
        Size = size;
    }

    /// <summary>
    /// Creates a new <see cref="LibraryTypeInfo"/> containing info for type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static LibraryTypeInfo Create<T>() => new(typeof(T), Unsafe.SizeOf<T>());
}
