namespace Api_Proyecto.Entities
{
    // En tu carpeta DTOs del proyecto de API
    public class MascotaEditarDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public int ClienteId { get; set; }
    }
}
