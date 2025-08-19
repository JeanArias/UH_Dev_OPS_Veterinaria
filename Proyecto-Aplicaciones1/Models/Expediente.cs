namespace Proyecto_Aplicaciones1.Models
{
    public class Expediente
    {
        public int Id { get; set; }
        public int MascotaId { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Observaciones { get; set; }
    }
}