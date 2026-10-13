namespace ecommerce_app.backend.web.Models.Payment
{
    public class YooKassaPaymentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public YooKassaAmount Amount { get; set; } = new();
        public YooKassaConfirmation? Confirmation { get; set; }
    }
}
