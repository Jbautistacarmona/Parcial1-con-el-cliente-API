namespace PARCIAL1.Models
{
    public class AlquilerUpdateDto
    {
        public int AlquilerID { get; set; }
        public int ClienteID { get; set; }
        public int TipoVehiculoID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoCobro { get; set; } // Asegúrate de que el tipo de dato sea el correcto
    }
}