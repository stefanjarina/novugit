# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Novugit is a CLI tool that automates git repository initialization by:
- Initializing a local git repository
- Creating a remote repository on various platforms (GitHub, GitLab, Azure DevOps, BitBucket, Gitea, Forgejo)
- Generating .gitignore files from gitignore.io templates
- Configuring remotes and pushing the initial commit

## Build and Development Commands

The project uses Task (taskfile.dev) as its task runner. Key commands:

```bash
# Restore dependencies
task restore

# Build (both Debug and Release)
task build

# Build Debug only
task build_debug

# Build Release only
task build_release

# Publish for all platforms (linux-x64, win-x64, osx-x64, osx-arm64)
task publish

# Publish for specific platform
task publish_linux
task publish_windows
task publish_macos

# Clean build artifacts
task clean

# Full clean, restore, build, and publish
task full_build
```

Alternatively, use dotnet CLI directly:
```bash
dotnet restore
dotnet build -c Debug
dotnet build -c Release
dotnet run --project Novugit -- init github
```

## Project Structure

The solution consists of three projects:

### 1. Novugit (Main CLI)
- **Location**: `Novugit/`
- **Type**: Executable (.NET 10)
- **Purpose**: Entry point and CLI command definitions
- **Key Framework**: McMaster.Extensions.CommandLineUtils for command parsing
- **Key Files**:
  - `Program.cs` - Dependency injection setup and command execution
  - `Commands/` - Command definitions (InitCmd, ConfigCmd, GitignoreCmd)

### 2. Novugit.API
- **Location**: `Novugit.API/`
- **Type**: Class library
- **Purpose**: Service implementations for all supported git platforms
- **Key Dependencies**:
  - `Octokit` - GitHub API client
  - `LibGit2Sharp` - Local git operations
  - `RestSharp` - HTTP client for GitLab, BitBucket, Gitea, Forgejo
  - `Microsoft.VisualStudio.Services.Client` - Azure DevOps API
  - `Kurukuru` - Terminal spinners
  - `Sharprompt` - Interactive prompts
- **Key Files**:
  - `Services/RepoService.cs` - Orchestrates the entire repository creation workflow
  - `Services/GithubService.cs`, `GitlabService.cs`, etc. - Provider-specific implementations
  - `Services/GitignoreService.cs` - Fetches .gitignore templates from gitignore.io
  - `Prompts.cs` - User interaction prompts

### 3. Novugit.Base
- **Location**: `Novugit.Base/`
- **Type**: Class library
- **Purpose**: Shared models, contracts (interfaces), enums, and utilities
- **Key Files**:
  - `Configuration.cs` - Manages YAML config at `~/.config/novugit/config.yml`
  - `Contracts/IProvider.cs` - Base interface for all git platform providers
  - `Contracts/IRepoService.cs`, `IGithubService.cs`, etc. - Service contracts
  - `Models/` - Data models (ProjectInfo, Provider, Config, etc.)
  - `Helpers.cs` - Utility functions including command execution with CliWrap

## Architecture Patterns

### Service Layer Pattern
All git platform implementations follow the IProvider interface and have dedicated service interfaces:
- Each platform (GitHub, GitLab, Azure, BitBucket, Gitea, Forgejo) has its own service interface in `Novugit.Base/Contracts/`
- Implementations live in `Novugit.API/Services/`
- Services are registered in `Program.cs` ConfigureServices() using dependency injection

### Command Pattern
Commands use McMaster.Extensions.CommandLineUtils attributes:
- `[Command]` - Defines command name and description
- `[Subcommand]` - Defines hierarchical command structure
- `[Argument]` / `[Option]` - Defines command-line arguments
- Commands inherit from `GlobalCommandOptionsBase`

### Configuration Management
- Config stored as YAML at `~/.config/novugit/config.yml` (Linux/Mac) or `%HOMEDRIVE%%HOMEPATH%\.config\novugit\config.yml` (Windows)
- Configuration.cs handles reading/writing provider tokens and settings
- Config structure: DefaultBranch + list of Provider objects (name, token, baseUrl, options)

### Workflow Orchestration
The `RepoService` orchestrates the complete flow:
1. Prompt for token (stored in config if not present)
2. Authenticate with provider
3. Fetch provider-specific data (projects, groups, organizations, workspaces)
4. Prompt user for repository details
5. Create remote repository
6. Generate .gitignore file from templates
7. Initialize local git repository with LibGit2Sharp
8. Push to remote using native git command via CliWrap

## Provider Implementation Details

Each provider service implements authentication differently:
- **GitHub**: Uses Octokit with token credentials
- **GitLab**: RestSharp with custom API calls to gitlab.com or custom instance
- **Azure DevOps**: Microsoft.VisualStudio.Services.Client SDK
- **BitBucket**: RestSharp with workspace/project hierarchy
- **Gitea/Forgejo**: RestSharp with organization-based API (Forgejo uses same API as Gitea)

All providers return a `ProjectInfo` object containing the remote URL and repository metadata.

## Key Technical Decisions

### Single-File Publishing
The main project is configured for single-file, self-contained publishing:
- `PublishSingleFile=true`
- `PublishReadyToRun=true`
- Targets .NET 10

### Git Operations
- Local git operations use LibGit2Sharp (portable C# implementation)
- Push operations use native git command via CliWrap for better credential handling and SSH support
- Platform determines URL type: HTTPS on Windows, SSH on Unix-like systems

### Error Handling
- Custom `NovugitException` for application errors
- Exception handling in Program.cs Main() catches CommandParsingException and NovugitException
- Each service operation wrapped in try-catch with spinner UI feedback

## Configuration File Format

```yaml
DefaultBranch: main
Providers:
  - Name: github
    Token: ""
    BaseUrl: https://github.com
    Options: {}
  - Name: gitlab
    Token: ""
    BaseUrl: https://gitlab.com
    Options: {}
  # ... other providers
```

Providers can have custom options stored in the Options dictionary (e.g., Azure stores OrgName).
