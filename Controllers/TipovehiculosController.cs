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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Tipovehiculos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var TipovehiculoJson = await response.Content.ReadAsStringAsync();
                    var Tipovehiculo = JsonConvert.DeserializeObject<TipovehiculoUpdateDto>(TipovehiculoJson); // Cambiado a TipovehiculoUpdateDto
                    return View(Tipovehiculo);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el tipo de vehículo desde la API
                    return Problem("No se pudo obtener el tipo de vehículo desde la API.");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre, Descripcion")] TipovehiculoUpdateDto tipoVehiculoDto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var TipovehiculoJson = JsonConvert.SerializeObject(tipoVehiculoDto);
                var content = new StringContent(TipovehiculoJson, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"/api/Tipovehiculos/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo actualizar el tipo de vehículo
                    ModelState.AddModelError(string.Empty, "Error al actualizar el tipo de vehículo en la API.");
                }
            }

            return View(tipoVehiculoDto);
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

