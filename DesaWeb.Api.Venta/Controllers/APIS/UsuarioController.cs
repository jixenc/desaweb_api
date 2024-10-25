using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
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
            return BadRequest(new { mensaje = "Datos de usuario inválidos" });

        try
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand("sp_crud_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("i_vCorreo", usuario.Correo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vContraseña", usuario.Contraseña ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vNombre", usuario.Nombre ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vApellido", usuario.Apellido ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vTelefono", usuario.Telefono ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vDireccion", usuario.Direccion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vEstado", usuario.Estado ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("i_iRolID", usuario.RolID > 0 ? usuario.RolID : (object)DBNull.Value);
            command.Parameters.AddWithValue("i_vAccion", usuario.Accion);
            command.Parameters.AddWithValue("i_iUsuarioID", usuario.UsuarioID > 0 ? usuario.UsuarioID : (object)DBNull.Value);

            var result = await command.ExecuteNonQueryAsync();
            return Ok(new
            {
                mensaje = "Usuario creado o actualizado exitosamente",
                filasAfectadas = result
            });
        }
        catch (MySqlException ex)
        {
            return BadRequest(new
            {
                mensaje = "Error al ejecutar el procedimiento",
                error = ex.Message
            });
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

            command.Parameters.AddWithValue("i_vCorreo", DBNull.Value);
            command.Parameters.AddWithValue("i_vContraseña", DBNull.Value);
            command.Parameters.AddWithValue("i_vNombre", DBNull.Value);
            command.Parameters.AddWithValue("i_vApellido", DBNull.Value);
            command.Parameters.AddWithValue("i_vTelefono", DBNull.Value);
            command.Parameters.AddWithValue("i_vDireccion", DBNull.Value);
            command.Parameters.AddWithValue("i_vEstado", DBNull.Value);
            command.Parameters.AddWithValue("i_iRolID", DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                var usuario = new Usuario
                {
                    UsuarioID = reader.GetInt32("UsuarioID"),
                    Correo = reader.GetString("Correo"),
                    Contraseña = reader.GetString("Contraseña"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.GetString("Telefono"),
                    Direccion = reader.GetString("Direccion"),
                    Estado = reader.GetString("Estado"),
                    RolID = reader.GetInt32("RolID")
                };
                return Ok(usuario);
            }
            return NotFound(new
            {
                mensaje = "Usuario no encontrado"
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

    [HttpDelete("eliminar/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
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
            command.Parameters.AddWithValue("i_vAccion", "D");

            command.Parameters.AddWithValue("i_vCorreo", DBNull.Value);
            command.Parameters.AddWithValue("i_vContraseña", DBNull.Value);
            command.Parameters.AddWithValue("i_vNombre", DBNull.Value);
            command.Parameters.AddWithValue("i_vApellido", DBNull.Value);
            command.Parameters.AddWithValue("i_vTelefono", DBNull.Value);
            command.Parameters.AddWithValue("i_vDireccion", DBNull.Value);
            command.Parameters.AddWithValue("i_vEstado", DBNull.Value);
            command.Parameters.AddWithValue("i_iRolID", DBNull.Value);

            var result = await command.ExecuteNonQueryAsync();
            return Ok(new { mensaje = "Usuario eliminado exitosamente", filasAfectadas = result });
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

    [HttpGet("listar-todos")]
    public async Task<IActionResult> ListarTodosUsuarios()
    {
        try
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand("sp_crud_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("i_iUsuarioID", DBNull.Value);
            command.Parameters.AddWithValue("i_vAccion", "T");

            command.Parameters.AddWithValue("i_vCorreo", DBNull.Value);
            command.Parameters.AddWithValue("i_vContraseña", DBNull.Value);
            command.Parameters.AddWithValue("i_vNombre", DBNull.Value);
            command.Parameters.AddWithValue("i_vApellido", DBNull.Value);
            command.Parameters.AddWithValue("i_vTelefono", DBNull.Value);
            command.Parameters.AddWithValue("i_vDireccion", DBNull.Value);
            command.Parameters.AddWithValue("i_vEstado", DBNull.Value);
            command.Parameters.AddWithValue("i_iRolID", DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();

            var usuarios = new List<Usuario>();
            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    UsuarioID = reader.GetInt32("UsuarioID"),
                    Correo = reader.GetString("Correo"),
                    Contraseña = reader.GetString("Contraseña"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Telefono = reader.GetString("Telefono"),
                    Direccion = reader.GetString("Direccion"),
                    Estado = reader.GetString("Estado"),
                    RolID = reader.GetInt32("RolID")
                });
            }

            return Ok(usuarios);
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

    [HttpPost("login")]
    public async Task<IActionResult> LoginUsuario([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Correo) || string.IsNullOrEmpty(loginRequest.Contraseña))
            return BadRequest(new { mensaje = "Correo y contraseña son requeridos." });

        try
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            using var command = new MySqlCommand("sp_crud_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("i_iUsuarioID", DBNull.Value);
            command.Parameters.AddWithValue("i_vAccion", "L");

            command.Parameters.AddWithValue("i_vCorreo", loginRequest.Correo);
            command.Parameters.AddWithValue("i_vContraseña", loginRequest.Contraseña);
            command.Parameters.AddWithValue("i_vNombre", DBNull.Value);
            command.Parameters.AddWithValue("i_vApellido", DBNull.Value);
            command.Parameters.AddWithValue("i_vTelefono", DBNull.Value);
            command.Parameters.AddWithValue("i_vDireccion", DBNull.Value);
            command.Parameters.AddWithValue("i_vEstado", DBNull.Value);
            command.Parameters.AddWithValue("i_iRolID", DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                var usuarioEncontrado = new Usuario
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
                return Ok(usuarioEncontrado);
            }

            return Unauthorized(new
            {
                mensaje = "Correo o contraseña incorrectos."
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




}
