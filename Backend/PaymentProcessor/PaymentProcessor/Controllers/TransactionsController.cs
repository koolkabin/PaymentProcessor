using BaseTypes;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessor.Model;
using PaymentProcessor.RabbitMQ;

namespace PaymentProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly PaymentDBContext _dbContext;
        private readonly PaymentEventPublisher _publisher;

        public TransactionsController(PaymentDBContext dbContext, PaymentEventPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        [HttpGet("MakePayment")]
        public async Task MakePayment()
        {
            if (Request.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await Request.HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket connected");

                var processor = Request.HttpContext.RequestServices.GetRequiredService<PaymentProcessWorker>();
                await processor.HandleWebSocketAsync(webSocket);
            }
            else
            {
                Request.HttpContext.Response.StatusCode = 400;
            }
        }

        [HttpPost("payments")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest data)
        {
            data.Status = EnumPaymentStatus.Pending;
            //_dbContext.PaymentRequests.Add(data);
            //await _dbContext.SaveChangesAsync();

            //var eventMessage = new
            //{
            //    Type = "payment_initiated",
            //    data.TransactionId,
            //    data.CustomerId,
            //    data.Amount,
            //    data.Currency,
            //    Timestamp = data.Timestamp
            //};
            //balance deduction on sender + on processing balance -> 
            //publish event
            _publisher.PublishMessage(data);

            return Ok(new { Message = "Payment initiated", data.PaymentId });
        }

        [HttpGet]
        public IActionResult GetTransactions([FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate, [FromQuery] EnumPaymentStatus? status,
            [FromQuery] int? CustomerId)
        {
            var query = _dbContext.Transactions.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate);
            }
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status);
            }
            //if (CustomerId.HasValue)
            //{
            //    query = query.Where(t => t.CustomerId == CustomerId);
            //}

            return Ok(query.ToList());
        }

        [HttpGet("events/logs")]
        public IActionResult GetEventLogs()
        {
            // Return a placeholder as logs would require persistent storage in production
            return Ok(new { Message = "Event logs endpoint is operational." });
        }
    }
}
