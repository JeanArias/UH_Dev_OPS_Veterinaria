namespace Proyecto_Aplicaciones1.Models
{
    public class AuthViewModel
    {
        public LoginDto Login { get; set; } = new();
        public RegisterDto Register { get; set; } = new();
    }
}
