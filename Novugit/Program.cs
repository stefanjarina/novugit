using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Novugit.API.Services;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Commands;

namespace Novugit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();

            var app = new CommandLineApplication<NovugitCmd>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException e)
            {
                app.Error.WriteLine(e.Message);

                if (e is UnrecognizedCommandParsingException uex && uex.NearestMatches.Any())
                {
                    app.Error.WriteLine();
                    app.Error.WriteLine("Did you mean this?");
                    app.Error.WriteLine("    " + uex.NearestMatches.First());
                }
            }
            finally
            {
                // Flush logs to the console https://github.com/aspnet/Logging/issues/631
                (services as IDisposable)?.Dispose();
                Environment.Exit(-1);
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(config => config.AddConsole());

            // add configuration
            serviceCollection.AddSingleton<IConfiguration, Configuration>();

            // add services
            serviceCollection.AddScoped<IRepoService, RepoService>();
            serviceCollection.AddScoped<IAzureService, AzureService>();
            serviceCollection.AddScoped<IGithubService, GithubService>();
            serviceCollection.AddScoped<IGitlabService, GitlabService>();
            serviceCollection.AddScoped<IGitignoreService, GitignoreService>();
        }
    }
}
