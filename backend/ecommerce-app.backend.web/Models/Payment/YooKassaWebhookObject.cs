namespace ecommerce_app.backend.web.Models.Payment
{
    public class YooKassaWebhookObject
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool Paid { get; set; }
    }
}
