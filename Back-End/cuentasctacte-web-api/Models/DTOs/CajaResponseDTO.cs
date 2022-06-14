namespace cuentasctacte_web_api.Models.DTOs
{
    public class CajaResponseDTO
    {
        public int IdCaja { get; set; }
        public string Nombre { get; set; }
        public string Cajero { get; set; }
        public string UserName { get; set; }
        public double Saldo { get; set; }
    }
}
