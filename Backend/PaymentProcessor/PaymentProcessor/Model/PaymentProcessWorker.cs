using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Net.WebSockets;
using BaseTypes;
using RabbitMQService;
using Azure;

namespace PaymentProcessor.Model
{
    public class PaymentProcessWorker
    {
        private readonly RabbitMqService _rabbitMQService;
        //private readonly ConnectionFactory _factory;
        private const string QueueName = nameof(EnumPaymentEvents.PaymentQueue);

        public PaymentProcessWorker()
        {
            _rabbitMQService = new RabbitMqService();

            //_factory = new ConnectionFactory() { HostName = "localhost" }; // Update RabbitMQ hostname as needed

            // Start consumer on initialization
            Task.Run(() => StartConsumer());
        }
        public async Task ResponseToWebSocket(WebSocket webSocket, string responseMessage)
        {
            var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }
        public async Task HandleWebSocketAsync(System.Net.WebSockets.WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
                if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closing", System.Threading.CancellationToken.None);
                }
                else
                {

                    string jsonString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ReqPaymentRequest? reqPaymentRequest = JsonConvert.DeserializeObject<ReqPaymentRequest>(jsonString);
                    if (reqPaymentRequest == null)
                    {
                        var res1 = new { msg = "Payment requests received and being processed." };
                        string jsonResponse1 = JsonConvert.SerializeObject(res1);

                        await ResponseToWebSocket(webSocket, jsonResponse1);
                        continue;
                    }
                    PaymentRequest paymentRequest = new PaymentRequest()
                    {
                        Amount = Convert.ToDecimal(reqPaymentRequest.PaymentInfo.Amount),
                        Currency = reqPaymentRequest.PaymentInfo.Currency,
                        CustomerName = reqPaymentRequest.CustomerInfo.Name,
                        CustomerEmail = reqPaymentRequest.CustomerInfo.Email,
                        //CustomerAddress= reqPaymentRequest.CustomerInfo.add,
                        CustomerMobile = reqPaymentRequest.CustomerInfo.Phone,
                        PaymentMethod = reqPaymentRequest.PaymentMethod,
                        AccountNumber = reqPaymentRequest.BankInfo?.AccountNumber,
                        RoutingNumber = reqPaymentRequest.BankInfo?.RoutingNumber,
                        CardNumber = reqPaymentRequest.CardInfo?.CardNumber,
                        ExpiryDate = reqPaymentRequest.CardInfo?.ExpiryDate,
                        CCV = reqPaymentRequest.CardInfo?.CVV

                    };



                    Console.WriteLine("Received payment request");

                    // Publish to RabbitMQ
                    PublishPaymentRequest(paymentRequest);
                    var res = new { msg = "Payment requests received and being processed." };
                    string jsonResponse = JsonConvert.SerializeObject(res);

                    await ResponseToWebSocket(webSocket, jsonResponse);

                }
            }
        }

        private void PublishPaymentRequest(PaymentRequest paymentRequest)
        {
            using var connection = _rabbitMQService.GetRabbitMQConnection();// _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(paymentRequest);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
            Console.WriteLine("Payment request published to RabbitMQ");
        }

        private void StartConsumer()
        {
            var connection = _rabbitMQService.GetRabbitMQConnection();// _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclarePassive(queue: QueueName);//, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentRequest = JsonConvert.DeserializeObject<PaymentRequest>(message);

                ProcessPayment(paymentRequest);
            };

            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private void ProcessPayment(PaymentRequest paymentRequest)
        {
            Console.WriteLine("Processing payment for: " + paymentRequest.CustomerName);
            // Simulate payment processing logic
            Console.WriteLine("Payment processed successfully.");
        }
    }
}
