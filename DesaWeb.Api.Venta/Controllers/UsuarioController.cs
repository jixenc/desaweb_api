using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
//using ApiUsuarios.Models;
//using ApiUsuarios.Data;
using System.Data;
using DesaWeb.Api.Venta.Models;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly MySQLDbContext _context;

    public UsuarioController(MySQLDbContext context)
    {
        _context = context;
    }

    [HttpPost("crear-o-actualizar")]
    public async Task<IActionResult> CrearOActualizarUsuario([FromBody] Usuario usuario)
    {
        if (usuario == null)
            return BadRequest("Datos de usuario inválidos");

        try
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand("sp_crud_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("i_vCorreo", usuario.Correo);
            command.Parameters.AddWithValue("i_vContraseña", usuario.Contraseña);
            command.Parameters.AddWithValue("i_vNombre", usuario.Nombre);
            command.Parameters.AddWithValue("i_vApellido", usuario.Apellido);
            command.Parameters.AddWithValue("i_vTelefono", usuario.Telefono);
            command.Parameters.AddWithValue("i_vDireccion", usuario.Direccion);
            command.Parameters.AddWithValue("i_vEstado", usuario.Estado);
            command.Parameters.AddWithValue("i_iRolID", usuario.RolID);
            command.Parameters.AddWithValue("i_vAccion", usuario.Accion);
            command.Parameters.AddWithValue("i_iUsuarioID", 0);

            var result = await command.ExecuteNonQueryAsync();
            return Ok(new { mensaje = "Usuario creado o actualizado exitosamente", filasAfectadas = result });
        }
        catch (MySqlException ex)
        {
            return StatusCode(500, $"Error al interactuar con la base de datos: {ex.Message}");
        }
    }


    [HttpGet("leer/{id}")]
    public async Task<IActionResult> LeerUsuario(int id)
    {
        try
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand("sp_crud_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("i_iUsuarioID", id);
            command.Parameters.AddWithValue("i_vAccion", "R");

            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                var usuario = new Usuario
                {
                    UsuarioID = reader.GetInt32("UsuarioID"),
                    Correo = reader.GetString("Correo"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.GetString("Telefono"),
                    Direccion = reader.GetString("Direccion"),
                    Estado = reader.GetString("Estado"),
                    RolID = reader.GetInt32("RolID")
                };
                return Ok(usuario);
            }
            return NotFound("Usuario no encontrado");
        }
        catch (MySqlException ex)
        {
            return StatusCode(500, $"Error al interactuar con la base de datos: {ex.Message}");
        }
    }

}
