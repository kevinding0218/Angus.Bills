using System.Net.Http;
using Angus.Bills.HTTP.HttpClient;

namespace Angus.Bills.LoadBalancing.Fabio.Http
{
    internal sealed class FabioHttpClient : AngusBillsHttpClient, IFabioHttpClient
    {
        public FabioHttpClient(HttpClient client, HttpClientOptions options)
            : base(client, options)
        {
        }
    }
}