using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using System;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace PaymentProcessor.Consumers
{
    // RabbitMQ Publisher Service
    public class RabbitMqPublisherService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "payment_events";

        public RabbitMqPublisherService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishEvent(object eventMessage)
        {
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventMessage));
            _channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: messageBody);
        }
    }
}
