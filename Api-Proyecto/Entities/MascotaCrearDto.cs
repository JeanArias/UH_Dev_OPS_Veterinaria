namespace Api_Proyecto.Entities
{

    public class MascotaCrearDto
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public int ClienteId { get; set; }
    }
}
