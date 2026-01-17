using RF.Shared.Common.Models.V1;

namespace RF.Shared.Common.Tests.Models.V1;

public class ResultTests
{
    [Fact]
    public void Success()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Failure()
    {
        // Arrange
        var error = new Error("Sample error", 404, "The requested resource was not found.");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}
