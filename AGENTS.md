# AGENTS.md

Repo-specific guidance for coding agents.
Do not duplicate details that are obvious from reading the codebase.
Prefer README and source for discoverable behavior; keep this file for durable repo context.

## Workflow

- Prefer Task targets over ad hoc commands.
- Restore dependencies: `task restore`
- Build all configurations: `task build`
- Build one configuration: `task build_debug`, `task build_release`
- Run tests: `task test`
- Publish all supported runtimes: `task publish`
- Publish one platform: `task publish_linux`, `task publish_windows`, `task publish_macos`
- Full rebuild and publish: `task full_build`
- `task` with no arguments lists available tasks.
- Use raw `dotnet` commands only when a Task target is insufficient for the change you are making.
- Do not run `task git_clean` or `task git_reset` unless the user explicitly asks; both are destructive.

## Architecture

- The solution has three projects:
- `Novugit`: CLI entrypoint and command registration.
- `Novugit.API`: service implementations for git providers and workflow orchestration.
- `Novugit.Base`: shared contracts, models, configuration, exceptions, and helpers.
- The CLI is built with `Spectre.Console.Cli`.
- Commands follow a command/settings split and are registered through DI in `Novugit/Program.cs`.
- `RepoService` is the main orchestration layer for `init`.
- The orchestration flow is: load config, prompt for missing values, create the remote repo, generate `.gitignore`, initialize the local repo, then optionally push.
- Provider-specific implementations live in `Novugit.API/Services` and implement contracts from `Novugit.Base/Contracts`.
- Keep provider behavior aligned where possible; diverge only when the upstream API requires it.

## Git And Publishing

- Local repository creation uses `LibGit2Sharp`.
- Remote push uses the native `git` executable via helpers instead of `LibGit2Sharp`.
- This split is intentional for better credential and SSH handling during push.
- The CLI targets `net10.0`.
- Publishing is self-contained and single-file.
- Supported publish runtimes are `linux-x64`, `win-x64`, `osx-x64`, and `osx-arm64`.
- Avoid changes that break cross-platform publishability unless the user explicitly wants that tradeoff.

## Config And Secrets

- User config is stored at `~/.config/novugit/config.yml` on Unix-like systems.
- On Windows it is stored at `%HOMEDRIVE%%HOMEPATH%\.config\novugit\config.yml`.
- Missing config is created automatically with provider defaults.
- Provider tokens are encrypted before being written to config.
- Encryption goes through `ISecretService` and ASP.NET Data Protection.
- Azure stores `OrgName` in provider options.
- Gitea and Forgejo depend on configured `BaseUrl` values for self-hosted instances.

## Testing Guidance

- The main automated coverage is in `Novugit.Base.Tests`.
- Use `task test` for the standard test run.
- Prefer adding unit tests for changes in `Novugit.Base` and other non-interactive logic.
- Interactive prompt flows and live provider integrations have limited automated coverage.
- When touching provider or prompt-heavy code, verify behavior carefully even if tests stay green.
- Preserve the existing CLI surface unless the user explicitly requests a breaking change.
