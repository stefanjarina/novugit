using Novugit.Base;

namespace Novugit.Base.Tests;

public class HelpersTests
{
  [Test]
  public async Task ConvertObjectToYaml_WithSimpleObject_ReturnsYamlString()
  {
    // Arrange
    var obj = new { Name = "test", Value = 123 };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Name: test");
    await Assert.That(result).Contains("Value: 123");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithNestedObject_ReturnsYamlString()
  {
    // Arrange
    var obj = new
    {
      Name = "parent",
      Child = new { Name = "child", Value = 456 }
    };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Name: parent");
    await Assert.That(result).Contains("Child:");
    await Assert.That(result).Contains("Name: child");
    await Assert.That(result).Contains("Value: 456");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithList_ReturnsYamlString()
  {
    // Arrange
    var obj = new
    {
      Items = new[] { "item1", "item2", "item3" }
    };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Items:");
    await Assert.That(result).Contains("- item1");
    await Assert.That(result).Contains("- item2");
    await Assert.That(result).Contains("- item3");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithEmptyObject_ReturnsYamlString()
  {
    // Arrange
    var obj = new { };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).IsNotNull();
  }

  [Test]
  public async Task ConvertObjectToYaml_WithDictionary_ReturnsYamlString()
  {
    // Arrange
    var obj = new Dictionary<string, string>
    {
      { "key1", "value1" },
      { "key2", "value2" }
    };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("key1: value1");
    await Assert.That(result).Contains("key2: value2");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithBooleanValues_ReturnsYamlString()
  {
    // Arrange
    var obj = new { Enabled = true, Disabled = false };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Enabled: true");
    await Assert.That(result).Contains("Disabled: false");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithNullValue_ReturnsYamlString()
  {
    // Arrange
    var obj = new { Name = "test", NullValue = (string)null };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Name: test");
    await Assert.That(result).Contains("NullValue:");
  }

  [Test]
  public async Task ConvertObjectToYaml_WithSpecialCharacters_ReturnsYamlString()
  {
    // Arrange
    var obj = new { Message = "Hello: World!" };

    // Act
    var result = Helpers.ConvertObjectToYaml(obj);

    // Assert
    await Assert.That(result).Contains("Message:");
    await Assert.That(result).Contains("Hello: World!");
  }
}
