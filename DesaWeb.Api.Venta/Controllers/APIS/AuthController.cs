using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesaWeb.Api.Venta.Controllers.APIS
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User userLogin)
        {

            return Ok();
        }
    }
}
