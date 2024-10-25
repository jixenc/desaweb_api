using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DesaWeb.Api.Venta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {

        private readonly MySQLDbContext _context;

        public ProductoController(MySQLDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear-o-actualizar")]
        public async Task<IActionResult> CrearOActualizarProducto([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest(new { mensaje = "Datos de producto inválidos" });

            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_producto", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("i_vCodigoProducto", producto.CodigoProducto);
                command.Parameters.AddWithValue("i_vDescripcion", producto.Descripcion);
                command.Parameters.AddWithValue("i_vCodigoProveedor", producto.CodigoProveedor);
                command.Parameters.AddWithValue("i_dFechaVencimiento", producto.FechaVencimiento);
                command.Parameters.AddWithValue("i_vUbicacionFisica", producto.UbicacionFisica);
                command.Parameters.AddWithValue("i_iExistenciaMinima", producto.ExistenciaMinima);
                command.Parameters.AddWithValue("i_dPrecio", producto.Precio);  // Nuevo parámetro para el precio
                command.Parameters.AddWithValue("i_vEstado", producto.Estado);
                command.Parameters.AddWithValue("i_vAccion", "C");  // 'C' para crear o actualizar
                command.Parameters.AddWithValue("i_iProductoID", producto.ProductoID);

                var result = await command.ExecuteNonQueryAsync();
                return Ok(new { mensaje = "Producto creado o actualizado correctamente", filasAfectadas = result });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { mensaje = "Error al interactuar con la base de datos", error = ex.Message });
            }

        }

        [HttpGet("listar-todo")]
        public async Task<IActionResult> ListarTodosProductos()
        {
            try
            {
                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_producto", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros para la acción 'T' (Listar todos los productos)
                command.Parameters.AddWithValue("i_vAccion", "T"); // 'T' para leer todos los productos
                command.Parameters.AddWithValue("i_iProductoID", DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoProducto", DBNull.Value);
                command.Parameters.AddWithValue("i_vDescripcion", DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_dFechaVencimiento", DBNull.Value);
                command.Parameters.AddWithValue("i_vUbicacionFisica", DBNull.Value);
                command.Parameters.AddWithValue("i_iExistenciaMinima", DBNull.Value);
                command.Parameters.AddWithValue("i_dPrecio", DBNull.Value); // Parámetro del precio
                command.Parameters.AddWithValue("i_vEstado", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                var productos = new List<Producto>();

                // Leer todos los productos
                while (await reader.ReadAsync())
                {
                    var producto = new Producto
                    {
                        ProductoID = reader.GetInt32("ProductoID"),
                        CodigoProducto = reader.GetString("CodigoProducto"),
                        Descripcion = reader.GetString("Descripcion"),
                        CodigoProveedor = reader.GetString("CodigoProveedor"),
                        FechaVencimiento = reader.GetDateTime("FechaVencimiento"),
                        UbicacionFisica = reader.GetString("UbicacionFisica"),
                        ExistenciaMinima = reader.GetInt32("ExistenciaMinima"),
                        Precio = reader.GetDecimal("Precio"), // Atributo del precio
                        Estado = reader.GetString("Estado")
                    };

                    productos.Add(producto); // Agregar cada producto a la lista
                }

                if (productos.Count > 0)
                {
                    return Ok(productos); // Devolver la lista de productos
                }
                else
                {
                    return NotFound(new { mensaje = "No se encontraron productos disponibles" });
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, $"Error al interactuar con la base de datos: {ex.Message}");
            }
        }


        [HttpGet("leer/{id}")]
        public async Task<IActionResult> LeerProducto(int id)
        {
            try
            {

                using var connection = _context.GetConnection();
                await connection.OpenAsync();

                using var command = new MySqlCommand("sp_crud_producto", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("i_vAccion", "R"); // 'R' para leer
                command.Parameters.AddWithValue("i_iProductoID", id);
                command.Parameters.AddWithValue("i_vCodigoProducto", DBNull.Value);
                command.Parameters.AddWithValue("i_vDescripcion", DBNull.Value);
                command.Parameters.AddWithValue("i_vCodigoProveedor", DBNull.Value);
                command.Parameters.AddWithValue("i_dFechaVencimiento", DBNull.Value);
                command.Parameters.AddWithValue("i_vUbicacionFisica", DBNull.Value);
                command.Parameters.AddWithValue("i_iExistenciaMinima", DBNull.Value);
                command.Parameters.AddWithValue("i_dPrecio", DBNull.Value); // Nuevo parámetro para el precio
                command.Parameters.AddWithValue("i_vEstado", DBNull.Value);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    var producto = new Producto
                    {
                        ProductoID = reader.GetInt32("ProductoID"),
                        CodigoProducto = reader.GetString("CodigoProducto"),
                        Descripcion = reader.GetString("Descripcion"),
                        CodigoProveedor = reader.GetString("CodigoProveedor"),
                        FechaVencimiento = reader.GetDateTime("FechaVencimiento"),
                        UbicacionFisica = reader.GetString("UbicacionFisica"),
                        ExistenciaMinima = reader.GetInt32("ExistenciaMinima"),
                        Precio = reader.GetDecimal("Precio"), // Nuevo atributo para el precio
                        Estado = reader.GetString("Estado")
                    };
                    return Ok(producto);
                }
                return NotFound("Producto no encontrado");
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
