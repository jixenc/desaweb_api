namespace DesaWeb.Api.Venta.Models
{
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string CodigoProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string TelefonoProveedor { get; set; }
        public string DireccionProveedor { get; set; }
        public string CorreoProveedor { get; set; }
        public string Estado { get; set; } // 'V' para válido, 'I' para inactivo
    }
}

