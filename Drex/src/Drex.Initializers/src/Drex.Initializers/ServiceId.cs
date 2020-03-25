using System;

namespace Drex.Initializers
{
    public class ServiceId : IServiceId
    {
        public string Id { get; } = $"{Guid.NewGuid():N}";
    }
}