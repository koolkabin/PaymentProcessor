using RabbitMQ.Client;
using System.Reflection;

namespace RabbitMQService
{
    public class RabbitMqService
    {
        // localhost rabbitmq adress
        private readonly string _hostName = "localhost";

        public IConnection GetRabbitMQConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                //definitins for default rabbitmq connection user (guest).You can change your own server information.
                HostName = _hostName,
                UserName = "admin",
                Password = "admin",
                Port = 5672
            };

            return connectionFactory.CreateConnection();
        }
    }
}
