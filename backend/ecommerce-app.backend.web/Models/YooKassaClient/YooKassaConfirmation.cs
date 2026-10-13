namespace ecommerce_app.backend.web.Models.YooKassaClient
{
    public class YooKassaConfirmation
    {
        public string Type { get; set; } = "redirect";
        public string ConfirmationUrl { get; set; } = string.Empty;
    }
}
