using BaseTypes;
using PaymentEventConsumer;

var _consumer = new PaymentRaisedConsumer(nameof(EnumPaymentEvents.PaymentQueue));
