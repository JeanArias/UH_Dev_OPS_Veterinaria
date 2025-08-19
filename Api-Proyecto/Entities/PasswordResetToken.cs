namespace Api_Proyecto.Entities
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public bool Usado { get; set; }

        public Usuario Usuario { get; set; }
    }
}
