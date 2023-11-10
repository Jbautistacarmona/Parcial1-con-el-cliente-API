namespace PARCIAL1.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
    }
}
