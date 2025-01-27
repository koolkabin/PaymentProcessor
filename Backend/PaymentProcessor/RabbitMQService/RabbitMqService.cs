using BaseTypes;
using RabbitMQ.Client;

namespace RabbitMQService
{
    public class PaymentRaisedEventModel
    {
        // Payment details
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public EnumPaymentStatus Status { get; set; } = EnumPaymentStatus.Pending;

        // Customer details
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobile { get; set; }

        // Payment method
        public EnumPaymentMethod PaymentMethod { get; set; } = EnumPaymentMethod.Bank; // "Card" or "Bank"

        // Optional: Add additional fields for card or bank-specific details if needed
        public string CardNumber { get; set; } // If PaymentMethod = Card
        public string BankName { get; set; }   // If PaymentMethod = Bank


    }
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
