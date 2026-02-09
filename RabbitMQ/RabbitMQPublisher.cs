using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System;
using System.Diagnostics;

namespace RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
    {
        private readonly IConfiguration _configuration;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
            // Intended: connect to RabbitMQ. Client library removed for build simplicity.
        }

        public void Publish<T>(string routingKey, T message)
        {
            // Serialize and log the message as a placeholder implementation
            string messageJson = JsonSerializer.Serialize(message);
#if DEBUG
            Debug.WriteLine($"[RabbitMQPublisher] RoutingKey: {routingKey}, Message: {messageJson}");
#endif
        }

        public void Dispose()
        {
            // Nothing to dispose in this placeholder
        }
    }
}
