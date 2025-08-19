using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Correo { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Apellidos { get; set; }

    [Required]
    public string Contraseña { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public bool Estado { get; set; } = true;
}
