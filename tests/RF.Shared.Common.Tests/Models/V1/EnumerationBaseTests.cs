namespace RF.Shared.Common.Tests.Models.V1;

using RF.Shared.Common.Models.V1;

public class EnumerationBaseTests
{
    public class Status : EnumerationBase
    {
        public static readonly Status Active = new(1, "Active");
        public static readonly Status Inactive = new(2, "Inactive");
        public static readonly Status Pending = new(3, "Pending");

        protected Status(int id, string value) : base(id, value) { }
    }

    [Fact]
    public void GetAll_ReturnsAllInstances()
    {
        // Act
        var statuses = EnumerationBase.GetAll<Status>();

        // Assert
        Assert.Equal(3, statuses.Count());
        Assert.Contains(statuses, x => x == Status.Active);
        Assert.Contains(statuses, x => x == Status.Inactive);
        Assert.Contains(statuses, x => x == Status.Pending);
    }

    [Fact]
    public void ParseById_ValidId_ReturnsCorrectInstance()
    {
        // Act
        var status = EnumerationBase.Parse<Status>(1);

        // Assert
        Assert.Equal(Status.Active, status);
    }

    [Fact]
    public void ParseById_NonExistentId_ThrowsInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => EnumerationBase.Parse<Status>(999));
    }

    [Fact]
    public void ParseById_NegativeId_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => EnumerationBase.Parse<Status>(-1));
    }

    [Fact]
    public void ParseByValue_ValidValue_ReturnsCorrectInstance()
    {
        // Act
        var status = EnumerationBase.Parse<Status>("Active");

        // Assert
        Assert.Equal(Status.Active, status);
    }

    [Fact]
    public void ParseByValue_CaseInsensitive_ReturnsCorrectInstance()
    {
        // Act
        var status = EnumerationBase.Parse<Status>("active", StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.Equal(Status.Active, status);
    }

    [Fact]
    public void ParseByValue_NullValue_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => EnumerationBase.Parse<Status>(null));
    }

    [Fact]
    public void ParseByValue_NonExistentValue_ThrowsInvalidOperationException()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => EnumerationBase.Parse<Status>("_"));
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        var status = Status.Active;

        // Act
        var result = status.ToString();

        // Assert
        Assert.Equal("Active", result);
    }

    [Fact]
    public void Equals_SameId_ReturnsTrue()
    {
        // Arrange
        var status1 = Status.Active;
        var status2 = Status.Active;

        // Act & Assert
        Assert.True(status1.Equals(status2));
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        // Arrange
        var status1 = Status.Active;
        var status2 = Status.Inactive;

        // Act & Assert
        Assert.False(status1.Equals(status2));
    }

    [Fact]
    public void Equals_NullOther_ReturnsFalse()
    {
        // Arrange
        var status = Status.Active;

        // Act & Assert
        Assert.False(status.Equals(null));
    }

    [Fact]
    public void Equals_NonEnumerationBaseObject_ReturnsFalse()
    {
        // Arrange
        var status = Status.Active;

        // Act & Assert
        Assert.False(status.Equals(new object()));
    }

    [Fact]
    public void GetHashCode_SameId_ReturnsSameHashCode()
    {
        // Arrange
        var id = 1;
        var status = Status.Active;

        // Act & Assert
        Assert.Equal(id.GetHashCode(), status.GetHashCode());
    }

    [Fact]
    public void CompareTo_SameId_ReturnsZero()
    {
        // Arrange
        var status1 = Status.Active;
        var status2 = Status.Active;

        // Act
        var result = status1.CompareTo(status2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_LowerId_ReturnsPositive()
    {
        // Arrange
        var status1 = Status.Inactive;
        var status2 = Status.Active;

        // Act
        var result = status1.CompareTo(status2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_HigherId_ReturnsNegative()
    {
        // Arrange
        var status1 = Status.Active;
        var status2 = Status.Inactive;

        // Act
        var result = status1.CompareTo(status2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_NullOther_ReturnsPositive()
    {
        // Arrange
        var status = Status.Active;

        // Act
        var result = status.CompareTo(null);

        // Assert
        Assert.Equal(1, result);
    }
}