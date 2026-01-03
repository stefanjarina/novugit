using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Novugit.API.Services;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Commands;
using Novugit.Commands.ConfigCommands;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

namespace Novugit;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Setup DI
        var services = new ServiceCollection();
        ConfigureServices(services);

        // Cancellation token for Ctrl+C
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        // Setup CommandApp with DI
        using var registrar = new DependencyInjectionRegistrar(services);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.SetApplicationName("novugit");

            // Add top-level commands
            config.AddCommand<InitCommand>("init")
                .WithDescription("Initialize new repository");

            config.AddCommand<GitignoreCommand>("gitignore")
                .WithDescription("Generate .gitignore file");

            // Add config branch with subcommands
            config.AddBranch<ConfigSettings>("config", cfg =>
            {
                cfg.SetDescription("Manage configuration");
                cfg.AddCommand<ConfigGetCommand>("get")
                    .WithDescription("Get option value for specified repository");
                cfg.AddCommand<ConfigSetCommand>("set")
                    .WithDescription("Set token for specified repository");
                cfg.AddCommand<ConfigRemoveCommand>("remove")
                    .WithDescription("Remove token for specified repository");
                cfg.AddCommand<ConfigListAllCommand>("all")
                    .WithDescription("List whole configuration");
            });
        });

        try
        {
            return await app.RunAsync(args, cts.Token);
        }
        catch (CommandRuntimeException e) when (e.InnerException is NovugitException ne)
        {
            Console.WriteLine("WENT IN CommandRuntimeException");
            // Unwrap NovugitException from CommandRuntimeException
            var message = !string.IsNullOrEmpty(ne.Provider)
                ? $"[{ne.Provider}] {ne.Message}"
                : ne.Message;

            ConsoleOutput.WriteError($"Error: {message}", ne.InnerException);
            return 1;
        }
        catch (NovugitException e)
        {
            Console.WriteLine("WENT IN NovugitException");
            var message = !string.IsNullOrEmpty(e.Provider)
                ? $"[{e.Provider}] {e.Message}"
                : e.Message;

            ConsoleOutput.WriteError($"Error: {message}", e.InnerException);
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine("WENT IN Exception");
            ConsoleOutput.WriteError($"Unexpected error: {e.Message}", e);
            return 1;
        }
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // add data protection
        serviceCollection.AddDataProtection().SetApplicationName("novugit");
        serviceCollection.AddSingleton<ISecretService, SecretService>();

        // add configuration
        serviceCollection.AddSingleton<IConfiguration, Configuration>();


        // add services
        serviceCollection.AddScoped<IGitignoreService, GitignoreService>();
        serviceCollection.AddScoped<IRepoService, RepoService>();
        serviceCollection.AddScoped<IAzureService, AzureService>();
        serviceCollection.AddScoped<IBitBucketService, BitBucketService>();
        serviceCollection.AddScoped<IForgejoService, ForgejoService>();
        serviceCollection.AddScoped<IGiteaService, GiteaService>();
        serviceCollection.AddScoped<IGithubService, GithubService>();
        serviceCollection.AddScoped<IGitlabService, GitlabService>();
    }
}
