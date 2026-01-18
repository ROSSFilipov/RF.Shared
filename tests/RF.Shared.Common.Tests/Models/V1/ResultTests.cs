using RF.Shared.Common.Models.V1;

namespace RF.Shared.Common.Tests.Models.V1;

public class ResultTests
{
    [Fact]
    public void Result_Success()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Result_Failure()
    {
        // Arrange
        var error = new Error("Sample error", 404, "The requested resource was not found.");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Result_Failure_NullError_ThrowsException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => Result.Failure(null));
    }

    [Fact]
    public void Result_Failure_ImplicitConversion()
    {
        // Arrange
        var error = new Error("Sample error", 500, "Internal server error.");

        // Act
        Result result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Result_Generic_Success()
    {
        // Arrange
        var value = 42;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Result_Generic_Failure()
    {
        // Arrange
        var error = new Error("Sample error", 400, "Bad request.");

        // Act
        var result = Result<int>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Result_Generic_Failure_NullError_ThrowsException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => Result<int>.Failure(null));
    }

    [Fact]
    public void Result_Generic_Success_NullValue_ThrowsException()
    {
        // Arrange
        string value = null;

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => Result<string>.Success(value));
    }

    [Fact]
    public void Result_Generic_Success_ImplicitConversion()
    {
        // Arrange
        var value = "Hello, World!";

        // Act
        Result<string> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Result_Generic_Failure_ImplicitConversion()
    {
        // Arrange
        var error = new Error("Sample error", 403, "Forbidden.");

        // Act
        Result<int> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}
