namespace DesaWeb.Api.Venta.Models
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public string TipoVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }
        public string Estado { get; set; } // 'V' para válida
        public List<DetalleVenta> DetalleVenta { get; set; }
    }
    public class DetalleVenta
    {
        public int DetalleVentaID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class DetalleVentaDTO
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
    }
}

