using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace AzFunctionSerilog.Infrastructure.Logging {

    /// <summary>
    /// Class AzFunctionLogger.
    /// Implements the <see cref="AzFunctionSerilog.Infrastructure.Logging.IAzFunctionLogger" />
    /// </summary>
    /// <seealso cref="AzFunctionSerilog.Infrastructure.Logging.IAzFunctionLogger" />
    public class AzFunctionLogger : IAzFunctionLogger {
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzFunctionLogger"/> class.
        /// </summary>
        /// <param name="configurationRoot">The configuration root.</param>
        public AzFunctionLogger(IConfigurationRoot configurationRoot) {

            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .CreateLogger();
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Log => _logger;
    }
}
