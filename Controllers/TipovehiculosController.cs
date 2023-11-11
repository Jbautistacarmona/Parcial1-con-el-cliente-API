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
    public class TipovehiculosController : Controller
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

                HttpResponseMessage response = await client.GetAsync("/api/Tipovehiculos");

                if (response.IsSuccessStatusCode)
                {
                    var TipovehiculoJson = await response.Content.ReadAsStringAsync();
                    var Tipovehiculo = JsonConvert.DeserializeObject<List<Tipovehiculo>>(TipovehiculoJson);
                    return View(Tipovehiculo);
                }
                else
                {
                    // Maneja el error si la solicitud no fue exitosa
                    return Problem("No se pudieron obtener los tipos de vehículos desde la API.");
                }
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Renderiza la vista para crear un nuevo tipo de vehículo
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre, TarifaPorDia")] TipovehiculoCrearDto tipoVehiculoDto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var TipovehiculoJson = JsonConvert.SerializeObject(tipoVehiculoDto);
                var content = new StringContent(TipovehiculoJson, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/api/Tipovehiculos", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo crear el tipo de vehículo
                    ModelState.AddModelError(string.Empty, "Error al crear el tipo de vehículo en la API.");
                }
            }

            return View(tipoVehiculoDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tipovehiculo = await ObtenerTipovehiculoAsync(id);

            if (tipovehiculo != null)
            {
                return View(tipovehiculo);
            }
            else
            {
                // Maneja el error si no se pudo obtener el cliente desde la API
                return Problem("No se pudo obtener el Tipo de vehiculo desde la API.");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoVehiculoID, Nombre, TarifaPorDia")] TipovehiculoUpdateDto tipoVehiculoDto)
        {
            try
            {
                // Verificación adicional antes de la serialización
                if (tipoVehiculoDto.TipoVehiculoID <= 0)
                {
                    ModelState.AddModelError(nameof(tipoVehiculoDto.TipoVehiculoID), "El ID del cliente no es válido.");
                    return View(tipoVehiculoDto);
                }

                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var clienteJson = JsonConvert.SerializeObject(tipoVehiculoDto);
                        var content = new StringContent(clienteJson, System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync($"/api/Tipovehiculos/{id}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            // Mensaje de depuración
                            System.Diagnostics.Debug.WriteLine($"Tipovehiculo actualizado correctamente. ID: {id}");

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            // Mensaje de depuración
                            System.Diagnostics.Debug.WriteLine($"Error al actualizar el Tipovehiculo. Código de estado: {response.StatusCode}");

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
                System.Diagnostics.Debug.WriteLine($"Excepción al actualizar el tipovehiculo: {ex.Message}");

                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            // Si llegamos a este punto, algo salió mal, vuelve a la vista con el modelo
            return View(tipoVehiculoDto);
        }
        private async Task<TipovehiculoUpdateDto> ObtenerTipovehiculoAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Tipovehiculos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var clienteJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TipovehiculoUpdateDto>(clienteJson);
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

                HttpResponseMessage response = await client.GetAsync($"/api/Tipovehiculos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var TipovehiculoJson = await response.Content.ReadAsStringAsync();
                    var Tipovehiculo = JsonConvert.DeserializeObject<Tipovehiculo>(TipovehiculoJson);
                    return View(Tipovehiculo);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el tipo de vehículo desde la API
                    return Problem("No se pudo obtener el tipo de vehículo desde la API.");
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

                try
                {
                    HttpResponseMessage response = await client.DeleteAsync($"/api/Tipovehiculos/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Maneja el error si la API no pudo eliminar el tipo de vehículo
                        return Problem("Error al eliminar el tipo de vehículo en la API.");
                    }
                }
                catch (Exception ex)
                {
                    // Loguea la excepción para obtener más detalles
                    Console.WriteLine($"Excepción al eliminar el tipo de vehículo: {ex.Message}");
                    return Problem("Error al eliminar el tipo de vehículo en la API.");
                }
            }
        }
    }
}

