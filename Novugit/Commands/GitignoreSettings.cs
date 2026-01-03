using JetBrains.Annotations;

namespace Novugit.Commands;

/// <summary>
/// Settings for the gitignore command.
/// No arguments needed - just inherits global options.
/// </summary>
[UsedImplicitly]
public class GitignoreSettings : GlobalSettings
{
    // No additional properties - command takes no arguments
}
