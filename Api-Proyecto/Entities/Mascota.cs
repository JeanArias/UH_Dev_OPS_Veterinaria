using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Proyecto.Entities
{
    public class Mascota
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public int ClienteId { get; set; }
        [NotMapped]
        public Cliente Cliente { get; set; }
        //public ICollection<Expediente> Expedientes { get; set; }
    }
}
