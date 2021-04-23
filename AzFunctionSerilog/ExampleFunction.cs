using AzFunctionSerilog.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Serilog;
using Serilog.Context;

namespace AzFunctionSerilog {
    /// <summary>
    /// Simple example function that uses Dependency Injection to inject the logger. 
    /// </summary>
    public class ExampleFunction {

        private readonly ILogger _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleFunction" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ExampleFunction(IAzFunctionLogger logger) {
            _log = logger.Log;
        }

        /// <summary>
        /// Runs the specified req against this function.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="executionContext">The execution context.</param>
        /// <returns>Returns the <see cref="IActionResult"/>.</returns>
        [FunctionName("ExampleFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request, ExecutionContext executionContext) {

            using (LogContext.PushProperty("SessionId", executionContext.InvocationId)) {
                _log.Information("Starting Session...");
                _log.Information("Request has been made and logged!");
                _log.Information("Ending Session...");
            }

            return new OkObjectResult("Hello, Serilog");
        }
    }
}
