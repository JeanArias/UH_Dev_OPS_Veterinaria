namespace Api_Proyecto.Entities
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string NuevaPassword { get; set; }
    }
}
