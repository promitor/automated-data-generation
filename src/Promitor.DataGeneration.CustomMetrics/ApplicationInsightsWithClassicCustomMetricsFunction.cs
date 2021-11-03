using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Promitor.DataGeneration.CustomMetrics
{
    public class ApplicationInsightsWithClassicCustomMetricsFunction : ApplicationInsightsCustomMetricsFunction
    {
        public ApplicationInsightsWithClassicCustomMetricsFunction(IConfiguration configuration)
            : base("APPLICATIONINSIGHTS_CLASSIC_INSTRUMENTATIONKEY", configuration)
        {
        }

        [FunctionName("custom-metrics-application-insights-classic")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo timerInfo)
        {
            Logger.LogVerbose("Simulating new orders for classic Azure Application Insights");

            SimulateNewOrdersCreated();

            Logger.LogVerbose("New orders simulated for classic Azure Application Insights");
        }
    }
}
