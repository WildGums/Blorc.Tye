namespace Blorc.Example
{
    using System.Threading.Tasks;

  
    using Blorc.Services;
    using Blorc.Tye;
    using Blorc.Tye.Services;
    using Blorc.Tye.Shared;

    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlorcCore();
            builder.Services.AddTransient<SurveyExecutionService>();
            builder.Services.AddTransient<SurveyVisualizationService>();

            await builder
                .Build()
                .MapComponentServices(options =>
                    {
                        options.Map<SurveyPrompt, SurveyExecutionService>();
                        options.Map<SurveyPrompt, SurveyVisualizationService>();
                    })
               .RunAsync();
        }
    }
}
