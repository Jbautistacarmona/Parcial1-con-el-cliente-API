using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
//using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace PARCIAL1.Cliente_API;
public class ApiClient
{
    private HttpClient _httpClient;
    private string _baseUri;

    public ApiClient(string baseUri)
    {
        _baseUri = baseUri;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetDatosAsync(string ruta)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUri + ruta);

            if (response.IsSuccessStatusCode)
            {
                string contenido = await response.Content.ReadAsStringAsync();
                return contenido;
            }
            else
            {
                Console.WriteLine("Error al obtener datos de la API. Código de estado HTTP: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error de conexión: " + ex.Message);
            return null;
        }
    }

    public async Task<string> PostDatosAsync(string ruta, string datos)
    {
        try
        {
            StringContent contenido = new StringContent(datos, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_baseUri + ruta, contenido);

            if (response.IsSuccessStatusCode)
            {
                string contenidoRespuesta = await response.Content.ReadAsStringAsync();
                return contenidoRespuesta;
            }
            else
            {
                Console.WriteLine("Error al crear datos en la API. Código de estado HTTP: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error de conexión: " + ex.Message);
            return null;
        }
    }

    public async Task<string> PutDatosAsync(string ruta, string datos)
    {
        try
        {
            StringContent contenido = new StringContent(datos, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(_baseUri + ruta, contenido);

            if (response.IsSuccessStatusCode)
            {
                string contenidoRespuesta = await response.Content.ReadAsStringAsync();
                return contenidoRespuesta;
            }
            else
            {
                Console.WriteLine("Error al actualizar datos en la API. Código de estado HTTP: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error de conexión: " + ex.Message);
            return null;
        }
    }

    public async Task<string> DeleteDatosAsync(string ruta)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUri + ruta);

            if (response.IsSuccessStatusCode)
            {
                string contenidoRespuesta = await response.Content.ReadAsStringAsync();
                return contenidoRespuesta;
            }
            else
            {
                Console.WriteLine("Error al eliminar datos en la API. Código de estado HTTP: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error de conexión: " + ex.Message);
            return null;
        }
    }
}