using Novugit.Base;

namespace Novugit.Base.Tests;

public class ExtensionsTests
{
  [Test]
  public async Task Capitalize_WithLowercaseString_ReturnsCapitalizedString()
  {
    // Arrange
    var input = "hello";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("Hello");
  }

  [Test]
  public async Task Capitalize_WithUppercaseString_ReturnsSameString()
  {
    // Arrange
    var input = "HELLO";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("HELLO");
  }

  [Test]
  public async Task Capitalize_WithMixedCaseString_CapitalizesFirstLetterOnly()
  {
    // Arrange
    var input = "hELLO";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("HELLO");
  }

  [Test]
  public async Task Capitalize_WithSingleCharacter_ReturnsUppercaseCharacter()
  {
    // Arrange
    var input = "a";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("A");
  }

  [Test]
  public async Task Capitalize_WithEmptyString_ReturnsEmptyString()
  {
    // Arrange
    var input = "";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("");
  }

  [Test]
  public async Task Capitalize_WithNull_ReturnsNull()
  {
    // Arrange
    string input = null;

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsNull();
  }

  [Test]
  public async Task Capitalize_WithNumberPrefix_ReturnsSameString()
  {
    // Arrange
    var input = "123abc";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo("123abc");
  }

  [Test]
  public async Task Capitalize_WithWhitespacePrefix_ReturnsSameString()
  {
    // Arrange
    var input = " hello";

    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo(" hello");
  }

  [Test]
  [Arguments("github", "Github")]
  [Arguments("gitlab", "Gitlab")]
  [Arguments("azure", "Azure")]
  [Arguments("bitbucket", "Bitbucket")]
  [Arguments("forgejo", "Forgejo")]
  [Arguments("gitea", "Gitea")]
  public async Task Capitalize_WithProviderNames_ReturnsExpectedResult(string input, string expected)
  {
    // Act
    var result = input.Capitalize();

    // Assert
    await Assert.That(result).IsEqualTo(expected);
  }
}
