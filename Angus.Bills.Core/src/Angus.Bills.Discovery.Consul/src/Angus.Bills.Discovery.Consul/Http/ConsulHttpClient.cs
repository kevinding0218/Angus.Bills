using System.Net.Http;
using Angus.Bills.HTTP.HttpClient;

namespace Angus.Bills.Discovery.Consul.Http
{
    internal sealed class ConsulHttpClient : AngusBillsHttpClient, IConsulHttpClient
    {
        public ConsulHttpClient(HttpClient client, HttpClientOptions options)
            : base(client, options)
        {
        }
    }
}