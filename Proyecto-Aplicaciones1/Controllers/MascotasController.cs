using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto_Aplicaciones1.Models;
using System.Net.Http;
using System.Text;

namespace Proyecto_Aplicaciones1.Controllers
{
    public class MascotasController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inyecta HttpClient configurado en Program.cs
        public MascotasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            // Asegúrate de configurar la URL base de tu API, ya sea aquí o en la configuración del cliente.
            _httpClient.BaseAddress = new Uri("https://localhost:7275/api/"); // <-- CAMBIA ESTO
        }
        public async Task<IActionResult> Index()
        {
            // 1. Llamar a la API para obtener la lista de clientes
            var response = await _httpClient.GetAsync("Clientes/ObtenerClientes"); // Endpoint de tu API
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(jsonString);

                // 2. Convertir la lista de clientes a un SelectList para el dropdown
                // Se usará el 'Id' del cliente como valor y 'Nombre' como texto visible.
                ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
            }
            else
            {
                // Manejar el error si no se pueden cargar los clientes
                ViewBag.Clientes = new SelectList(new List<Cliente>(), "Id", "Nombre");
                ModelState.AddModelError(string.Empty, "Error al cargar los clientes desde la API.");
            }
            return View();
        }
        // En tu Controlador Web
        public async Task<IActionResult> ListaMascotas()
        {
            // --- Llamada 1: Obtener TODAS las mascotas ---
            var mascotasResponse = await _httpClient.GetAsync("Mascotas");
            if (!mascotasResponse.IsSuccessStatusCode)
            {
                ViewBag.Mascotas = new List<MascotaListaDto>();
                return View("ListaMascotas");
            }
            var mascotasJson = await mascotasResponse.Content.ReadAsStringAsync();
            var listaMascotas = JsonConvert.DeserializeObject<List<Mascota>>(mascotasJson);


            // --- Llamada 2: Obtener TODOS los clientes ---
            var clientesResponse = await _httpClient.GetAsync("Clientes/ObtenerClientes");
            if (!clientesResponse.IsSuccessStatusCode)
            {
                // Si fallan los clientes, al menos muestra las mascotas con dueño desconocido
                ViewBag.Mascotas = listaMascotas.Select(m => new MascotaListaDto { /*...llena los campos...*/ NombreDueño = "Error al cargar" }).ToList();
                return View("ListaMascotas");
            }
            var clientesJson = await clientesResponse.Content.ReadAsStringAsync();
            var listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);

            // --- Unir los datos en memoria ---
            // Convertimos la lista de clientes a un diccionario para búsquedas súper rápidas
            var clientesDictionary = listaClientes.ToDictionary(c => c.Id);

            var resultadoFinal = listaMascotas.Select(mascota => new MascotaListaDto
            {
                Id = mascota.Id,
                Nombre = mascota.Nombre,
                Especie = mascota.Especie,
                Raza = mascota.Raza,
                Sexo = mascota.Sexo,
                FechaNacimiento = (DateTime)mascota.FechaNacimiento,
                // Buscamos el dueño en el diccionario. Si existe, tomamos su nombre.
                NombreDueño = clientesDictionary.TryGetValue(mascota.ClienteId, out var cliente)
                    ? $"{cliente.Nombre} {cliente.Apellido}"
                    : "Dueño no asignado"
            }).ToList();


            ViewBag.Mascotas = resultadoFinal;
            return View("ListaMascotas");
        }
        // POST: Recibe los datos del formulario y crea la mascota
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Mascota mascota)
        {
            
                // 1. Convertir el objeto Mascota a JSON
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(mascota),
                    Encoding.UTF8,
                    "application/json"
                );

                // 2. Llamar a la API para crear la mascota
                var response = await _httpClient.PostAsync("Mascotas/Crear", jsonContent); // Endpoint de tu API

                if (response.IsSuccessStatusCode)
                {
                    // Si fue exitoso, redirigir a la lista de mascotas (o donde prefieras)
                    return RedirectToAction("Index"); // Asumiendo que tienes una vista Index
                }
                else
                {
                    // Manejar el error si la API falla
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error de la API: {errorContent}");
                }
            

            // Si el modelo no es válido, recargar los clientes para el dropdown y mostrar la vista de nuevo
            await CargarClientesViewBag();
            return RedirectToAction("Index"); // Asumiendo que tienes una vista Index
        }
        // Método auxiliar para no repetir código
        private async Task CargarClientesViewBag()
        {
            var response = await _httpClient.GetAsync("Clientes/ObtenerClientes");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(jsonString);
                ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
            }
        }

    }
}
