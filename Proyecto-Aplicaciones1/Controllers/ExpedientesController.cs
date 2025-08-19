using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Aplicaciones1.Models;
using System.Text;

namespace Proyecto_Aplicaciones1.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ExpedientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7275/api/");
        }

        // Vista principal con formulario vacío y expedientes cargados
        public async Task<IActionResult> Index()
        {
            var expedientes = await ObtenerExpedientesDesdeAPI();
            ViewBag.Expedientes = expedientes;
            return View(new Expediente()); // Modelo vacío para el formulario
        }

        // Acción POST que guarda el expediente en la API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Expediente expediente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Expedientes = await ObtenerExpedientesDesdeAPI();
                return View(expediente);
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(expediente), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Expedientes", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error al crear expediente: {error}");

            ViewBag.Expedientes = await ObtenerExpedientesDesdeAPI();
            return View(expediente);
        }

        // Acción auxiliar para recargar solo la tabla de expedientes
        public async Task<IActionResult> ListaExpediente()
        {
            var expedientes = await ObtenerExpedientesDesdeAPI();
            ViewBag.Expedientes = expedientes;
            return View("Index", new Expediente()); // Redirige a Index con modelo vacío
        }

        // Método auxiliar para obtener los expedientes desde la API
        private async Task<List<Expediente>> ObtenerExpedientesDesdeAPI()
        {
            var response = await _httpClient.GetAsync("Expedientes");

            if (!response.IsSuccessStatusCode)
                return new List<Expediente>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Expediente>>(json);
        }
    }
}