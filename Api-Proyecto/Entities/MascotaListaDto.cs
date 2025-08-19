namespace Api_Proyecto.Entities
{
    // Puedes crear una carpeta DTOs en tu proyecto de API
    public class MascotaListaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NombreDueño { get; set; } // Campo para el nombre del dueño
    }
}
