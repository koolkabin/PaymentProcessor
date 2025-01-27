using RabbitMQService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

namespace PaymentEventConsumer
{
    public class PaymentRaisedConsumer
    {
        private readonly RabbitMqService _rabbitMQService;

        public PaymentRaisedConsumer(string _queueName)
        {
            _rabbitMQService = new RabbitMqService();

            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var _channel = connection.CreateModel())
                {
                    var _consumer = new EventingBasicConsumer(_channel);
                    // Received event'i sürekli listen modunda olacaktır.
                    _consumer.Received += async (model, ea) =>
                    {
                        //var body = ea.Body.Span;
                        string message = Encoding.UTF8.GetString(ea.Body.Span);
                        PaymentRaisedEventModel paymentRaisedEventModel = JsonConvert.DeserializeObject<PaymentRaisedEventModel>(message);

                        string response = await MockApiRequestAsync(paymentRaisedEventModel);

                        Console.WriteLine("Queue:{0}, Received Message: \"{1}\", API Response: {2}",
                            _queueName, message, response);
                        //_channel.BasicAck(ea.DeliveryTag, false); // Explicit act.
                    };

                    _channel.BasicConsume(_queueName, true, _consumer); // Implicit  act.
                    Console.ReadLine();
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
