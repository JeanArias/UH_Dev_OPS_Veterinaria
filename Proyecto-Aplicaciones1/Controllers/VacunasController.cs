using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Aplicaciones1.Models;
using System.Text;

namespace Proyecto_Aplicaciones1.Controllers
{
    public class VacunasController : Controller
    {
        private readonly HttpClient _httpClient;

        public VacunasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7275/api/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListaVacunas()
        {
            var response = await _httpClient.GetAsync("Vacunaciones");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Vacunas = new List<Vacunacion>();
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var vacunas = JsonConvert.DeserializeObject<List<Vacunacion>>(json);
            ViewBag.Vacunas = vacunas;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Vacunacion vacuna)
        {
            var json = JsonConvert.SerializeObject(vacuna);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Vacunaciones", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListaVacunas");
            }

            var apiError = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error al guardar la vacuna. Detalle: {apiError}");
            return View("Index");
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Vacunacion vacuna)
        {
            var json = JsonConvert.SerializeObject(vacuna);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Vacunaciones", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListaVacunas");
            }

            var apiError = await response.Content.ReadAsStringAsync(); // 👈 MOSTRAR EL ERROR
            ModelState.AddModelError(string.Empty, $"Error al guardar la vacuna. Detalle: {apiError}");
            return View("Index");
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Vacunacion vacuna)
        {
            var json = JsonConvert.SerializeObject(vacuna);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Vacunaciones", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListaVacunas");
            }

            ModelState.AddModelError(string.Empty, "Error al guardar la vacuna.");
            return View("Index");
        }
        */
    }
}