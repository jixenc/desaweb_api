using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DesaWeb.Api.Venta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly MySQLDbContext _context;

        // Constructor usando inyección de dependencia para la conexión a la base de datos a través de MySQLDbContext
        public ProveedorController(MySQLDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("crear-o-actualizar")]
        public async Task<IActionResult> CrearOActualizarProveedor([FromBody] Proveedor proveedor)
        {
            if (proveedor == null)

                return BadRequest(new { mensaje = "Datos del proveedor inválidos" });

            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_proveedor", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros requeridos por el procedimiento almacenado
                command.Parameters.AddWithValue("i_iProveedorID", proveedor.ProveedorID > 0 ? proveedor.ProveedorID : (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoProveedor", proveedor.CodigoProveedor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vNombreProveedor", proveedor.NombreProveedor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoProveedor", proveedor.TelefonoProveedor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionProveedor", proveedor.DireccionProveedor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoProveedor", proveedor.CorreoProveedor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vEstado", proveedor.Estado ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("i_vAccion", proveedor.ProveedorID > 0 ? "U" : "C"); // "C" para crear, "U" para actualizar

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Proveedor creado o actualizado exitosamente", filasAfectadas = result });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { mensaje = "Error al interactuar con la base de datos", error = ex.Message });
            }
        }

        [HttpGet("leer/{id}")]
        public async Task<IActionResult> LeerProveedor(int id)
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_proveedor", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("i_iProveedorID", id);
                command.Parameters.AddWithValue("i_vAccion", "R");

                // Agregar parámetros no usados con valores nulos
                command.Parameters.AddWithValue("i_vCodigoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombreProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstado", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    var proveedor = new Proveedor
                    {
                        ProveedorID = reader.GetInt32("ProveedorID"),
                        CodigoProveedor = reader.GetString("CodigoProveedor"),
                        NombreProveedor = reader.GetString("NombreProveedor"),
                        TelefonoProveedor = reader.GetString("TelefonoProveedor"),
                        DireccionProveedor = reader.GetString("DireccionProveedor"),
                        CorreoProveedor = reader.GetString("CorreoProveedor"),
                        Estado = reader.GetString("Estado")
                    };
                    return Ok(proveedor);
                }
                return NotFound(new { mensaje = "Proveedor no encontrado" });
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

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_proveedor", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("i_iProveedorID", id);
                command.Parameters.AddWithValue("i_vAccion", "D");

                // Agregar parámetros no usados con valores nulos
                command.Parameters.AddWithValue("i_vCodigoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombreProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstado", DBNull.Value);

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Proveedor eliminado exitosamente", filasAfectadas = result });
            }
            catch (MySqlException ex)
            {
                // Retornar un mensaje de error si ocurre una excepción con la base de datos
                return StatusCode(500, new
                {
                    mensaje = "Error al interactuar con la base de datos",
                    error = ex.Message
                });
            }
        }


        [HttpGet("listar-todo")]
        public async Task<IActionResult> ListarTodosProveedores()
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_proveedor", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para la acción 'T' (Listar todos los proveedores)
                command.Parameters.AddWithValue("i_vAccion", "T"); // 'T' para leer todos los proveedores
                command.Parameters.AddWithValue("i_iProveedorID", DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombreProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstado", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                var proveedores = new List<Proveedor>();

                // Leer todos los proveedores
                while (await reader.ReadAsync())
                {
                    var proveedor = new Proveedor
                    {
                        ProveedorID = reader.GetInt32("ProveedorID"),
                        CodigoProveedor = reader.GetString("CodigoProveedor"),
                        NombreProveedor = reader.GetString("NombreProveedor"),
                        TelefonoProveedor = reader.GetString("TelefonoProveedor"),
                        DireccionProveedor = reader.GetString("DireccionProveedor"),
                        CorreoProveedor = reader.GetString("CorreoProveedor"),
                        Estado = reader.GetString("Estado")
                    };

                    proveedores.Add(proveedor); // Agregar cada proveedor a la lista
                }

                if (proveedores.Count > 0)
                {
                    return Ok(proveedores); // Devolver la lista de proveedores
                }
                else
                {
                    return NotFound(new { mensaje = "No se encontraron proveedores disponibles" });
                }
            }
            catch (MySqlException ex)
            {
                // Retornar un mensaje de error si ocurre una excepción con la base de datos
                return StatusCode(500, new
                {
                    mensaje = "Error al interactuar con la base de datos",
                    error = ex.Message
                });
            }
        }



    }
}
