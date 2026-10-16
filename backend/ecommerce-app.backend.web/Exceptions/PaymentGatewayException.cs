namespace ecommerce_app.backend.web.Exceptions
{
    public class PaymentGatewayException : Exception
    {
        public PaymentGatewayException() : this("Payment gateway error.")
        { 

        }

        public PaymentGatewayException(string message) : base(message)
        {
            
        }

        public PaymentGatewayException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
