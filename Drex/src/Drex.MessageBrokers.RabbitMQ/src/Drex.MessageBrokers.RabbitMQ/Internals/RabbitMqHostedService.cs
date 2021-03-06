using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Drex.MessageBrokers.RabbitMQ.Internals
{
    internal sealed class RabbitMqHostedService : IHostedService
    {
        private readonly IConnection _connection;

        public RabbitMqHostedService(IConnection connection)
        {
            _connection = connection;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _connection.Close();
            }
            catch
            {
                // ignored
            }

            return Task.CompletedTask;
        }
    }
}