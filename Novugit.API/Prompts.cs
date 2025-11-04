using System.ComponentModel.DataAnnotations;

using Novugit.Base;
using Novugit.Base.Enums;
using Novugit.Base.Models;

using Sharprompt;

namespace Novugit.API;

public static class Prompts
{
    // AZURE SPECIFIC
    public static string AskForOrgName()
    {
        var validators = new List<Func<object, ValidationResult>> { Validators.Required() };

        var orgName = Prompt.Input<string>("Enter your organization name (https://dev.azure.com/{yourorgname})", validators: validators);

        return orgName;
    }

    public static string AskForAzureProject(IEnumerable<Dictionary<string, object>> projects)
    {
        var selector = delegate (Dictionary<string, object> dict)
        {
            return dict["name"].ToString();
        };

        var project = Prompt.Select("Which project to use?", items: projects, textSelector: selector);

        return project["id"].ToString();
    }

    // GITHUB SPECIFIC
    public static AuthMethods AskGithubAuthenticationMethod()
    {
        var method = Prompt.Select<AuthMethods>("Authentication method");

        return method;
    }

    public static (string, string) AskForGithubUserAndPassword()
    {
        var validators = new List<Func<object, ValidationResult>> { Validators.Required() };

        var username = Prompt.Input<string>("Enter your GitHub username or email address", validators: validators);
        var password = Prompt.Password("Enter your password", validators: validators);

        return (username, password);
    }

    public static string AskForTwoFactorAuthenticationCode()
    {
        var validators = new List<Func<object, ValidationResult>> { Validators.Required() };

        var twoFactorCode = Prompt.Input<string>("Enter your two-factor authentication code", validators: validators);

        return twoFactorCode;
    }

    // GITLAB SPECIFIC
    public static string AskForGitlabGroup(IEnumerable<Dictionary<string, string>> groups)
    {
        var selector = delegate (Dictionary<string, string> dict)
        {
            return dict["name"];
        };

        var group = Prompt.Select("Which group to use?", items: groups, textSelector: selector);

        return group["value"];
    }

    // GENERAL
    public static string AskForVisibility(Repos repo)
    {
        var visibilityOptions = new[] { "private", "public" };

        if (repo == Repos.Gitlab)
            visibilityOptions = new[] { "private", "internal", "public" };

        var visibility = Prompt.Select<string>("Visibility of a repository", items: visibilityOptions, defaultValue: "public");

        return visibility;
    }

    public static string AskForToken(string repo)
    {
        var validators = new List<Func<object, ValidationResult>> { Validators.Required() };

        var token = Prompt.Input<string>($"{repo} Personal Access Token", validators: validators);

        return token;
    }

    public static ProjectInfo AskForProjectInfo(Repos repo, IEnumerable<string> availableGitignoreConfigs)
    {
        var currentDirectoryInfo = Helpers.GetCurrentDirInfo();

        var validators = new List<Func<object, ValidationResult>> { Validators.Required() };
        var defaultGitIgnoreConfigs = new[] { "windows", "linux", "macos", "node", "dotnetcore", "visualstudiocode", "webstorm+all" };

        var name = Prompt.Input<string>("Name", defaultValue: currentDirectoryInfo.Name, validators: validators);
        var description = Prompt.Input<string>("Description (optional)", defaultValue: "");
        var visibility = AskForVisibility(repo);

        var (gitIgnoreConfigs, excludedLocalFiles) = AskForGitignoreDetails(currentDirectoryInfo, availableGitignoreConfigs);

        var projectInfo = new ProjectInfo
        {
            Name = name,
            Description = description,
            Visibility = visibility,
            GitIgnoreConfigs = gitIgnoreConfigs,
            ExcludedLocalFiles = excludedLocalFiles
        };

        return projectInfo;
    }

    public static (IEnumerable<string>, IEnumerable<string>) AskForGitignoreDetails(CurrentDirectoryInfo currentDirectoryInfo, IEnumerable<string> availableGitignoreConfigs)
    {
        var localExcludeList = currentDirectoryInfo.Files.Concat(currentDirectoryInfo.Directories);

        IEnumerable<string> gitIgnoreConfigs = Array.Empty<string>();
        IEnumerable<string> excludedLocalFiles = Array.Empty<string>();

        var createGitIgnore = true;

        if (currentDirectoryInfo.Files.Contains(".gitignore"))
        {
            var answer = Prompt.Confirm("Do you want to use existing .gitignore file?", defaultValue: true);

            if (answer)
                createGitIgnore = false;
        }

        var defaultGitIgnoreConfigs = new[] { "windows", "linux", "macos", "node", "dotnetcore", "visualstudiocode", "webstorm+all" };

        if (createGitIgnore)
        {
            gitIgnoreConfigs = Prompt.MultiSelect("Select config names you wish to fetch from https://gitignore.io",
                items: availableGitignoreConfigs,
                defaultValues: defaultGitIgnoreConfigs,
                minimum: 0,
                pageSize: 10);

            if (localExcludeList.Any())
                excludedLocalFiles = Prompt.MultiSelect("Select the files and/or folders you wish to ignore", items: localExcludeList, minimum: 0, defaultValues: new[] { "node_modules" });
        }

        return (gitIgnoreConfigs, excludedLocalFiles);
    }
}
