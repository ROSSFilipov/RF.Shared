namespace RF.Shared.Common.Models.V1;

/// <summary>
/// Represents the outcome of an operation, indicating success or failure and providing error information if applicable.
/// </summary>
/// <remarks>Use the <see cref="Result"/> class to standardize the representation of operation results, especially
/// in scenarios where an operation can fail and you want to avoid using exceptions for control flow. A successful
/// result is created using <see cref="Success"/>, while a failed result is created using <see cref="Failure(Error)"/>
/// or by implicitly converting an <see cref="Error"/> to a <see cref="Result"/>. The <see cref="IsSuccess"/> property
/// indicates whether the operation succeeded, and the <see cref="Error"/> property provides details about the failure
/// if any. Implicit conversions allow you to return an error directly from a method without using the Result factory methods.
/// </remarks>
public class Result
{
    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        Error = error;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);


    public static implicit operator Result(Error error) => Failure(error);
}

/// <summary>
/// Represents the outcome of an operation that returns a value, encapsulating either a successful result with a value
/// of the specified type or a failure with an associated error.
/// </summary>
/// <remarks>Use this type to indicate the success or failure of an operation while carrying a value in the
/// success case or an error in the failure case. The <see cref="Result{T}"/> type provides static methods and implicit
/// conversions to simplify creating success or failure results. Accessing the <see cref="Value"/> property is only
/// valid when the result represents success; otherwise, it may throw an exception or yield undefined
/// behavior. Implicit conversions allow you to return a value of the generic type or an error directly from a method 
/// without using the Result factory methods.</remarks>
/// <typeparam name="T">The type of the value returned on a successful result.</typeparam>
public sealed class Result<T> : Result
{
    private Result(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;
    }

    private Result(Error error) : base(error) { }

    public T Value { get; }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Failure(error);
}
