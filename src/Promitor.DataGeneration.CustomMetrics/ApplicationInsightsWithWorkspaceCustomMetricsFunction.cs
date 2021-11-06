using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
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
            Logger.LogInformation("Simulating new orders for workspace-based Azure Application Insights");

            SimulateNewOrdersCreated();

            Logger.LogInformation("New orders simulated for workspace-based Azure Application Insights");
        }
    }
}
