using System.ComponentModel.DataAnnotations;

namespace PARCIAL1.Models
{
    public class TipovehiculoSelectDto
    {
        public int TipoVehiculoID { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal TarifaPorDia { get; set; }
    }
}
