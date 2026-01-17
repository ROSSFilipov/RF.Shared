using RF.Shared.Common.Models.V1;

namespace RF.Shared.Common.Tests.Models.V1;

public class ErrorTests
{
    [Fact]
    public void Error_Constructor_InvalidParameters_ThrowsException()
    {
        // Arrange
        var validTitle = "Valid Title";
        var validCode = 200;
        var validMessage = "Valid Message";
        var validExtensions = new Dictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Error(null, validCode, validMessage, validExtensions));
        Assert.Throws<ArgumentException>(() => new Error(string.Empty, validCode, validMessage, validExtensions));
        Assert.Throws<ArgumentException>(() => new Error("   ", validCode, validMessage, validExtensions));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Error(validTitle, -1, validMessage, validExtensions));
        Assert.Throws<ArgumentNullException>(() => new Error(validTitle, validCode, null, validExtensions));
        Assert.Throws<ArgumentException>(() => new Error(validTitle, validCode, string.Empty, validExtensions));
        Assert.Throws<ArgumentException>(() => new Error(validTitle, validCode, "   ", validExtensions));
    }

    [Fact]
    public void Error_Constructor_Default()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Error());
    }

    [Fact]
    public void Error_Constructor()
    {
        // Arrange
        var title = "Sample Error";
        var code = 404;
        var message = "The requested resource was not found.";
        var extensions = new Dictionary<string, string>
        {
            { "Detail", "Additional error details." }
        };

        // Act
        var error = new Error(title, code, message, extensions);

        // Assert
        Assert.Equal(title, error.Title);
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
        Assert.Equal(extensions, error.Extensions);
    }

    [Fact]
    public void Error_Constructor_NullExtensions()
    {
        // Arrange
        var title = "Sample Error";
        var code = 500;
        var message = "Internal server error.";
        IDictionary<string, string> extensions = null;

        // Act
        var error = new Error(title, code, message, extensions);

        // Assert
        Assert.Equal(title, error.Title);
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
        Assert.NotNull(error.Extensions);
        Assert.Empty(error.Extensions);
    }

    [Fact]
    public void Error_Constructor_EmptyExtensions()
    {
        // Arrange
        var title = "Sample Error";
        var code = 400;
        var message = "Bad request.";
        var extensions = new Dictionary<string, string>();

        // Act
        var error = new Error(title, code, message, extensions);

        // Assert
        Assert.Equal(title, error.Title);
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
        Assert.NotNull(error.Extensions);
        Assert.Empty(error.Extensions);
    }

    [Fact]
    public void Error_Extensions_Immutability()
    {
        // Arrange
        var title = "Sample Error";
        var code = 400;
        var message = "Bad request.";
        var extensions = new Dictionary<string, string>()
        {
            { "Detail", "Additional error details." }
        };

        // Act
        var error = new Error(title, code, message, extensions);
        extensions["Detail"] = "Modified error details";

        // Assert
        Assert.Equal(title, error.Title);
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
        Assert.Equal("Additional error details.", error.Extensions["Detail"]);
    }

    [Fact]
    public void Error_Equal()
    {
        // Arrange
        var title = "Sample Error";
        var code = 403;
        var message = "Forbidden.";
        var extensions = new Dictionary<string, string>
        {
            { "Reason", "User does not have permission." }
        };

        var error1 = new Error(title, code, message, extensions);
        var error2 = new Error(title, code, message, extensions);

        // Act & Assert
        Assert.Equal(error1, error2);
        Assert.True(error1.Equals(error2));
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void Error_NotEqual()
    {   
        // Arrange
        var error1 = new Error("Error One", 401, "Unauthorized.", new Dictionary<string, string>());
        var error2 = new Error("Error Two", 402, "Payment Required.", new Dictionary<string, string>());

        // Act & Assert
        Assert.NotEqual(error1, error2);
        Assert.False(error1.Equals(error2));
        Assert.NotEqual(error1.GetHashCode(), error2.GetHashCode());
    }
}
