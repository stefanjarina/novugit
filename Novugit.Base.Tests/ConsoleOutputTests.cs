using Novugit.Base;

namespace Novugit.Base.Tests;

[NotInParallel("ConsoleOutput")]
public class ConsoleOutputTests
{
  [Before(Test)]
  public void Setup()
  {
    // Reset static state before each test
    ConsoleOutput.NoColor = false;
    ConsoleOutput.Verbose = false;
  }

  [After(Test)]
  public void Cleanup()
  {
    // Reset static state after each test
    ConsoleOutput.NoColor = false;
    ConsoleOutput.Verbose = false;
  }

  [Test]
  public async Task NoColor_WhenSetToTrue_ReturnsTrue()
  {
    // Arrange & Act
    ConsoleOutput.NoColor = true;

    // Assert
    await Assert.That(ConsoleOutput.NoColor).IsTrue();
  }

  [Test]
  public async Task NoColor_WhenSetToFalse_ReturnsFalse()
  {
    // Arrange & Act
    ConsoleOutput.NoColor = false;

    // Assert
    await Assert.That(ConsoleOutput.NoColor).IsFalse();
  }

  [Test]
  public async Task Verbose_WhenSetToFalse_ReturnsFalse()
  {
    // Arrange & Act
    ConsoleOutput.Verbose = false;

    // Assert
    await Assert.That(ConsoleOutput.Verbose).IsFalse();
  }

  [Test]
  public async Task Verbose_WhenSetToTrue_ReturnsTrue()
  {
    // Arrange & Act
    ConsoleOutput.Verbose = true;

    // Assert
    await Assert.That(ConsoleOutput.Verbose).IsTrue();
  }

  [Test]
  public async Task WriteInfo_WritesToConsole()
  {
    // Arrange
    var originalOut = Console.Out;
    var stringWriter = new StringWriter();

    try
    {
      Console.SetOut(stringWriter);

      // Act
      ConsoleOutput.WriteInfo("Test message");

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Test message");
    }
    finally
    {
      Console.SetOut(originalOut);
    }
  }

  [Test]
  public async Task WriteSuccess_WritesToConsole()
  {
    // Arrange
    var originalOut = Console.Out;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true; // Disable color to simplify testing

    try
    {
      Console.SetOut(stringWriter);

      // Act
      ConsoleOutput.WriteSuccess("Success message");

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Success message");
    }
    finally
    {
      Console.SetOut(originalOut);
    }
  }

  [Test]
  public async Task WriteWarning_WritesToConsole()
  {
    // Arrange
    var originalOut = Console.Out;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true; // Disable color to simplify testing

    try
    {
      Console.SetOut(stringWriter);

      // Act
      ConsoleOutput.WriteWarning("Warning message");

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Warning message");
    }
    finally
    {
      Console.SetOut(originalOut);
    }
  }

  [Test]
  public async Task WriteError_WritesToStdErr()
  {
    // Arrange
    var originalErr = Console.Error;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true; // Disable color to simplify testing

    try
    {
      Console.SetError(stringWriter);

      // Act
      ConsoleOutput.WriteError("Error message");

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Error message");
    }
    finally
    {
      Console.SetError(originalErr);
    }
  }

  [Test]
  public async Task WriteError_WithException_WhenVerboseIsFalse_DoesNotWriteStackTrace()
  {
    // Arrange
    var originalErr = Console.Error;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true;
    ConsoleOutput.Verbose = false;
    var exception = new Exception("Test exception");

    try
    {
      Console.SetError(stringWriter);

      // Act
      ConsoleOutput.WriteError("Error message", exception);

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Error message");
      await Assert.That(output).DoesNotContain("Stack trace:");
    }
    finally
    {
      Console.SetError(originalErr);
    }
  }

  [Test]
  public async Task WriteError_WithException_WhenVerboseIsTrue_WritesStackTrace()
  {
    // Arrange
    var originalErr = Console.Error;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true;
    ConsoleOutput.Verbose = true;
    var exception = new Exception("Test exception");

    try
    {
      Console.SetError(stringWriter);

      // Act
      ConsoleOutput.WriteError("Error message", exception);

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Error message");
      await Assert.That(output).Contains("Stack trace:");
    }
    finally
    {
      Console.SetError(originalErr);
    }
  }

  [Test]
  public async Task WriteError_WithNullException_WhenVerboseIsTrue_DoesNotThrow()
  {
    // Arrange
    var originalErr = Console.Error;
    var stringWriter = new StringWriter();
    ConsoleOutput.NoColor = true;
    ConsoleOutput.Verbose = true;

    try
    {
      Console.SetError(stringWriter);

      // Act
      ConsoleOutput.WriteError("Error message", null);

      // Assert
      var output = stringWriter.ToString();
      await Assert.That(output).Contains("Error message");
      await Assert.That(output).DoesNotContain("Stack trace:");
    }
    finally
    {
      Console.SetError(originalErr);
    }
  }
}
