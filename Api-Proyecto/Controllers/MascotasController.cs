using Api_Proyecto.Data;
using Api_Proyecto.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasController : Controller
    {
        private readonly AppDbContext _context;
        public MascotasController(AppDbContext context) => _context = context;

        [HttpPost("Crear")]
        // CAMBIO: Aceptamos el DTO en lugar de la entidad completa
        public async Task<IActionResult> Crear([FromBody] MascotaCrearDto mascotaDto)
        {
            // Creamos una nueva entidad Mascota a partir de los datos del DTO
            var nuevaMascota = new Mascota
            {
                Nombre = mascotaDto.Nombre,
                Especie = mascotaDto.Especie,
                Raza = mascotaDto.Raza,
                FechaNacimiento = mascotaDto.FechaNacimiento,
                Sexo = mascotaDto.Sexo,
                ClienteId = mascotaDto.ClienteId
            };

            _context.Mascotas.Add(nuevaMascota);
            await _context.SaveChangesAsync();

            // Devolvemos la entidad completa que se creó (o un Ok simple)
            return Ok(nuevaMascota);
        }

        // Consultar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mascotas = await _context.Mascotas.ToListAsync();
            return Ok(mascotas);
        }

        // Consultar por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null) return NotFound();
            return Ok(mascota);
        }

        // En tu API: MascotasController.cs

        [HttpPut("{id}")]
        // CAMBIO: Aceptamos el DTO en lugar de la entidad completa
        public async Task<IActionResult> Edit(int id, [FromBody] MascotaEditarDto mascotaDto)
        {
            // Verificamos que los IDs coincidan
            if (id != mascotaDto.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            // 1. Buscar la mascota existente en la base de datos.
            var mascotaExistente = await _context.Mascotas.FindAsync(id);
            if (mascotaExistente == null)
            {
                return NotFound("La mascota no fue encontrada.");
            }

            // 2. Actualizar las propiedades de la entidad existente con los valores del DTO.
            //    Este es el método más seguro, ya que solo modificas lo que necesitas.
            mascotaExistente.Nombre = mascotaDto.Nombre;
            mascotaExistente.Especie = mascotaDto.Especie;
            mascotaExistente.Raza = mascotaDto.Raza;
            mascotaExistente.FechaNacimiento = mascotaDto.FechaNacimiento;
            mascotaExistente.Sexo = mascotaDto.Sexo;
            mascotaExistente.ClienteId = mascotaDto.ClienteId;

            try
            {
                // 3. Guardar los cambios en la base de datos.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Esto es para manejar casos de concurrencia, es una buena práctica tenerlo.
                if (!_context.Mascotas.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retornar 204 No Content es un estándar para operaciones PUT exitosas.
        }

        // Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null) return NotFound();
            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
