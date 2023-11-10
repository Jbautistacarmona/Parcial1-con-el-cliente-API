using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PARCIAL1.Models
{
    public class AlquilerCrearDto
    {
        [Required]
        [Display(Name = "Cliente")]
        public int ClienteID { get; set; }
        [Required]
        [Display(Name = "Tipo de Vehículo")]
        public int TipoVehiculoID { get; set; }
        [Required]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set; }
        [Required]
        [Display(Name = "Monto de Cobro")]
        public decimal MontoCobro { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual Tipovehiculo? Tipovehiculo { get; set; }
    }
}
