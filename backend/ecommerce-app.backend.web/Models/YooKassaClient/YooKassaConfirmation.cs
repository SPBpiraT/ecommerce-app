using System.Text.Json.Serialization;

namespace ecommerce_app.backend.web.Models.YooKassaClient
{
    public class YooKassaConfirmation
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "redirect";
        [JsonPropertyName("confirmation_url")]
        public string ConfirmationUrl { get; set; } = string.Empty;
    }
}
