using Microsoft.AspNetCore.Mvc;
using Proyecto_Aplicaciones1.Models;
using System.Text;
using System.Text.Json;

namespace Proyecto_Aplicaciones1.Controllers
{
    public class CuentaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CuentaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.Login, nameof(model.Login));

            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Por favor complete todos los campos.";
                TempData["ActiveTab"] = "login"; // Aseguramos que la pestaña de login esté activa si falla por campos vacíos
                return RedirectToAction("Index", "Home"); // O "Index", "Cuenta" si tu vista está en ese controlador
            }

            var loginDto = model.Login;

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7275/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                TempData["LoginSuccess"] = "¡Inicio de sesión exitoso! Bienvenido.";
                return RedirectToAction("Index", "DashBoard");
            }

            TempData["LoginError"] = "Credenciales inválidas. Por favor, inténtelo de nuevo.";
            TempData["ActiveTab"] = "login";
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword(string correo)
        {
            // URL de tu API REST (puede ser localhost o producción)
            var apiUrl = "https://localhost:7275/api/Auth/forgot-password";
            var payload = new { Correo = correo };
            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(new { success = true, message = "Correo enviado. Revisa tu bandeja de entrada." });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = error });
            }
        }
        [HttpPost]
        public async Task<IActionResult> RestablecerPassword(string token, string nuevaPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(nuevaPassword) || nuevaPassword.Length < 12)
            {
                return Json(new { icon = "warning", title = "Contraseña muy corta", message = "La contraseña debe tener al menos 12 caracteres." });
            }
            if (nuevaPassword != confirmPassword)
            {
                return Json(new { icon = "error", title = "Error", message = "Las contraseñas no coinciden." });
            }

            var apiUrl = "https://localhost:7275/api/Auth/reset-password";
            var payload = new { token = token, nuevaPassword = nuevaPassword };
            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { icon = "success", title = "¡Éxito!", message = "Contraseña actualizada exitosamente." });
            }
            else
            {
                return Json(new { icon = "error", title = "Error", message = "Hubo un error al actualizar la contraseña." });
            }
        }
        [HttpGet]
        public IActionResult Registro() => View();

        [HttpPost]
        public async Task<IActionResult> Registro(AuthViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.Register, nameof(model.Register));

            if (!ModelState.IsValid)
            {
    
                var errorMessages = ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

               
                return BadRequest(new { success = false, type = "validation", messages = errorMessages, title = "Errores de Validación" });
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model.Register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://localhost:7275/api/Auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "¡Usuario registrado con éxito!", title = "Registro Exitoso" });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { success = false, type = "api", message = $"Error al registrar: {errorContent}", title = "Error de Registro" });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { success = false, type = "network", message = $"No se pudo conectar con el servicio de autenticación: {ex.Message}", title = "Error de Conexión" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, type = "unexpected", message = $"Ocurrió un error inesperado: {ex.Message}", title = "Error Interno" });
            }
        }

    }
}
