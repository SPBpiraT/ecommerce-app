using ecommerce_app.backend.web.Common.Providers;
using ecommerce_app.backend.web.Services.YooKassaClient;
using ecommerce_app.backend.web.Models.Order;

using Dapper;
using System.Data;
using ecommerce_app.backend.web.Exceptions;

namespace ecommerce_app.backend.web.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IYooKassaClient _yooKassaClient;

        public OrderService(IDbConnection dbConnection,
            IDateTimeProvider dateTimeProvider,
            IYooKassaClient yooKassaClient)
        {
            _dbConnection = dbConnection;
            _dateTimeProvider = dateTimeProvider;
            _yooKassaClient = yooKassaClient;
        }

        public async Task<CreateOrderResponse> CheckoutAsync(CreateOrderRequest request, Guid userId, CancellationToken cancellationToken = default)
        {
            var orderId = Guid.NewGuid();

            var order = new Entities.Order
            {
                Id = orderId,
                UserId = userId,
                TotalAmount = request.TotalAmount,
                Status = "Pending",
                PaymentId = null,
                Created = _dateTimeProvider.UtcNow
            };

            try
            {
                await CreateAsync(order, cancellationToken);

                var yooKassaResponse = await _yooKassaClient.CreatePaymentAsync(orderId, order.TotalAmount, cancellationToken);

                if (yooKassaResponse == null || yooKassaResponse.Confirmation == null)
                {
                    throw new PaymentGatewayException("Failed to initiate payment via YooKassa.");
                }

                await UpdatePaymentInfoAsync(orderId, yooKassaResponse.Id, cancellationToken);

                var response = new CreateOrderResponse
                {
                    OrderId = orderId,
                    Status = order.Status,
                    PaymentUrl = yooKassaResponse.Confirmation.ConfirmationUrl
                };

                return response;
            }
            catch (Exception)
            {
                await UpdateStatusAsync(orderId, "Canceled", cancellationToken);
                throw;
            }
        }

        public async Task CreateAsync(Entities.Order order, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO orders (id, user_id, total_amount, status, payment_id, created, updated)
                VALUES (@Id, @UserId, @TotalAmount, @Status, @PaymentId, @Created, @Updated);";

            var command = new CommandDefinition(sql, order, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }

        public async Task UpdatePaymentInfoAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE orders 
                SET payment_id = @PaymentId,
                    updated = @Updated
                WHERE id = @Id;";

            var parameters = new
            {
                Id = orderId,
                PaymentId = paymentId,
                Updated = _dateTimeProvider.UtcNow
            };

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }

        public async Task<Entities.Order?> GetByPaymentIdAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT 
                    id, 
                    user_id AS userid, 
                    total_amount AS totalamount, 
                    status, 
                    payment_id AS paymentid, 
                    created, 
                    updated
                FROM orders 
                WHERE payment_id = @PaymentId;";

            var command = new CommandDefinition(sql, new { PaymentId = paymentId }, cancellationToken: cancellationToken);
            return await _dbConnection.QueryFirstOrDefaultAsync<Entities.Order>(command);
        }

        public async Task UpdateStatusAsync(Guid orderId, string status, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE orders 
                SET status = @Status,
                    updated = @Updated
                WHERE id = @Id;";

            var parameters = new
            {
                Id = orderId,
                Status = status,
                Updated = _dateTimeProvider.UtcNow
            };

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }
    }
}
