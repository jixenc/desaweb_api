using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using VentaModel = DesaWeb.Api.Venta.Models.Venta;

namespace DesaWeb.Api.Venta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {

        private readonly MySQLDbContext _context;


        public VentaController(MySQLDbContext context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaModel venta)
        {
            if (venta == null || venta.DetalleVenta == null || venta.DetalleVenta.Count == 0)
            return BadRequest(new { mensaje = "Datos de la venta o detalle de la venta inválidos" });

            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_registrar_venta", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("i_iClienteID", venta.ClienteID);
                command.Parameters.AddWithValue("i_vTipoVenta", venta.TipoVenta ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_dTotalVenta", venta.TotalVenta);
                command.Parameters.AddWithValue("i_dFechaVenta", venta.FechaVenta);
                command.Parameters.AddWithValue("i_vEstado", venta.Estado ?? (object)DBNull.Value);

                // Convertir el detalle de la venta a formato JSON
                var detalleJson = JsonSerializer.Serialize(venta.DetalleVenta);
                command.Parameters.AddWithValue("i_tDetalleVenta", detalleJson);

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Venta registrada correctamente", filasAfectadas = result });
            }
            catch (MySqlException ex)
            {

                return StatusCode(500, new
                {
                    mensaje = "Error al interactuar con la base de datos",
                    error = ex.Message
                });
            }

        }
    }
}
