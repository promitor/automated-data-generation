using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Promitor.DataGeneration.CustomMetrics
{
    public class ApplicationInsightsCustomMetricsFunction
    {
        public ILogger<ApplicationInsightsCustomMetricsFunction> Logger { get; }
        private readonly Random _randomizer = new Random();
        private const string OrdersCreatedMetricName = "Orders Created";

        private readonly List<string> _tenants = new List<string>{ "Contoso", "Fabrikam", "Promito" };

        internal ApplicationInsightsCustomMetricsFunction(string configurationKeyName, IConfiguration configuration)
        {
            var instrumentationKey = configuration.GetValue<string>(configurationKeyName);
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithComponentName("Promitor Automation")
                .Enrich.WithVersion()
                .WriteTo.Console()
                .WriteTo.AzureApplicationInsights(instrumentationKey);

            var serilogLogger = loggerConfiguration.CreateLogger();
            Logger = new SerilogLoggerFactory(serilogLogger)
                    .CreateLogger<ApplicationInsightsCustomMetricsFunction>();
        }

        public void SimulateNewOrdersCreated()
        {
            foreach (var tenant in _tenants)
            {
                SimulateNewOrdersCreated(tenant);
            }
        }

        private void SimulateNewOrdersCreated(string tenantName)
        {
            // Annotate with contextual information
            var contextualInformation = new Dictionary<string, object>
            {
                {"Tenant", tenantName}
            };

            // Generate order amount            
            var orderAmount = _randomizer.Next(100, 1337);

            // Log metric
            Logger.LogMetric(OrdersCreatedMetricName, orderAmount, contextualInformation);
        }
    }
}
