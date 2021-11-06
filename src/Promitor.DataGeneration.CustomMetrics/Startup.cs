using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.DataGeneration.CustomMetrics;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Promitor.DataGeneration.CustomMetrics
{
    public class Startup : FunctionsStartup
    {
        // This method gets called by the runtime. Use this method to configure the app configuration.
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder.AddEnvironmentVariables();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.ConfigureSecretStore(stores =>
            {
                stores.AddEnvironmentVariables();
            });

            builder.Services.AddLogging(logging =>
            {
                logging.ClearProvidersExceptFunctionProviders();
            });
        }
    }
}
