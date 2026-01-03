using JetBrains.Annotations;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config all command.
/// No arguments needed - just inherits global options.
/// </summary>
[UsedImplicitly]
public class ConfigListAllSettings : ConfigSettings
{
    // No additional properties - command takes no arguments
}
