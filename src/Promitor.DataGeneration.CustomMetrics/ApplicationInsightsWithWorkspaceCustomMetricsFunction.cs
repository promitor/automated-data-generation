using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Promitor.DataGeneration.CustomMetrics
{
    public class ApplicationInsightsWithWorkspaceCustomMetricsFunction : ApplicationInsightsCustomMetricsFunction
    {
        public ApplicationInsightsWithWorkspaceCustomMetricsFunction(IConfiguration configuration)
            : base("APPLICATIONINSIGHTS_WORKSPACE_INSTRUMENTATIONKEY", configuration)
        {
        }

        [FunctionName("custom-metrics-application-insights-workspace")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo timerInfo)
        {
            Logger.LogVerbose("Simulating new orders for workspace-based Azure Application Insights");

            SimulateNewOrdersCreated();

            Logger.LogVerbose("New orders simulated for workspace-based Azure Application Insights");
        }
    }
}
