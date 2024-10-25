using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DesaWeb.Api.Venta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregaController : ControllerBase
    {
        private readonly MySQLDbContext _context;

        // Constructor utilizando inyección de dependencia con MySQLDbContext
        public EntregaController(MySQLDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear-entrega")]
        public async Task<IActionResult> CrearOActualizarEntrega([FromBody] Entrega entrega)
        {
            if (entrega == null)
                return BadRequest(new { mensaje = "Datos de entrega inválidos" });

            try
            {
                // Obtener una conexión desde el contexto
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                // Verificar si la venta existe y está en estado A
                using (var checkVentaCommand = new MySqlCommand("SELECT Estado FROM Venta WHERE VentaID = @VentaID", connection))
                {
                    checkVentaCommand.Parameters.AddWithValue("@VentaID", entrega.VentaID);

                    var estadoVenta = await checkVentaCommand.ExecuteScalarAsync();

                    if (estadoVenta == null)
                    {
                        return NotFound(new { mensaje = "Venta no encontrada" });
                    }

                    if (estadoVenta.ToString() == "I")
                    {
                        return BadRequest(new { mensaje = "Venta ya ha sido entregada" });
                    }
                }

                // Crear o actualizar la entrega
                using var command = new MySqlCommand("sp_crud_entrega", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros requeridos por el procedimiento almacenado
                command.Parameters.AddWithValue("i_iVentaID", entrega.VentaID > 0 ? entrega.VentaID : (object)DBNull.Value);
                //command.Parameters.AddWithValue("i_dFechaEntrega", entrega.FechaEntrega ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoEntrega", !string.IsNullOrEmpty(entrega.EstadoEntrega) ? entrega.EstadoEntrega : (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vUbicacionActual", !string.IsNullOrEmpty(entrega.UbicacionActual) ? entrega.UbicacionActual : (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vAccion", "C");  // Se puede cambiar a "U" si es actualización
                command.Parameters.AddWithValue("i_iEntregaID", entrega.EntregaID > 0 ? entrega.EntregaID : (object)DBNull.Value);

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Entrega creada o actualizada correctamente", filasAfectadas = result });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, $"Error al interactuar con la base de datos: {ex.Message}");
            }
 
        }

        [HttpGet("leer-entrega/{id}")]
        public async Task<IActionResult> LeerEntrega(int id)
        {
            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_entrega", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Solo se usa el ID de entrega para leer
                command.Parameters.AddWithValue("i_vAccion", "R");
                command.Parameters.AddWithValue("i_iEntregaID", id);

                // Parámetros adicionales enviados como vacíos
                command.Parameters.AddWithValue("i_iVentaID", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoEntrega", DBNull.Value);
                command.Parameters.AddWithValue("i_vUbicacionActual", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var entrega = new Entrega
                    {
                        EntregaID = reader.GetInt32("EntregaID"),
                        VentaID = reader.GetInt32("VentaID"),
                        //FechaEntrega = reader.GetDateTime("FechaEntrega"),
                        EstadoEntrega = reader.GetString("EstadoEntrega"),
                        UbicacionActual = reader.GetString("UbicacionActual")
                    };
                    return Ok(entrega);
                }
                return NotFound(new
                {
                    mensaje = "Entrega no encontrada"
                });
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

        [HttpDelete("eliminar-entrega/{id}")]
        public async Task<IActionResult> EliminarEntrega(int id)
        {
            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_entrega", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para eliminar la entrega
                command.Parameters.AddWithValue("i_vAccion", "D");
                command.Parameters.AddWithValue("i_iEntregaID", id);

                // Parámetros adicionales enviados como vacíos
                command.Parameters.AddWithValue("i_iVentaID", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoEntrega", DBNull.Value);
                command.Parameters.AddWithValue("i_vUbicacionActual", DBNull.Value);

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Entrega eliminada correctamente", filasAfectadas = result });
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
