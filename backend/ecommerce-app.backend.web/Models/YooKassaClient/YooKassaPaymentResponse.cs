using System.Text.Json.Serialization;

namespace ecommerce_app.backend.web.Models.YooKassaClient
{
    public class YooKassaPaymentResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public YooKassaAmount Amount { get; set; } = new();
        [JsonPropertyName("confirmation")]
        public YooKassaConfirmation? Confirmation { get; set; }
    }
}
