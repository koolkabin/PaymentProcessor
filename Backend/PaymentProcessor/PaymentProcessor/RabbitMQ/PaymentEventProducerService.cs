using BaseTypes;
using PaymentProcessor.Model;
using RabbitMQ.Client;
using RabbitMQService;
using System.Text;

namespace PaymentProcessor.RabbitMQ
{
    public class PaymentEventPublisher : IDisposable
    {
        private readonly RabbitMqService _rabbitMQService;
        private readonly IModel _rabbitMQchannel;
        private readonly IConnection _rabbitMQconection;
        private const string _routingKey = nameof(EnumPaymentEvents.PaymentRaised);
        public PaymentEventPublisher()
        {
            _rabbitMQService = new RabbitMqService();
            _rabbitMQconection = _rabbitMQService.GetRabbitMQConnection();
            _rabbitMQchannel = _rabbitMQconection.CreateModel();

            _rabbitMQchannel.QueueDeclare(_routingKey, false, false, false);
        }
        public string PublishMessage(PaymentRequest data)
        {

            #region Persistence Message example
            /*
                var props = _rabbitMQchannel.CreateBasicProperties();
                props.Persistent = true; // or props.DeliveryMode = 2;
            */
            #endregion           

            //default exchange.
            var _message = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            _rabbitMQchannel.BasicPublish("", _routingKey, null, Encoding.UTF8.GetBytes(_message));

            return $"Queue:{_routingKey}, Outgoing message:{_message}";
        }

        public void Dispose()
        {
            _rabbitMQchannel?.Dispose();
            _rabbitMQconection?.Dispose();
        }

    }

}
