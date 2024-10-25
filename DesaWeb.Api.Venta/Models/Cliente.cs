namespace DesaWeb.Api.Venta.Models
{
    public class Cliente
    {
        public string CodigoCliente { get; set; }
        public string NombresCliente { get; set; }
        public string ApellidosCliente { get; set; } 
        public string DireccionCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string CorreoCliente { get; set; }
        public string NIT { get; set; }
        public string CategoriaCliente { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string EstadoCliente { get; set; }
        public string Accion { get; set; }
        public int? ClienteID { get; set; }
    }
}

