namespace Blorc.Tye
{
    using System;
    using System.Threading.Tasks;

    using Blorc.Services;

    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The web assembly host builder extensions.
    /// </summary>
    public static class WebAssemblyHostBuilderExtensions
    {
        /// <summary>
        /// The assembly host builder.
        /// </summary>
        private static WebAssemblyHostBuilder AssemblyHostBuilder;

        /// <summary>
        /// Checks if is the micronetes configuration file is available.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<bool> IsTyeConfigurationAvailableAsync(this WebAssemblyHostBuilder @this)
        {
            GetWebAssemblyHostBuilder().Services.AddBlorcCore();
            var assemblyHost = GetWebAssemblyHostBuilder().Build();

            var configurationService = assemblyHost.Services.GetService<IConfigurationService>();
            var section = await configurationService.GetSectionAsync<Tye>("tye");

            return !string.IsNullOrWhiteSpace(section?.Url);
        }

        /// <summary>
        /// Gets web assembly host builder.
        /// </summary>
        /// <returns>
        /// The <see cref="WebAssemblyHostBuilder"/>.
        /// </returns>
        private static WebAssemblyHostBuilder GetWebAssemblyHostBuilder()
        {
            return AssemblyHostBuilder ?? (AssemblyHostBuilder = WebAssemblyHostBuilder.CreateDefault(Array.Empty<string>()));
        }
    }
}
