using ecommerce_app.backend.web.Common.Configuration;
using ecommerce_app.backend.web.Models.YooKassaClient;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace ecommerce_app.backend.web.Services.YooKassaClient
{
    public class YooKassaClient : IYooKassaClient
    {
        private readonly HttpClient _httpClient;
        private readonly YooKassaSettings _settings;

        public YooKassaClient(HttpClient httpClient, IOptions<YooKassaSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<YooKassaPaymentResponse?> CreatePaymentAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default)
        {
            var requestBody = new
            {
                amount = new
                {
                    value = amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    currency = "RUB"
                },
                capture = true,
                confirmation = new
                {
                    type = "redirect",
                    return_url = $"https://localhost:7008/order/success"
                },
                description = $"Payment for Order #{orderId}"
            };

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ShopId}:{_settings.SecretKey}"));

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.yookassa.ru/v3/payments");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            request.Headers.Add("Idempotence-Key", Guid.NewGuid().ToString());

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception($"YooKassa API Error: {response.StatusCode} - {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<YooKassaPaymentResponse>(cancellationToken: cancellationToken);
        }
    }
}
