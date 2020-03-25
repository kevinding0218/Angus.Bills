using System.Net.Http;
using Drex.HTTP.HttpClient;

namespace Drex.LoadBalancing.Fabio.Http
{
    internal sealed class FabioHttpClient : DrexHttpClient, IFabioHttpClient
    {
        public FabioHttpClient(HttpClient client, HttpClientOptions options)
            : base(client, options)
        {
        }
    }
}