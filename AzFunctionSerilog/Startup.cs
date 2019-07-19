using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: WebJobsStartup(typeof(AzFunctionSerilog.Startup))]

namespace AzFunctionSerilog {

    /// <summary>
    /// Class Startup.
    /// Implements the <see cref="Microsoft.Azure.WebJobs.Hosting.IWebJobsStartup" />
    /// </summary>
    /// <seealso cref="Microsoft.Azure.WebJobs.Hosting.IWebJobsStartup" />
    /// TODO Edit XML Comment Template for Startup
    public class Startup : IWebJobsStartup {

        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// TODO Edit XML Comment Template for Configure
        public void Configure(IWebJobsBuilder builder) {

            // Add dependency injection for the ILogger.
            builder.Services.AddSingleton<ILogger>(logger => GetSeriLogger());
        }

        /// <summary>
        /// Gets the constructed <see cref="IConfigurationRoot"/> instance.
        /// </summary>
        /// <returns>IConfigurationRoot.</returns>
        public IConfigurationRoot GetConfiguration() {

            var environmentSettings = $"appsettings.{AzureVariables.AzureFunctionsEnvironment}.json";

            return new ConfigurationBuilder()
                .SetBasePath(FunctionRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(environmentSettings, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Gets the Serilog Logger.
        /// </summary>
        /// <returns><see cref="Serilog.ILogger"/>.</returns>
        public ILogger GetSeriLogger() {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfiguration())
                // The following enrichments should be loaded from the appsettings.json files "Serilog:Enrich" values.
                //.Enrich.FromLogContext()
                //.Enrich.WithThreadId()
                //.Enrich.WithMachineName()
                .CreateLogger();

            // At this point, you should see logger._enricher contain 4 values in it's collection.
            return logger;
        }

        /// <summary>
        /// Gets the function root directory path.
        /// </summary>
        /// <value>The function root directory path.</value>
        public string FunctionRootPath =>
            AzureVariables.AzureWebJobsScriptRoot ?? $"{AzureVariables.HomeDirectory}/site/wwwroot";
    }
}
