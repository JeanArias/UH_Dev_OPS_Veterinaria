using Api_Proyecto.Data;
using Api_Proyecto.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Proyecto.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExpedientesController : Controller
    {
        private readonly AppDbContext _context;
        public ExpedientesController(AppDbContext context) => _context = context;

        // Crear Expediente
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Expediente expediente)
        {
            _context.Expedientes.Add(expediente);
            await _context.SaveChangesAsync();
            return Ok(expediente);
        }

        // Consultar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expedientes = await _context.Expedientes.ToListAsync();
            return Ok(expedientes);
        }

        // Consultar por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            return Ok(expediente);
        }

        // Editar
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Expediente expediente)
        {
            if (id != expediente.Id) return BadRequest();
            _context.Entry(expediente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(expediente);
        }

        // Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
