using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Novugit.Commands;

public abstract class RepoArgBase
{
    [Required]
    [Argument(0)]
    [McMaster.Extensions.CommandLineUtils.AllowedValues("github", "gitlab", "azure", IgnoreCase = true)]
    public string Repo { get; init; }
}