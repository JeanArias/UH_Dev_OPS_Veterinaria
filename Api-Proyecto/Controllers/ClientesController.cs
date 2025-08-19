using Api_Proyecto.Data;
using Api_Proyecto.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ClientesController(AppDbContext context) => _context = context;

    // Crear Cliente

    [HttpPost("agregarCliente")]
    public async Task<IActionResult> Crear([FromBody] Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return Ok(cliente);
    }

    // Consultar todos
    [HttpGet("ObtenerClientes")]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return Ok(clientes);
    }

    // Consultar por Id
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }

    // Editar
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [FromBody] Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(cliente);
    }

    // Eliminar
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return Ok();
    }
}