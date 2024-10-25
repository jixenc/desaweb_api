namespace DesaWeb.Api.Venta.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string CodigoProducto { get; set; }
        public string Descripcion { get; set; }
        public string CodigoProveedor { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string UbicacionFisica { get; set; }
        public int ExistenciaMinima { get; set; }
        public decimal Precio { get; set; }  // Nueva propiedad para el precio
        public string Estado { get; set; } // 'V' para activo, 'I' para inactivo
    }
}

