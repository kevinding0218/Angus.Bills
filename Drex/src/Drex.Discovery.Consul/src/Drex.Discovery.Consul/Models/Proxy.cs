using System.Collections.Generic;

namespace Drex.Discovery.Consul.Models
{
    public class Proxy
    {
        public List<Upstream> Upstreams { get; set; }
    }
}