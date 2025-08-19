using System.ComponentModel.DataAnnotations;

namespace Api_Proyecto.Entities
{
    public class Vacunacion
    {
        [Key]
        public int Id { get; set; }
        public int ExpedienteId { get; set; }
        public Expediente? Expediente { get; set; } 
        public DateTime FechaVacuna { get; set; }
        public string TipoVacuna { get; set; }
        public string Observaciones { get; set; }
    }
}
