using System.Text.Json.Serialization;

namespace ecommerce_app.backend.web.Models.YooKassaClient
{
    public class YooKassaAmount
    {
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "RUB";
    }
}
