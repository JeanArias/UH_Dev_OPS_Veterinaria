using Microsoft.AspNetCore.Mvc;
using Proyecto_Aplicaciones1.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Proyecto_Aplicaciones1.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: /ClientesWeb/ListaClientes
        public async Task<IActionResult> ListaClientes()
        {
            var httpClient = _httpClientFactory.CreateClient();

            // ¡Asegúrate que la URL de tu API sea la correcta!
            var response = await httpClient.GetAsync("https://localhost:7275/api/Clientes/ObtenerClientes");

            if (response.IsSuccessStatusCode)
            {
                // Leemos el contenido de la respuesta y lo convertimos a una lista de clientes
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonSerializer.Deserialize<List<Cliente>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Pasamos la lista de clientes a la vista a través del ViewBag
                ViewBag.Clientes = clientes;
            }
            else
            {
                // Si hay un error, pasamos una lista vacía para evitar errores en la vista
                ViewBag.Clientes = new List<Cliente>();
                // Opcional: puedes pasar un mensaje de error también
                TempData["ErrorMessage"] = "No se pudieron cargar los datos de los clientes.";
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Cliente cliente)
        {
            // 1. Validar el modelo
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, usa TempData y redirige
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                return RedirectToAction("Agregar");
            }

            // 2. Preparar la llamada a la API
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(cliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 3. Realizar la petición POST a la API de Clientes
            // ¡Ajusta la URL a la de tu proyecto!
            var response = await client.PostAsync("https://localhost:7275/api/Clientes/agregarCliente", content);

            // 4. Procesar la respuesta de la API
            if (response.IsSuccessStatusCode)
            {
                // Éxito: lee el contenido (aunque no se use), establece TempData y redirige
                var data = await response.Content.ReadAsStringAsync(); // Coincide con tu ejemplo
                TempData["SuccessMessage"] = "¡Cliente agregado con éxito!";
                return RedirectToAction("Index"); // Redirige a la lista de clientes o al Dashboard
            }

            // Error: establece TempData y redirige de vuelta al formulario
            TempData["ErrorMessage"] = "Error al guardar el cliente. Por favor, inténtelo de nuevo.";
            return RedirectToAction("Agregar");
        }
    }
}
