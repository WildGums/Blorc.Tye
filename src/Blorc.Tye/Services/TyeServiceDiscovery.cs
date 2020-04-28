// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicronetesServiceDiscovery.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Blorc.Tye.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Blorc.Services;
    using Blorc.Services.Interfaces;
    using Blorc.Tye.Model.Configuration;
    using Blorc.Tye.Model.Tye.Hosting;

    using Newtonsoft.Json;

    /// <summary>
    ///     The Micronetes service discovery.
    /// </summary>
    public class TyeServiceDiscovery : IServiceDiscovery
    {
        /// <summary>
        ///     The configuration service.
        /// </summary>
        private readonly IConfigurationService _configurationService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TyeServiceDiscovery" /> class.
        /// </summary>
        /// <param name="configurationService">
        ///     The configuration service.
        /// </param>
        public TyeServiceDiscovery(IConfigurationService configurationService)
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
            var endPoint = string.Empty;

            var tyeSection = await _configurationService.GetSection<Tye>("tye");

            var apiV1Services = "api/v1/services";
            var address = new Uri(tyeSection.Url);
            
            var httpClient = new HttpClient { BaseAddress = address };

            var deserializedService = await httpClient.GetStringAsync($"{apiV1Services}/{serviceName}");

            var service = JsonConvert.DeserializeObject<Service>(deserializedService);

            var serviceBinding = service.Description.Bindings.ElementAt(idx);
            if (serviceBinding != null)
            {
                endPoint = $"{serviceBinding.Protocol}://{address.Host}:{serviceBinding.Port}";
            }

            return endPoint;
        }

        public Task<string> GetServiceEndPoint(string serviceName, string bindingName)
        {
            throw new NotImplementedException();
        }
    }
}
