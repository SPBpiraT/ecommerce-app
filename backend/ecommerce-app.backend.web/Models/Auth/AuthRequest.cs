using System.ComponentModel.DataAnnotations;

namespace ecommerce_app.backend.web.Models.Auth
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
