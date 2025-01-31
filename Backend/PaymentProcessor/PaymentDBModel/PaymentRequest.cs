using BaseTypes;

namespace PaymentProcessor.Model
{
    public class PaymentRequest
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
        public string? CardNumber { get; set; } // If PaymentMethod = Card
        public string? ExpiryDate { get; set; } // If PaymentMethod = Card
        public string? CCV { get; set; } // If PaymentMethod = Card
        public string? AccountNumber { get; set; }   // If PaymentMethod = Bank
        public string? RoutingNumber { get; set; }   // If PaymentMethod = Bank
    }
    public record ReqPaymentRequest
    {
        public CustomerInfo CustomerInfo { get; set; }
        public CardInfo? CardInfo { get; set; }
        public BankInfo? BankInfo { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
    }

    public record CustomerInfo { public string Name { get; set; } public string Email { get; set; } public string Phone { get; set; } }
    public record CardInfo { public string CardNumber { get; set; } public string ExpiryDate { get; set; } public string CVV { get; set; } }
    public record BankInfo { public string AccountNumber { get; set; } public string RoutingNumber { get; set; } }
    public record PaymentInfo { public string Amount { get; set; } public string Currency { get; set; } }

}

