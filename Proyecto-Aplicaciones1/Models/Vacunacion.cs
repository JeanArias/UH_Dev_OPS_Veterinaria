namespace Proyecto_Aplicaciones1.Models
{
    public class Vacunacion
    {
        public int Id { get; set; }
        public int ExpedienteId { get; set; }
        public DateTime FechaVacuna { get; set; }
        public string TipoVacuna { get; set; }
        public string Observaciones { get; set; }

        // Alias para que coincidan con las vistas heredadas del módulo de mascotas
        public string NombreVacuna => TipoVacuna;
        public DateTime Fecha => FechaVacuna;
        public string Descripcion => Observaciones;
        public int MascotaId => ExpedienteId; // <- Este es el alias que soluciona el error actual
    }
}