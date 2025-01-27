namespace PaymentProcessor.Model
{
    // Transaction entity
    public class Transaction : PaymentRequest
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}

