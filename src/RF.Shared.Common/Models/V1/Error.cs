using System.Collections.Frozen;

namespace RF.Shared.Common.Models.V1;

public readonly record struct Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified title, code, message, and optional extensions.
    /// </summary>
    /// <param name="title">The short, human-readable summary of the error.</param>
    /// <param name="code">The numeric code that uniquely identifies the error.</param>
    /// <param name="message">The detailed message describing the error.</param>
    /// <param name="extensions">An optional dictionary containing additional key-value pairs that provide extra context about the error.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="message"/> is null, empty, or consists only of white-space characters.</exception>"
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="extensions"/> is null.</exception>"
    public Error(string title, int code, string message, IDictionary<string, string> extensions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentOutOfRangeException.ThrowIfNegative(code);

        Title = title;
        Code = code;
        Message = message;
        Extensions = extensions?.ToFrozenDictionary()
            ?? FrozenDictionary<string, string>.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the Error class with default values.
    /// </summary>
    public Error() : this(null, -1, null, null) { }

    public string Title { get; }

    public int Code { get; }

    public string Message { get; }

    public FrozenDictionary<string, string> Extensions { get; }

    public bool Equals(Error other)
    {
        return Title == other.Title
               && Code == other.Code
               && Message == other.Message
               && Extensions.Count == other.Extensions.Count
               && !Extensions.Except(other.Extensions).Any();
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Title);
        hash.Add(Code);
        hash.Add(Message);
        return hash.ToHashCode();
    }
}
