using System.Text.Json.Serialization;

namespace Drex.Discovery.Consul.Models
{
    public class Connect
    {
        [JsonPropertyName("sidecar_service")] public SidecarService SidecarService { get; set; }
    }
}