namespace DesaWeb.Api.Venta.Models
{
    public class Entrega
    {
        public int EntregaID { get; set; }
        public int VentaID { get; set; }
        //public DateTime? FechaEntrega { get; set; }
        public string EstadoEntrega { get; set; } // 'V' para válida, 'I' para inactiva
        public string UbicacionActual { get; set; }
    }

}

