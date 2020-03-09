// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicronetesServiceDiscovery.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Blorc.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Blorc.Model.Configuration;
    using Blorc.Model.Micronetes.Hosting;
    using Blorc.Services.Interfaces;

    using Newtonsoft.Json;

    /// <summary>
    /// The Micronetes service discovery.
    /// </summary>
    public class MicronetesServiceDiscovery : IServiceDiscovery
    {
        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly IConfigurationService _configurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicronetesServiceDiscovery"/> class.
        /// </summary>
        /// <param name="configurationService">
        /// The configuration service.
        /// </param>
        public MicronetesServiceDiscovery(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        /// <summary>
        ///     The get service end point.
        /// </summary>
        /// <param name="serviceName">
        ///     The service name.
        /// </param>
        /// <param name="idx">
        ///     The idx.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task<string> GetServiceEndPoint(string serviceName, int idx = 0)
        {
            var micronetesSection = await _configurationService.GetSection<Micronetes>("micronetes");

            var apiV1Services = "api/v1/services";
            var httpClient = new HttpClient { BaseAddress = new Uri(micronetesSection.Url) };

            var deserializedService = await httpClient.GetStringAsync($"{apiV1Services}/{serviceName}");

            var service = JsonConvert.DeserializeObject<Service>(deserializedService);

            var endPoint = string.Empty;

            var serviceBinding = service.Description.Bindings.ElementAt(idx);
            if (serviceBinding != null)
            {
                endPoint = $"{serviceBinding.Protocol}://127.0.0.1:{serviceBinding.Port}";
            }

            return endPoint;
        }
    }
}
