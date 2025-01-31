using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQService;
using System.Text;

namespace PaymentProcessor.RabbitMQ
{
    public class PaymentProcessedConsumer
    {
        private readonly RabbitMqService _rabbitMQService;

        public PaymentProcessedConsumer(string _queueName)
        {
            _rabbitMQService = new RabbitMqService();

            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var _channel = connection.CreateModel())
                {
                    _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var _consumer = new EventingBasicConsumer(_channel);
                    _consumer.Received += async (model, ea) =>
                    {
                        string message = Encoding.UTF8.GetString(ea.Body.Span);
                        PaymentProcessCompleteEventModel eventData = JsonConvert.DeserializeObject<PaymentProcessCompleteEventModel>(message);

                        string response = "lets assume process result has been received and are to save things to db.";

                        Console.WriteLine("Queue:{0}, Received Message: \"{1}\", API Response: {2}",
                            _queueName, message, response);
                    };

                    _channel.BasicConsume(_queueName, true, _consumer);
                    //Console.ReadLine();
                }
            }
        }
        private static async Task<string> MockApiRequestAsync(PaymentRaisedEventModel paymentData)
        {
            // Simulate a 3-second delay
            await Task.Delay(3000);

            // Randomly return success or failure
            var random = new Random();
            bool isSuccess = random.Next(2) == 0;

            var response = new
            {
                paymentData,
                status = isSuccess ? "success" : "failure",
                timestamp = DateTime.Now
            };

            return JsonConvert.SerializeObject(response);
        }

    }

}
