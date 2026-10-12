namespace ecommerce_app.backend.web.Models.Payment
{
    public class YooKassaWebhookNotification
    {
        public string Event { get; set; } = string.Empty;
        public YooKassaWebhookObject Object { get; set; } = new();
    }
}
