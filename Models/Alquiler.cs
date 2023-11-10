using PARCIAL1.Models;

namespace PARCIAL1.Models
{
    public class Alquiler
    {
        public int AlquilerID { get; set; }
        public int ClienteID { get; set; }
        public int TipoVehiculoID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoCobro { get; set; } // Asegúrate de que el tipo de dato sea el correcto

        // Propiedades de navegación para establecer relaciones
        public Cliente? Cliente { get; set; } = null;
        public Tipovehiculo? Tipovehiculo { get; set; } = null;
        public Tipovehiculo? TafiraporDia { get; set; } = null;
    }
}


