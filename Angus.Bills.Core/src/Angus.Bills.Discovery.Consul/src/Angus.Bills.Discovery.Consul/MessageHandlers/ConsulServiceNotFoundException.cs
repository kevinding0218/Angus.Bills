using System;

namespace Angus.Bills.Discovery.Consul.MessageHandlers
{
    internal sealed class ConsulServiceNotFoundException : Exception
    {
        public ConsulServiceNotFoundException(string serviceName) : this(string.Empty, serviceName)
        {
        }

        public ConsulServiceNotFoundException(string message, string serviceName) : base(message)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}