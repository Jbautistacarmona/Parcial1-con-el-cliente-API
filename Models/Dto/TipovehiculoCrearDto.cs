using System.ComponentModel.DataAnnotations;    

namespace PARCIAL1.Models
{
    public class TipovehiculoCrearDto
    {
        public string Nombre { get; set; } = null!;
        public decimal TarifaPorDia { get; set; }
    }
}
