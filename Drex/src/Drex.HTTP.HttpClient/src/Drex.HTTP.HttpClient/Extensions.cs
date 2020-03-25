using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Drex.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

[assembly: InternalsVisibleTo("Drex.Discovery.Consul")]
[assembly: InternalsVisibleTo("Drex.LoadBalancing.Fabio")]

namespace Drex.HTTP.HttpClient
{
    public static class Extensions
    {
        private const string SectionName = "httpClient";
        private const string RegistryName = "http.client";

        public static IDrexBuilder AddHttpClient(this IDrexBuilder builder,
            string clientName = "Drex",
            IEnumerable<string> maskedRequestUrlParts = null, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            if (!builder.TryRegister(RegistryName)) return builder;

            if (string.IsNullOrWhiteSpace(clientName))
                throw new ArgumentException("HTTP client name cannot be empty.", nameof(clientName));

            var options = builder.GetOptions<HttpClientOptions>(sectionName);
            if (maskedRequestUrlParts is {} && options.RequestMasking is {})
                options.RequestMasking.UrlParts = maskedRequestUrlParts;

            builder.Services.AddSingleton(options);
            builder.Services.AddHttpClient<IHttpClient, DrexHttpClient>(clientName);
            if (options.RequestMasking?.Enabled == true)
                builder.Services.Replace(ServiceDescriptor
                    .Singleton<IHttpMessageHandlerBuilderFilter, DrexHttpLoggingFilter>());

            return builder;
        }

        [Description("This is a hack related to HttpClient issue: https://github.com/aspnet/AspNetCore/issues/13346")]
        internal static void RemoveHttpClient(this IDrexBuilder builder)
        {
            var registryType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .SingleOrDefault(t => t.Name == "HttpClientMappingRegistry");
            var registry = builder.Services.SingleOrDefault(s => s.ServiceType == registryType)?.ImplementationInstance;
            var registrations = registry?.GetType().GetProperty("TypedClientRegistrations");
            var clientRegistrations = registrations?.GetValue(registry) as IDictionary<Type, string>;
            clientRegistrations?.Remove(typeof(IHttpClient));
        }
    }
}