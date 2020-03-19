using System.Collections.Generic;

namespace Angus.Bills.Discovery.Consul.Models
{
    public class Proxy
    {
        public List<Upstream> Upstreams { get; set; }
    }
}