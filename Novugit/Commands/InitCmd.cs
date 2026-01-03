﻿using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;

namespace Novugit.Commands;

[Command(Name = "init", Description = "Initialize new repository")]
public class InitCmd(IRepoService repoService) : GlobalCommandOptionsBase
{
    [Required]
    [Argument(0)]
    [McMaster.Extensions.CommandLineUtils.AllowedValues("github", "gitlab", "azure", "bitbucket", "forgejo", "gitea", IgnoreCase = true)]
    public string Repo { get; init; }

    protected async Task<int> OnExecute(CommandLineApplication app)
    {
        ApplyGlobalOptions(app);
        
        var currentDir = Environment.CurrentDirectory;
        if (Directory.Exists(Path.Join(currentDir, ".git")))
        {
            ConsoleOutput.WriteWarning("Already a git repository, exiting...");
            return 0;
        }
        
        var repoType = Enum.Parse<Repos>(Repo.Capitalize());

        var projectInfo = await repoService.CreateRemoteRepo(repoType);

        await repoService.CreateGitIgnoreFile(projectInfo);
        await repoService.InitializeLocalGit(Enum.Parse<Repos>(Repo.Capitalize()), projectInfo);

        return 0;
    }
}