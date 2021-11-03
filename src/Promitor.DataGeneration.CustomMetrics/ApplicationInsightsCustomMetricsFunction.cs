using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Promitor.DataGeneration.CustomMetrics
{
    public class ApplicationInsightsCustomMetricsFunction
    {
        public ILogger Logger { get; }
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

            Logger = loggerConfiguration.CreateLogger();
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
            var contextInformation = new Dictionary<string, object>
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
