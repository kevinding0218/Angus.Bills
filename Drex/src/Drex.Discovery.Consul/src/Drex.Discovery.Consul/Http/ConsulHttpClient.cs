using System.Net.Http;
using Drex.HTTP.HttpClient;

namespace Drex.Discovery.Consul.Http
{
    internal sealed class ConsulHttpClient : DrexHttpClient, IConsulHttpClient
    {
        public ConsulHttpClient(HttpClient client, HttpClientOptions options)
            : base(client, options)
        {
        }
    }
}