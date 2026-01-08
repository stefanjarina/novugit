using NSubstitute;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;

namespace Novugit.Base.Tests;

[NotInParallel("Configuration")]
public class ConfigurationTests : IAsyncDisposable
{
  private string _tempConfigPath;
  private string _tempConfigDir;
  private ISecretService _secretService;

  [Before(Test)]
  public void Setup()
  {
    // Create a temporary directory for config files
    _tempConfigDir = Path.Combine(Path.GetTempPath(), $"novugit_test_{Guid.NewGuid()}");
    Directory.CreateDirectory(_tempConfigDir);
    _tempConfigPath = Path.Combine(_tempConfigDir, "config.yml");

    // Create mock secret service
    _secretService = Substitute.For<ISecretService>();
    _secretService.Encrypt(Arg.Any<string>()).Returns(x => $"encrypted_{x.Arg<string>()}");
    _secretService.Decrypt(Arg.Any<string>()).Returns(x => x.Arg<string>()?.Replace("encrypted_", "") ?? "");
  }

  [After(Test)]
  public void Cleanup()
  {
    // Clean up temp directory
    if (Directory.Exists(_tempConfigDir))
    {
      try
      {
        Directory.Delete(_tempConfigDir, true);
      }
      catch
      {
        // Ignore cleanup errors
      }
    }
  }

  public ValueTask DisposeAsync()
  {
    Cleanup();
    return ValueTask.CompletedTask;
  }

  [Test]
  public async Task Constructor_CreatesDefaultConfig_WhenConfigFileDoesNotExist()
  {
    // Act
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    await Assert.That(configuration.Config).IsNotNull();
    await Assert.That(configuration.Config.DefaultBranch).IsEqualTo("main");
    await Assert.That(configuration.Config.Providers).IsNotNull();
    await Assert.That(configuration.Config.Providers.Count).IsEqualTo(6);
  }

  [Test]
  public async Task Constructor_CreatesConfigFile_WhenConfigFileDoesNotExist()
  {
    // Act
    _ = new Configuration(_secretService, _tempConfigPath);

    // Assert
    await Assert.That(File.Exists(_tempConfigPath)).IsTrue();
  }

  [Test]
  public async Task Constructor_LoadsExistingConfig_WhenConfigFileExists()
  {
    // Arrange
    var configContent = """
      DefaultBranch: develop
      Providers:
        - Name: github
          Token: test_token
          BaseUrl: https://github.com
          Options: {}
      """;
    File.WriteAllText(_tempConfigPath, configContent);

    // Act
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    await Assert.That(configuration.Config.DefaultBranch).IsEqualTo("develop");
    await Assert.That(configuration.Config.Providers.Count).IsEqualTo(1);
    await Assert.That(configuration.Config.Providers[0].Name).IsEqualTo("github");
  }

  [Test]
  public async Task GetProvider_ByName_ReturnsCorrectProvider()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var provider = configuration.GetProvider("github");

    // Assert
    await Assert.That(provider).IsNotNull();
    await Assert.That(provider.Name).IsEqualTo("github");
    await Assert.That(provider.BaseUrl).IsEqualTo("https://github.com");
  }

  [Test]
  public async Task GetProvider_ByName_ReturnsNull_WhenProviderDoesNotExist()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var provider = configuration.GetProvider("nonexistent");

    // Assert
    await Assert.That(provider).IsNull();
  }

  [Test]
  public async Task GetProvider_ByReposEnum_ReturnsCorrectProvider()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var provider = configuration.GetProvider(Repos.Github);

    // Assert
    await Assert.That(provider).IsNotNull();
    await Assert.That(provider.Name).IsEqualTo("github");
  }

  [Test]
  [Arguments(Repos.Azure, "azure")]
  [Arguments(Repos.Bitbucket, "bitbucket")]
  [Arguments(Repos.Forgejo, "forgejo")]
  [Arguments(Repos.Gitea, "gitea")]
  [Arguments(Repos.Github, "github")]
  [Arguments(Repos.Gitlab, "gitlab")]
  public async Task GetProvider_ByReposEnum_ReturnsCorrectProvider_ForAllProviders(Repos repo, string expectedName)
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var provider = configuration.GetProvider(repo);

    // Assert
    await Assert.That(provider).IsNotNull();
    await Assert.That(provider.Name).IsEqualTo(expectedName);
  }

  [Test]
  public async Task DecryptToken_CallsSecretServiceDecrypt()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var result = configuration.DecryptToken("encrypted_mytoken");

    // Assert
    await Assert.That(result).IsEqualTo("mytoken");
    _secretService.Received(1).Decrypt("encrypted_mytoken");
  }

  [Test]
  public async Task GetValue_ReturnsToken_WhenKeyIsToken()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Token = "encrypted_secret_token";

    // Act
    var result = configuration.GetValue("github", "token");

    // Assert
    await Assert.That(result).IsEqualTo("encrypted_secret_token");
  }

  [Test]
  public async Task GetValue_ReturnsDecryptedToken_WhenKeyIsTokenAndDecryptIsTrue()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Token = "encrypted_secret_token";

    // Act
    var result = configuration.GetValue("github", "token", decrypt: true);

    // Assert
    await Assert.That(result).IsEqualTo("secret_token");
    _secretService.Received().Decrypt("encrypted_secret_token");
  }

  [Test]
  public async Task GetValue_ReturnsBaseUrl_WhenKeyIsBaseUrl()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var result = configuration.GetValue("github", "baseurl");

    // Assert
    await Assert.That(result).IsEqualTo("https://github.com");
  }

  [Test]
  public async Task GetValue_ReturnsOptionValue_WhenKeyIsCustomOption()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Options["customKey"] = "customValue";

    // Act
    var result = configuration.GetValue("github", "customKey");

    // Assert
    await Assert.That(result).IsEqualTo("customValue");
  }

  [Test]
  public async Task GetValue_ReturnsEmptyString_WhenProviderDoesNotExist()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var result = configuration.GetValue("nonexistent", "token");

    // Assert
    await Assert.That(result).IsEqualTo("");
  }

  [Test]
  public async Task GetValue_ReturnsEmptyString_WhenOptionDoesNotExist()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    var result = configuration.GetValue("github", "nonexistent_option");

    // Assert
    await Assert.That(result).IsEqualTo("");
  }

  [Test]
  public async Task UpdateValue_UpdatesToken_WhenKeyIsToken()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "token", "new_token");

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.Token).IsEqualTo("encrypted_new_token");
    _secretService.Received().Encrypt("new_token");
  }

  [Test]
  public async Task UpdateValue_UpdatesBaseUrl_WhenKeyIsBaseUrl()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "baseurl", "https://enterprise.github.com");

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.BaseUrl).IsEqualTo("https://enterprise.github.com");
  }

  [Test]
  public async Task UpdateValue_UpdatesOption_WhenKeyIsCustomOption()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "organization", "myorg");

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.Options["organization"]).IsEqualTo("myorg");
  }

  [Test]
  public async Task UpdateValue_EncryptsOption_WhenEncryptIsTrue()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "secret_option", "secret_value", encrypt: true);

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.Options["secret_option"]).IsEqualTo("encrypted_secret_value");
    _secretService.Received().Encrypt("secret_value");
  }

  [Test]
  public async Task UpdateValue_DoesNotEncryptOption_WhenEncryptIsFalse()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "plain_option", "plain_value", encrypt: false);

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.Options["plain_option"]).IsEqualTo("plain_value");
  }

  [Test]
  public async Task UpdateValue_PersistsChangesToFile()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    configuration.UpdateValue("github", "token", "persisted_token");

    // Act - Create new instance to load from file
    var newConfiguration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    var provider = newConfiguration.GetProvider("github");
    await Assert.That(provider.Token).IsEqualTo("encrypted_persisted_token");
  }

  [Test]
  public async Task UpdateValue_DoesNothing_WhenProviderDoesNotExist()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var originalProviderCount = configuration.Config.Providers.Count;

    // Act
    configuration.UpdateValue("nonexistent", "token", "value");

    // Assert - No exception thrown and provider count unchanged
    await Assert.That(configuration.Config.Providers.Count).IsEqualTo(originalProviderCount);
  }

  [Test]
  public async Task RemoveValue_ClearsToken_WhenKeyIsToken()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Token = "some_token";

    // Act
    configuration.RemoveValue("github", "token");

    // Assert
    await Assert.That(provider.Token).IsEqualTo("");
  }

  [Test]
  public async Task RemoveValue_ClearsBaseUrl_WhenKeyIsBaseUrl()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.RemoveValue("github", "baseurl");

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.BaseUrl).IsEqualTo("");
  }

  [Test]
  public async Task RemoveValue_RemovesOption_WhenKeyIsCustomOption()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Options["customKey"] = "customValue";

    // Act
    configuration.RemoveValue("github", "customKey");

    // Assert
    await Assert.That(provider.Options.ContainsKey("customKey")).IsFalse();
  }

  [Test]
  public async Task RemoveValue_PersistsChangesToFile()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    configuration.UpdateValue("github", "token", "token_to_remove");
    configuration.RemoveValue("github", "token");

    // Act - Create new instance to load from file
    var newConfiguration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    var provider = newConfiguration.GetProvider("github");
    await Assert.That(provider.Token).IsEqualTo("");
  }

  [Test]
  public async Task RemoveValue_DoesNothing_WhenProviderDoesNotExist()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var originalProviderCount = configuration.Config.Providers.Count;

    // Act
    configuration.RemoveValue("nonexistent", "token");

    // Assert - No exception thrown and provider count unchanged
    await Assert.That(configuration.Config.Providers.Count).IsEqualTo(originalProviderCount);
  }

  [Test]
  public async Task GetValue_IsCaseInsensitive_ForKeyName()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act & Assert
    var result1 = configuration.GetValue("github", "TOKEN");
    var result2 = configuration.GetValue("github", "Token");
    var result3 = configuration.GetValue("github", "token");

    await Assert.That(result1).IsEqualTo(result2);
    await Assert.That(result2).IsEqualTo(result3);
  }

  [Test]
  public async Task UpdateValue_IsCaseInsensitive_ForKeyName()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Act
    configuration.UpdateValue("github", "BASEURL", "https://new-url.com");

    // Assert
    var provider = configuration.GetProvider("github");
    await Assert.That(provider.BaseUrl).IsEqualTo("https://new-url.com");
  }

  [Test]
  public async Task RemoveValue_IsCaseInsensitive_ForKeyName()
  {
    // Arrange
    var configuration = new Configuration(_secretService, _tempConfigPath);
    var provider = configuration.GetProvider("github");
    provider.Token = "some_token";

    // Act
    configuration.RemoveValue("github", "TOKEN");

    // Assert
    await Assert.That(provider.Token).IsEqualTo("");
  }

  [Test]
  public async Task Constructor_CreatesDefaultProviders_WithCorrectBaseUrls()
  {
    // Arrange & Act
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    await Assert.That(configuration.GetProvider("azure").BaseUrl).IsEqualTo("https://dev.azure.com");
    await Assert.That(configuration.GetProvider("bitbucket").BaseUrl).IsEqualTo("https://api.bitbucket.org");
    await Assert.That(configuration.GetProvider("forgejo").BaseUrl).IsEqualTo("");
    await Assert.That(configuration.GetProvider("gitea").BaseUrl).IsEqualTo("");
    await Assert.That(configuration.GetProvider("github").BaseUrl).IsEqualTo("https://github.com");
    await Assert.That(configuration.GetProvider("gitlab").BaseUrl).IsEqualTo("https://gitlab.com");
  }

  [Test]
  public async Task Constructor_CreatesDefaultProviders_WithEmptyTokens()
  {
    // Arrange & Act
    var configuration = new Configuration(_secretService, _tempConfigPath);

    // Assert
    foreach (var provider in configuration.Config.Providers)
    {
      await Assert.That(provider.Token).IsEqualTo("");
    }
  }
}
