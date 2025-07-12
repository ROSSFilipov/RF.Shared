namespace RF.Shared.Common.Models.V1;

using System;
using System.Collections.Concurrent;
using System.Reflection;

/// <summary>
/// Represents a base class for creating strongly-typed enumerations.
/// </summary>
/// <remarks>The <see cref="EnumerationBase"/> class provides a pattern for defining strongly-typed enumerations with
/// associated integer identifiers and string values. It is designed to be extended by derived classes that define
/// specific enumeration types. Instances of <see cref="EnumerationBase"/> are immutable and can be compared for equality or
/// ordering based on their <see cref="Id"/> property.</remarks>
public class EnumerationBase : IEquatable<EnumerationBase>, IComparable<EnumerationBase>
{
    private static readonly ConcurrentDictionary<Type, EnumerationBase[]> Cache = new();

    protected EnumerationBase(int id, string value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id, nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        Id = id;
        Value = value;
    }

    public int Id { get; }

    public string Value { get; }

    public static IEnumerable<T> GetAll<T>() where T : EnumerationBase
    {
        return Cache.GetOrAdd(typeof(T), type =>
        {
            return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == type)
                .Select(f => (EnumerationBase)f.GetValue(null))
                .ToArray();
        }).Cast<T>();
    }

    /// <summary>
    /// Retrieves an enumeration instance by its unique identifier.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration, which must derive from <see cref="EnumerationBase"/>.</typeparam>
    /// <param name="id">The unique identifier of the enumeration to retrieve.</param>
    /// <returns>The enumeration instance with the specified <paramref name="id"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no enumeration with the specified <paramref name="id"/> is found.</exception>
    public static T Parse<T>(int id) where T : EnumerationBase
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id, nameof(id));
        return GetAll<T>().FirstOrDefault(x => x.Id == id)
            ?? throw new InvalidOperationException($"{typeof(T).Name} not found with id {id}.");
    }

    /// <summary>
    /// Retrieves an enumeration instance by its string value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration, which must derive from <see cref="EnumerationBase"/>.</typeparam>
    /// <param name="value">The string value of the enumeration to retrieve.</param>
    /// <param name="comparisonType">The <see cref="StringComparison"/> to use for string comparison. Uses <see cref="StringComparison.OrdinalIgnoreCase"/> by default.</param>
    /// <returns>The enumeration instance with the specified <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null or white space.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no enumeration with the specified <paramref name="value"/> is found.</exception>
    public static T Parse<T>(string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) where T : EnumerationBase
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        return GetAll<T>().FirstOrDefault(x => x.Value.Equals(value, comparisonType))
            ?? throw new InvalidOperationException($"{typeof(T).Name} not found with value {value}.");
    }

    /// <summary>
    /// Returns the string representation of the enumeration, which is its <see cref="Value"/> property.
    /// </summary>
    /// <returns>The <see cref="Value"/> of the enumeration.</returns>
    public sealed override string ToString() => Value;

    /// <summary>
    /// Determines whether the specified object is equal to the current enumeration.
    /// </summary>
    /// <param name="obj">The object to compare with the current enumeration.</param>
    /// <returns>True if the specified object is an <see cref="EnumerationBase"/> with the same <see cref="Id"/>; otherwise, false.</returns>
    public sealed override bool Equals(object obj)
    {
        if (obj is EnumerationBase enumeration)
        {
            return Equals(enumeration);
        }

        return false;
    }

    /// <summary>
    /// Returns the hash code for the enumeration based on its <see cref="Id"/> property.
    /// </summary>
    /// <returns>The hash code for the enumeration.</returns>
    public sealed override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Determines whether the specified enumeration is equal to the current enumeration.
    /// </summary>
    /// <param name="other">The enumeration to compare with the current enumeration.</param>
    /// <returns>True if the specified enumeration has the same <see cref="Id"/>; otherwise, false."</returns>
    public bool Equals(EnumerationBase other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <summary>
    /// Compares the current enumeration with another enumeration for ordering.
    /// </summary>
    /// <param name="other">The enumeration to compare with the current enumeration.</param>
    /// <returns>A value less than zero if the current enumeration's <see cref="Id"/> is less than the other's, 
    /// zero if equal, or greater than zero if greater.</returns>
    public int CompareTo(EnumerationBase other)
    {
        if (other is null)
        {
            return 1;
        }

        return Id.CompareTo(other.Id);
    }
}
