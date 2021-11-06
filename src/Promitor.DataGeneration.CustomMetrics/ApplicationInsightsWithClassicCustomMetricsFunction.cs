using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
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
            Logger.LogInformation("Simulating new orders for classic Azure Application Insights");

            SimulateNewOrdersCreated();

            Logger.LogInformation("New orders simulated for classic Azure Application Insights");
        }
    }
}
