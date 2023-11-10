using System.ComponentModel.DataAnnotations;

namespace PARCIAL1.Models
{
    public class TipovehiculoUpdateDto
    {
        public int TipoVehiculoID { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal TarifaPorDia { get; set; }
    }
}
