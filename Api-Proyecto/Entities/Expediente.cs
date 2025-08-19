using System.ComponentModel.DataAnnotations;

namespace Api_Proyecto.Entities
{
    public class Expediente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MascotaId { get; set; }

        [Required]
        public DateTime FechaApertura { get; set; }

        public string? Observaciones { get; set; }

        // Propiedades de navegación - NO REQUERIDAS al crear el expediente
        public virtual Mascota? Mascota { get; set; }

        public virtual ICollection<Vacunacion>? Vacunaciones { get; set; }
    }
}