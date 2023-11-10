using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PARCIAL1.Cliente_API;
using PARCIAL1.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace PARCIAL1.Controllers
{
    public class AlquilersController : Controller
    {
        private string apiUrl = "https://localhost:7128";

        private async Task<Tipovehiculo> ObtenerTipovehiculoAsync(int tipoVehiculoID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Tipovehiculos/{tipoVehiculoID}");

                if (response.IsSuccessStatusCode)
                {
                    var tipovehiculoJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Tipovehiculo>(tipovehiculoJson);
                }
                else
                {
                    Console.WriteLine($"Error al obtener el TipoVehiculo ID {tipoVehiculoID} desde la API: {response.StatusCode}");
                    return null;
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Obtener la lista de alquileres desde la API
                HttpResponseMessage responseAlquileres = await client.GetAsync("/api/Alquilers");

                if (responseAlquileres.IsSuccessStatusCode)
                {
                    var alquilersJson = await responseAlquileres.Content.ReadAsStringAsync();
                    var alquilers = JsonConvert.DeserializeObject<List<Alquiler>>(alquilersJson);

                    // Obtener la lista de clientes desde la API
                    HttpResponseMessage responseClientes = await client.GetAsync("/api/Clientes");
                    List<Cliente> clientes = new List<Cliente>();

                    if (responseClientes.IsSuccessStatusCode)
                    {
                        var clientesJson = await responseClientes.Content.ReadAsStringAsync();
                        clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);
                    }
                    else
                    {
                        // Maneja el error si la solicitud no fue exitosa
                        return Problem("No se pudieron obtener los clientes desde la API.");
                    }

                    // Obtener la lista de tipos de vehículos desde la API
                    HttpResponseMessage responseTiposVehiculo = await client.GetAsync("/api/Tipovehiculos");
                    List<Tipovehiculo> tiposDeVehiculo = new List<Tipovehiculo>();

                    if (responseTiposVehiculo.IsSuccessStatusCode)
                    {
                        var tiposVehiculoJson = await responseTiposVehiculo.Content.ReadAsStringAsync();
                        tiposDeVehiculo = JsonConvert.DeserializeObject<List<Tipovehiculo>>(tiposVehiculoJson);
                    }
                    else
                    {
                        // Maneja el error si la solicitud no fue exitosa
                        return Problem("No se pudieron obtener los tipos de vehículos desde la API.");
                    }

                    // Crear diccionarios para clientes, tipos de vehículos y tarifas por día
                    var clientesDictionary = clientes.ToDictionary(c => c.ClienteID, c => c.Nombre);
                    var tiposDeVehiculoDictionary = tiposDeVehiculo.ToDictionary(t => t.TipoVehiculoID, t => new {t.Nombre,});
                    var tiposDeVehiculoDictionary1 = tiposDeVehiculo.ToDictionary(t => t.TipoVehiculoID, t => new {t.TarifaPorDia });

                    // Pasar la información adicional a la vista usando ViewBag
                    ViewBag.Clientes = clientes.ToDictionary(c => c.ClienteID, c => c.Nombre);
                    ViewBag.TiposDeVehiculoID = tiposDeVehiculo.ToDictionary(t => t.TipoVehiculoID, t => t.Nombre);
                    ViewBag.TiposDeVehiculoTarifa = tiposDeVehiculo.ToDictionary(t => t.TipoVehiculoID, t => t.TarifaPorDia);

                    // Devolver la vista con la lista de alquileres
                    return View(alquilers);
                }
                else
                {
                    // Maneja el error si la solicitud no fue exitosa
                    return Problem("No se pudieron obtener los alquileres desde la API.");
                }
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            var alquilerDto = new AlquilerCrearDto();
            ViewBag.Clientes = new SelectList(ObtenerClientes(), "ClienteID", "Nombre");
            ViewBag.Tipovehiculos = new SelectList(ObtenerTipovehiculos(), "TipoVehiculoID", "Nombre");

            return View(alquilerDto);
        }
        private List<Cliente> ObtenerClientes()
        {
            // Crear una lista para almacenar los clientes
            List<Cliente> clientes = new List<Cliente>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Realizar una solicitud a la API para obtener la lista de clientes
                HttpResponseMessage response = client.GetAsync("/api/Clientes").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta y deserializarla a una lista de clientes
                    var clientesJson = response.Content.ReadAsStringAsync().Result;
                    clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);
                }
                else
                {
                    // Maneja el error de la forma que prefieras (lanzar una excepción, loggearlo, etc.)
                    // Por ejemplo, puedes registrar el error:
                    Console.WriteLine("Error al obtener la lista de clientes desde la API: " + response.StatusCode);
                }
            }
            return clientes;
        }
        private List<Tipovehiculo> ObtenerTipovehiculos()
        {
            // Crear una lista para almacenar los tipos de vehículos
            List<Tipovehiculo> tipovehiculos = new List<Tipovehiculo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Realizar una solicitud a la API para obtener la lista de tipos de vehículos
                HttpResponseMessage response = client.GetAsync("/api/Tipovehiculos").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta y deserializarla a una lista de tipos de vehículos
                    var tipovehiculosJson = response.Content.ReadAsStringAsync().Result;
                    tipovehiculos = JsonConvert.DeserializeObject<List<Tipovehiculo>>(tipovehiculosJson);
                }
                else
                {
                    // Maneja el error de la forma que prefieras (lanzar una excepción, loggearlo, etc.)
                    // Por ejemplo, puedes registrar el error:
                    Console.WriteLine("Error al obtener la lista de tipos de vehículos desde la API: " + response.StatusCode);
                }
            }
            return tipovehiculos;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> GetClienteNameById(int clienteId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"/api/Clientes/{clienteId}");

                if (response.IsSuccessStatusCode)
                {
                    var clienteJson = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<Cliente>(clienteJson);

                    return cliente.Nombre;
                }
                else
                {
                    // Maneja el error de la forma que prefieras (lanzar una excepción, devolver un valor predeterminado, etc.)
                    return null; // Devuelve null si no se puede obtener el nombre del cliente
                }
            }
        }
        public async Task<string> GetTipovehiculoNameById(int tipovehiculoId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Tipovehiculos/{tipovehiculoId}");

                if (response.IsSuccessStatusCode)
                {
                    var tipovehiculoJson = await response.Content.ReadAsStringAsync();
                    var tipovehiculo = JsonConvert.DeserializeObject<Tipovehiculo>(tipovehiculoJson);

                    return tipovehiculo.Nombre;
                }
                else
                {
                    // Maneja el error de la forma que prefieras (lanzar una excepción, devolver un valor predeterminado, etc.)
                    return null; // Devuelve null si no se puede obtener el nombre del cliente
                }
            }
        }
        public async Task<IActionResult> Create([Bind("ClienteID, TipoVehiculoID, FechaInicio, FechaFin")] AlquilerCrearDto alquilerDto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Obtener los valores de ClienteID y TipoVehiculoID desde la entrada del usuario
                    int clienteId = alquilerDto.ClienteID;
                    int tipovehiculoId = alquilerDto.TipoVehiculoID;

                    string clienteNombre = await GetClienteNameById(clienteId);
                    string tipovehiculoNombre = await GetTipovehiculoNameById(tipovehiculoId);

                    // Realizar una llamada a la API para obtener los datos del Cliente y del TipoVehiculo
                    var clienteResponse = await client.GetAsync($"/api/Clientes/{clienteId}");
                    var tipovehiculoResponse = await client.GetAsync($"/api/Tipovehiculos/{tipovehiculoId}");

                    if (clienteResponse.IsSuccessStatusCode && tipovehiculoResponse.IsSuccessStatusCode)
                    {
                        var clienteJson = await clienteResponse.Content.ReadAsStringAsync();
                        var tipovehiculoJson = await tipovehiculoResponse.Content.ReadAsStringAsync();

                        var cliente = JsonConvert.DeserializeObject<Cliente>(clienteJson);
                        var Tipovehiculo = JsonConvert.DeserializeObject<Tipovehiculo>(tipovehiculoJson);

                        // Calcular la duración del alquiler
                        TimeSpan duracion = alquilerDto.FechaFin - alquilerDto.FechaInicio;

                        // Calcular el monto total a cobrar
                        decimal montoCobro = Tipovehiculo.TarifaPorDia * (decimal)duracion.TotalDays;

                        // Actualizar el campo MontoCobro en alquilerDto
                        alquilerDto.MontoCobro = montoCobro;

                        // Serializar el objeto alquilerDto y enviarlo a la API
                        var alquilerJson = JsonConvert.SerializeObject(alquilerDto);
                        var content = new StringContent(alquilerJson, System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("/api/Alquilers", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error al crear el alquiler en la API.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al cargar los datos necesarios desde la API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }
            return View(alquilerDto);
        }
        private async Task<List<Cliente>> ObtenerClientesAsync()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (var client = new HttpClient())
            {
                // Configuración del cliente HTTP
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Llamada a la API para obtener la lista de clientes
                HttpResponseMessage response = await client.GetAsync("/api/Clientes");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialización de la respuesta
                    var clientesJson = await response.Content.ReadAsStringAsync();
                    clientes = JsonConvert.DeserializeObject<List<Cliente>>(clientesJson);
                }
                else
                {
                    // Manejo de errores
                    Console.WriteLine("Error al obtener la lista de clientes desde la API: " + response.StatusCode);
                }
            }
            return clientes;
        }
        private async Task<List<Tipovehiculo>> ObtenerTipovehiculosAsync()
        {
            List<Tipovehiculo> tipovehiculos = new List<Tipovehiculo>();

            using (var client = new HttpClient())
            {
                // Configuración del cliente HTTP
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Llamada a la API para obtener la lista de tipos de vehículos
                HttpResponseMessage response = await client.GetAsync("/api/Tipovehiculos");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialización de la respuesta
                    var tipovehiculosJson = await response.Content.ReadAsStringAsync();
                    tipovehiculos = JsonConvert.DeserializeObject<List<Tipovehiculo>>(tipovehiculosJson);
                }
                else
                {
                    // Manejo de errores
                    Console.WriteLine("Error al obtener la lista de tipos de vehículos desde la API: " + response.StatusCode);
                }
            }
            return tipovehiculos;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Alquilers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var alquilerJson = await response.Content.ReadAsStringAsync();
                    var alquiler = JsonConvert.DeserializeObject<AlquilerUpdateDto>(alquilerJson);

                    // Cargar las listas de clientes y tipos de vehículos
                    ViewBag.Clientes = new SelectList(await ObtenerClientesAsync(), "ClienteID", "Nombre");
                    ViewBag.Tipovehiculos = new SelectList(await ObtenerTipovehiculosAsync(), "TipoVehiculoID", "Nombre");

                    return View(alquiler);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el alquiler desde la API
                    return Problem("No se pudo obtener el alquiler desde la API.");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteID, TipoVehiculoID, FechaInicio, FechaFin")] AlquilerUpdateDto alquilerDto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Cargar las listas de clientes y tipos de vehículos
                ViewBag.Clientes = new SelectList(await ObtenerClientesAsync(), "ClienteID", "Nombre");
                ViewBag.Tipovehiculos = new SelectList(await ObtenerTipovehiculosAsync(), "TipoVehiculoID", "Nombre");

                var alquilerJson = JsonConvert.SerializeObject(alquilerDto);
                var content = new StringContent(alquilerJson, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"/api/Alquilers/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo actualizar el alquiler
                    ModelState.AddModelError(string.Empty, "Error al actualizar el alquiler en la API.");
                }
            }
            return View(alquilerDto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/api/Alquilers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var alquilerJson = await response.Content.ReadAsStringAsync();
                    var alquiler = JsonConvert.DeserializeObject<Alquiler>(alquilerJson);
                    return View(alquiler);
                }
                else
                {
                    // Maneja el error si no se pudo obtener el alquiler desde la API
                    return Problem("No se pudo obtener el alquiler desde la API.");
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

                HttpResponseMessage response = await client.DeleteAsync($"/api/Alquilers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Maneja el error si la API no pudo eliminar el alquiler
                    return Problem("Error al eliminar el alquiler en la API.");
                }
            }
        }
    }
}
