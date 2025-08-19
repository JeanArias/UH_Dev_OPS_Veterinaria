using System.ComponentModel.DataAnnotations;

namespace Proyecto_Aplicaciones1.Models
{
    public class Mascota
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Especie { get; set; }
        [Required]
        public string Raza { get; set; }
        [Required]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        //public ICollection<Expediente> Expedientes { get; set; }
    }
}
