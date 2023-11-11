using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PARCIAL1.Cliente_API;
using PARCIAL1.Models;

namespace PARCIAL1.Controllers
{
    public class ClientesController : Controller
    {
        private string apiUrl = "https://localhost:7128";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Clientes");

                if (response.IsSuccessStatusCode)
                {
                    var clientesJson = await response.Content.ReadAsStringAsync();
                    var clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);
                    return View(clientes);
                }
                else
                {
                    // Maneja el error si la solicitud no fue exitosa
                    return Problem("No se pudieron obtener los clientes desde la API.");
                }
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Renderiza la vista para crear un nuevo cliente
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre, Email, Telefono")] ClienteCrearDto clienteDto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var clienteJson = JsonConvert.SerializeObject(clienteDto);
                var content = new StringContent(clienteJson, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/Clientes", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo crear el cliente
                    ModelState.AddModelError(string.Empty, "Error al crear el cliente en la API.");
                }
            }

            return View(clienteDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await ObtenerClienteAsync(id);

            if (cliente != null)
            {
                return View(cliente);
            }
            else
            {
                // Maneja el error si no se pudo obtener el cliente desde la API
                return Problem("No se pudo obtener el cliente desde la API.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteID, Nombre, Email, Telefono")] ClienteUpdateDto clienteDto)
        {
            try
            {
                // Verificación adicional antes de la serialización
                if (clienteDto.ClienteID <= 0)
                {
                    ModelState.AddModelError(nameof(clienteDto.ClienteID), "El ID del cliente no es válido.");
                    return View(clienteDto);
                }

                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var clienteJson = JsonConvert.SerializeObject(clienteDto);
                        var content = new StringContent(clienteJson, System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync($"/api/Clientes/{id}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            // Mensaje de depuración
                            System.Diagnostics.Debug.WriteLine($"Cliente actualizado correctamente. ID: {id}");

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // Mensaje de depuración
                            System.Diagnostics.Debug.WriteLine($"Error al actualizar el cliente. Código de estado: {response.StatusCode}");

                            var errorContent = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine($"Contenido del error: {errorContent}");

                            ModelState.AddModelError(string.Empty, "Error al actualizar el cliente en la API.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Mensaje de depuración
                System.Diagnostics.Debug.WriteLine($"Excepción al actualizar el cliente: {ex.Message}");

                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            // Si llegamos a este punto, algo salió mal, vuelve a la vista con el modelo
            return View(clienteDto);
        }

        private async Task<ClienteUpdateDto> ObtenerClienteAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Clientes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var clienteJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ClienteUpdateDto>(clienteJson);
                }
                else
                {
                    return null;
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Clientes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var clienteJson = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<Cliente>(clienteJson);
                    return View(cliente);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el cliente desde la API
                    return Problem("No se pudo obtener el cliente desde la API.");
                }
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"/api/Clientes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo eliminar el cliente
                    return Problem("Error al eliminar el cliente en la API.");
                }
            }
        }
    }
}
