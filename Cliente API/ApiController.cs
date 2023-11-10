using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PARCIAL1.Cliente_API
{
    public class ApiController : Controller
    {
        private string baseUri = "https://localhost:7128"; // URL de tu API
        private HttpClient httpClient;

        public ApiController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri);
        }

        public async Task<ActionResult> Index()
        {
            // Agrega el encabezado Content-Type de "application/json" a todas las solicitudes
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Realiza operaciones CRUD para Alquilers, Clientes y Tipovehiculoes aquí

            // Ejemplo: Obtener todos los alquileres
            string alquilersJson = await httpClient.GetStringAsync("/api/Alquilers");

            // Ejemplo: Crear un nuevo alquiler
            string nuevoAlquilerJson = @"{
              //""alquilerID"": 0,
              ""clienteID"": 0,
              ""tipoVehiculoID"": 0,
              ""fechaInicio"": ""2023-11-06T03:12:17.833Z"",
              ""fechaFin"": ""2023-11-06T03:12:17.833Z"",
              ""montoCobro"": 0
            }";

            // Ejemplo: Actualizar un alquiler existente con ID 1
            int alquilerIdActualizar = 1;
            string alquilerActualizadoJson = @"{
              ""clienteID"": 0,
              ""tipoVehiculoID"": 0,
              ""fechaInicio"": ""2023-11-05T22:30:25.758Z"",
              ""fechaFin"": ""2023-11-05T22:30:25.758Z"",
              ""montoCobro"": 0
            }";

            // Ejemplo: Eliminar un alquiler existente
            int alquilerIdEliminar = 2;

            // Obtener todos los clientes
            string clientesJson = await httpClient.GetStringAsync("/api/Clientes");

            // Crear un nuevo cliente
            string nuevoClienteJson = @"{
              //""clienteID"": 0,
              ""nombre"": ""string"",
              ""email"": ""string"",
              ""telefono"": ""string""
            }";

            // Ejemplo: Actualizar un cliente existente
            int clienteIdActualizar = 1;
            string clienteActualizadoJson = @"{
              ""nombre"": ""string"",
              ""email"": ""string"",
              ""telefono"": ""string""
            }";

            // Eliminar un cliente existente
            int clienteIdEliminar = 2;

            // Operaciones CRUD para Tipovehiculos

            // Obtener todos los Tipovehiculos
            string tipovehiculoJson = await httpClient.GetStringAsync("/api/Tipovehiculos");

            // Crear un nuevo Tipovehiculos
            string nuevoTipovehiculoJson = @"{
              //""tipoVehiculoID"": 0,
              ""nombre"": ""string"",
              ""tarifaPorDia"": 0
            }";

            // Actualizar un Tipovehiculos existente
            int tipovehiculoIdActualizar = 1;
            string tipovehiculoActualizadoJson = @"{
              ""nombre"": ""string"",
              ""tarifaPorDia"": 0
            }";

            // Eliminar un Tipovehiculos existente
            int tipovehiculoIdEliminar = 2;

            // Procesa los datos y respuestas como desees
            return View();
        }
    }
}