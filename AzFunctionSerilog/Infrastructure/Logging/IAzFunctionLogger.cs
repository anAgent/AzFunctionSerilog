
namespace AzFunctionSerilog.Infrastructure.Logging {

    /// <summary>
    /// Interface IAzFunctionLogger
    /// </summary>
    public interface IAzFunctionLogger {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        Serilog.ILogger Log { get; }
    }
}
