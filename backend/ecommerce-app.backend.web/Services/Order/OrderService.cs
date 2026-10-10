using ecommerce_app.backend.web.Common.Providers;
using Dapper;
using System.Data;

namespace ecommerce_app.backend.web.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OrderService(IDbConnection dbConnection,
            IDateTimeProvider dateTimeProvider)
        {
            _dbConnection = dbConnection;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Entities.Order> CreateAsync(Entities.Order order, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO orders (id, user_id, total_amount, status, payment_id, created, updated)
                VALUES (@Id, @UserId, @TotalAmount, @Status, @PaymentId, @Created, @Updated);";

            var command = new CommandDefinition(sql, order, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
            return order;
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
