using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using DesaWeb.Api.Venta.Models;
using MySqlX.XDevAPI.Common;

namespace DesaWeb.Api.Venta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly MySQLDbContext _context;

        public ClienteController(MySQLDbContext context)
        {
            _context = context;
        }

        [HttpPost("crud")]
        public async Task<IActionResult> EjecutarProcedimientoAlmacenado([FromBody] Cliente request)
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_cliente", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Agregar los parámetros del procedimiento
                command.Parameters.AddWithValue("i_vCodigoCliente", request.CodigoCliente);
                command.Parameters.AddWithValue("i_vNombresCliente", request.NombresCliente);
                command.Parameters.AddWithValue("i_vApellidosCliente", request.ApellidosCliente); 
                command.Parameters.AddWithValue("i_vDireccionCliente", request.DireccionCliente);
                command.Parameters.AddWithValue("i_vTelefonoCliente", request.TelefonoCliente);
                command.Parameters.AddWithValue("i_vCorreoCliente", request.CorreoCliente);
                command.Parameters.AddWithValue("i_vNIT", request.NIT);
                command.Parameters.AddWithValue("i_vCategoriaCliente", request.CategoriaCliente);
                command.Parameters.AddWithValue("i_vDepartamento", request.Departamento);
                command.Parameters.AddWithValue("i_vMunicipio", request.Municipio);
                command.Parameters.AddWithValue("i_vEstadoCliente", request.EstadoCliente);
                command.Parameters.AddWithValue("i_vAccion", request.Accion);
                command.Parameters.AddWithValue("i_iClienteID", request.ClienteID);

                // Ejecutar el procedimiento almacenado
                var result = await command.ExecuteNonQueryAsync();

                // Retornar respuesta en formato JSON sencillo
                return Ok(new
                {
                    mensaje = "Procedimiento ejecutado exitosamente",
                    filasAfectadas = result
                });
            }
            catch (Exception ex)
            {
                // Retornar mensaje de error en formato JSON
                return BadRequest(new
                {
                    mensaje = "Error al ejecutar el procedimiento",
                    error = ex.Message
                });
            }
        }

        [HttpGet("leer/{id}")]
        public async Task<IActionResult> LeerCliente(int id)
        {
            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_cliente", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para leer el cliente
                command.Parameters.AddWithValue("i_vAccion", "R");  // Acción 'R' para lectura
                command.Parameters.AddWithValue("i_iClienteID", id);
                command.Parameters.AddWithValue("i_vCodigoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombresCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vApellidosCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNIT", DBNull.Value);
                command.Parameters.AddWithValue("i_vCategoriaCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDepartamento", DBNull.Value);
                command.Parameters.AddWithValue("i_vMunicipio", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoCliente", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var cliente = new Cliente
                    {
                        ClienteID = reader.GetInt32("ClienteID"),
                        CodigoCliente = reader.GetString("CodigoCliente"),
                        NombresCliente = reader.GetString("NombresCliente"),
                        ApellidosCliente = reader.GetString("ApellidosCliente"),
                        DireccionCliente = reader.GetString("DireccionCliente"),
                        NIT = reader.GetString("NIT"),
                        CategoriaCliente = reader.GetString("CategoriaCliente"),
                        EstadoCliente = reader.GetString("EstadoCliente"),
                        Departamento = reader.GetString("Departamento"),
                        Municipio = reader.GetString("Municipio"),
                        TelefonoCliente = reader.GetString("Telefono"),
                        CorreoCliente = reader.GetString("Correo")
                    };
                    return Ok(cliente);
                }
                else
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al interactuar con la base de datos",
                    error = ex.Message
                });
            }

        }

        [HttpGet("listar-todo")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_cliente", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para leer el cliente
                command.Parameters.AddWithValue("i_vAccion", "T");  // Acción 'T' para leer todos los clientes
                command.Parameters.AddWithValue("i_iClienteID", DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombresCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vApellidosCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNIT", DBNull.Value);
                command.Parameters.AddWithValue("i_vCategoriaCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDepartamento", DBNull.Value);
                command.Parameters.AddWithValue("i_vMunicipio", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoCliente", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                var clientes = new List<Cliente>();

                while (await reader.ReadAsync())  // Cambiamos if por while para leer todas las filas
                {
                    var cliente = new Cliente
                    {
                        ClienteID = reader.GetInt32("ClienteID"),
                        CodigoCliente = reader.GetString("CodigoCliente"),
                        NombresCliente = reader.GetString("NombresCliente"),
                        ApellidosCliente = reader.GetString("ApellidosCliente"),
                        DireccionCliente = reader.GetString("DireccionCliente"),
                        NIT = reader.GetString("NIT"),
                        CategoriaCliente = reader.GetString("CategoriaCliente"),
                        EstadoCliente = reader.GetString("EstadoCliente"),
                        Departamento = reader.GetString("Departamento"),
                        Municipio = reader.GetString("Municipio"),
                        TelefonoCliente = reader.GetString("Telefono"),
                        CorreoCliente = reader.GetString("Correo")
                    };

                    clientes.Add(cliente);  // Añadir cada cliente a la lista
                }

                if (clientes.Count > 0)
                {
                    return Ok(clientes);  // Devolver todos los clientes
                }
                else
                {
                    return NotFound(new { mensaje = "No se encontraron clientes" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al interactuar con la base de datos",
                    error = ex.Message
                });
            }
        }



        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_cliente", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para eliminar el cliente
                command.Parameters.AddWithValue("i_vAccion", "D");  // Acción 'D' para eliminar (inactivar)
                command.Parameters.AddWithValue("i_iClienteID", id);
                command.Parameters.AddWithValue("i_vCodigoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNombresCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vApellidosCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDireccionCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vTelefonoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vCorreoCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vNIT", DBNull.Value);
                command.Parameters.AddWithValue("i_vCategoriaCliente", DBNull.Value);
                command.Parameters.AddWithValue("i_vDepartamento", DBNull.Value);
                command.Parameters.AddWithValue("i_vMunicipio", DBNull.Value);
                command.Parameters.AddWithValue("i_vEstadoCliente", DBNull.Value);

                var result = await command.ExecuteNonQueryAsync();
                if (result > 0)
                {
                    return Ok(new { mensaje = "Cliente eliminado (inactivo) correctamente" });
                }
                else
                {
                    return NotFound(new { mensaje = "Cliente no encontrado para eliminar" });
                }
            }
            catch (Exception ex)
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
