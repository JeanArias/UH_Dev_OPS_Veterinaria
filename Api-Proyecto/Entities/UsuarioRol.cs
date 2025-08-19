using System.ComponentModel.DataAnnotations;

namespace Api_Proyecto.Entities
{
    public class UsuarioRol
    {
        [Key]
        public int UsuarioId { get; set; }
        public int RolId { get; set; }

        public Usuario Usuario { get; set; }
        public Rol Rol { get; set; }
    }
}
