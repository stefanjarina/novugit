using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Novugit.API.Services;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Commands;

namespace Novugit;

public static class Program
{
    public static int Main(string[] args)
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
            return app.Execute(args);
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

            return 1;
        }
        catch (NovugitException e)
        {
            var message = !string.IsNullOrEmpty(e.Provider)
                ? $"[{e.Provider}] {e.Message}"
                : e.Message;
            
            ConsoleOutput.WriteError($"Error: {message}", e.InnerException);
            return 1;
        }
        catch (Exception e)
        {
            ConsoleOutput.WriteError($"Unexpected error: {e.Message}", e);
            return 1;
        }
        finally
        {
            // Flush logs to the console https://github.com/aspnet/Logging/issues/631
            services.Dispose();
        }
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // add logging
        serviceCollection.AddLogging(config => config.AddConsole());

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