using System.ComponentModel.DataAnnotations;

namespace Proyecto_Aplicaciones1.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es requerido.")]
        public string username { get; set; }


        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos {2} y un máximo de {1} caracteres.")]
        [DataType(DataType.Password)]
        public string contraseña { get; set; }
    }
}
