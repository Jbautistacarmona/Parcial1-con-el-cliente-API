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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Clientes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var clienteJson = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<ClienteUpdateDto>(clienteJson);
                    return View(cliente);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el cliente desde la API
                    return Problem("No se pudo obtener el cliente desde la API.");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nombre, Email, Telefono")] ClienteUpdateDto clienteDto)
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
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo actualizar el cliente
                    ModelState.AddModelError(string.Empty, "Error al actualizar el cliente en la API.");
                }
            }
            return View(clienteDto);
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
