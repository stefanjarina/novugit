using Novugit.Base;

namespace Novugit.Base.Tests;

public class NovugitExceptionTests
{
  [Test]
  public async Task Constructor_WithMessage_SetsMessage()
  {
    // Arrange & Act
    var exception = new NovugitException("Test error message");

    // Assert
    await Assert.That(exception.Message).IsEqualTo("Test error message");
  }

  [Test]
  public async Task Constructor_WithMessage_ProviderIsNull()
  {
    // Arrange & Act
    var exception = new NovugitException("Test error message");

    // Assert
    await Assert.That(exception.Provider).IsNull();
  }

  [Test]
  public async Task Constructor_WithMessageAndProvider_SetsMessageAndProvider()
  {
    // Arrange & Act
    var exception = new NovugitException("Test error message", "github");

    // Assert
    await Assert.That(exception.Message).IsEqualTo("Test error message");
    await Assert.That(exception.Provider).IsEqualTo("github");
  }

  [Test]
  public async Task Constructor_WithMessageAndInnerException_SetsMessageAndInnerException()
  {
    // Arrange
    var innerException = new InvalidOperationException("Inner error");

    // Act
    var exception = new NovugitException("Outer error message", innerException);

    // Assert
    await Assert.That(exception.Message).IsEqualTo("Outer error message");
    await Assert.That(exception.InnerException).IsEqualTo(innerException);
    await Assert.That(exception.InnerException!.Message).IsEqualTo("Inner error");
  }

  [Test]
  public async Task Constructor_WithMessageAndInnerException_ProviderIsNull()
  {
    // Arrange
    var innerException = new InvalidOperationException("Inner error");

    // Act
    var exception = new NovugitException("Outer error message", innerException);

    // Assert
    await Assert.That(exception.Provider).IsNull();
  }

  [Test]
  public async Task Constructor_WithMessageProviderAndInnerException_SetsAllProperties()
  {
    // Arrange
    var innerException = new InvalidOperationException("Inner error");

    // Act
    var exception = new NovugitException("Outer error message", "gitlab", innerException);

    // Assert
    await Assert.That(exception.Message).IsEqualTo("Outer error message");
    await Assert.That(exception.Provider).IsEqualTo("gitlab");
    await Assert.That(exception.InnerException).IsEqualTo(innerException);
  }

  [Test]
  [Arguments("github")]
  [Arguments("gitlab")]
  [Arguments("azure")]
  [Arguments("bitbucket")]
  [Arguments("forgejo")]
  [Arguments("gitea")]
  public async Task Constructor_WithDifferentProviders_SetsProviderCorrectly(string provider)
  {
    // Arrange & Act
    var exception = new NovugitException("Error for provider", provider);

    // Assert
    await Assert.That(exception.Provider).IsEqualTo(provider);
  }

  [Test]
  public async Task NovugitException_IsException()
  {
    // Arrange & Act
    var exception = new NovugitException("Test message");

    // Assert
    await Assert.That(exception).IsAssignableTo<Exception>();
  }

  [Test]
  public async Task NovugitException_CanBeCaughtAsException()
  {
    // Arrange
    Exception caughtException = null;

    // Act
    try
    {
      throw new NovugitException("Test message", "github");
    }
    catch (Exception ex)
    {
      caughtException = ex;
    }

    // Assert
    await Assert.That(caughtException).IsNotNull();
    await Assert.That(caughtException).IsAssignableTo<NovugitException>();
    await Assert.That(((NovugitException)caughtException!).Provider).IsEqualTo("github");
  }

  [Test]
  public async Task NovugitException_WithEmptyProvider_SetsEmptyString()
  {
    // Arrange & Act
    var exception = new NovugitException("Error message", "");

    // Assert
    await Assert.That(exception.Provider).IsEqualTo("");
  }

  [Test]
  public async Task NovugitException_PreservesInnerExceptionChain()
  {
    // Arrange
    var innermost = new ArgumentException("Innermost");
    var middle = new InvalidOperationException("Middle", innermost);

    // Act
    var exception = new NovugitException("Outermost", "azure", middle);

    // Assert
    await Assert.That(exception.InnerException).IsEqualTo(middle);
    await Assert.That(exception.InnerException!.InnerException).IsEqualTo(innermost);
  }
}
