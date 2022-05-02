using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands
{
    [Command(Name = "get", Description = "get option value for specified repository")]
    public class GetCmd : RepoArgBase
    {
        private readonly IConfiguration _config;

        public GetCmd(IConfiguration config)
        {
            _config = config;
        }

        [Argument(1)]
        [Required]
        public string Key { get; set; }

        protected int OnExecute(CommandLineApplication app)
        {
            Console.WriteLine($"Configuration for '{Repo}'");
            var token = _config.GetValue(Repo, Key);
            Console.WriteLine($"Token: {token}");
            return 0;
        }
    }
}
