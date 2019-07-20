using AzFunctionSerilog.Infrastructure.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(AzFunctionSerilog.Startup))]

namespace AzFunctionSerilog {

    /// <summary>
    /// Class Startup.
    /// Implements the <see cref="Microsoft.Azure.WebJobs.Hosting.IWebJobsStartup" />
    /// </summary>
    /// <seealso cref="Microsoft.Azure.WebJobs.Hosting.IWebJobsStartup" />
    public class Startup : IWebJobsStartup {

        private IConfigurationRoot _configurationRoot;

        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Configure(IWebJobsBuilder builder) {

            // Add dependency injection for the ILogger.
            builder.Services.AddSingleton<IAzFunctionLogger>(logger => new AzFunctionLogger(Configuration));
        }

        /// <summary>
        /// Gets the constructed <see cref="IConfigurationRoot"/> instance.
        /// </summary>
        /// <returns>IConfigurationRoot.</returns>
        private IConfigurationRoot GetConfiguration() {

            var environmentSettings = $"appsettings.{AzureVariables.AzureFunctionsEnvironment}.json";

            return new ConfigurationBuilder()
                .SetBasePath(FunctionRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(environmentSettings, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        private IConfigurationRoot Configuration => (_configurationRoot ?? (_configurationRoot = GetConfiguration()));

        /// <summary>
        /// Gets the function root directory path.
        /// </summary>
        /// <value>The function root directory path.</value>
        private string FunctionRootPath =>
            AzureVariables.AzureWebJobsScriptRoot ?? $"{AzureVariables.HomeDirectory}/site/wwwroot";
    }
}
