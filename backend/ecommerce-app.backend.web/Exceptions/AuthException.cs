namespace ecommerce_app.backend.web.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException() : this("Unauthorized")
        {

        }

        public AuthException(string message) : base(message)
        {

        }

        public AuthException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
