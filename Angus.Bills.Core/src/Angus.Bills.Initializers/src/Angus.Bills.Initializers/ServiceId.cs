using System;

namespace Angus.Bills.Initializers
{
    public class ServiceId : IServiceId
    {
        public string Id { get; } = $"{Guid.NewGuid():N}";
    }
}