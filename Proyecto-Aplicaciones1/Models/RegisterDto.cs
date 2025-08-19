using System.ComponentModel.DataAnnotations;

namespace Proyecto_Aplicaciones1.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos {2} y un máximo de {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        [Display(Name = "Confirmar Contraseña")] // <--- Añade esto
        public string ConfirmarContraseña { get; set; }
    }
}
