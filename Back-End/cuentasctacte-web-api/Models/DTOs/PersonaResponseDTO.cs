namespace cuentasctacte_web_api.Models.DTOs
{
    public class PersonaResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string DocumentoTipo { get; set; }
        public string UserName { get; set; }

    }
}