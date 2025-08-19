using Api_Proyecto.Data;
using Api_Proyecto.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunacionesController : Controller
    {
        private readonly AppDbContext _context;
        public VacunacionesController(AppDbContext context) => _context = context;

        // Crear Vacunacion
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Vacunacion vacunacion)
        {
            _context.Vacunaciones.Add(vacunacion);
            await _context.SaveChangesAsync();
            return Ok(vacunacion);
        }

        // Consultar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vacunaciones = await _context.Vacunaciones.ToListAsync();
            return Ok(vacunaciones);
        }

        // Consultar por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vacunacion = await _context.Vacunaciones.FindAsync(id);
            if (vacunacion == null) return NotFound();
            return Ok(vacunacion);
        }

        // Editar
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Vacunacion vacunacion)
        {
            if (id != vacunacion.Id) return BadRequest();
            _context.Entry(vacunacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(vacunacion);
        }

        // Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacunacion = await _context.Vacunaciones.FindAsync(id);
            if (vacunacion == null) return NotFound();
            _context.Vacunaciones.Remove(vacunacion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
