using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DesaWeb.Api.Venta.Controllers.APIS
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User userLogin)
        {
            if (userLogin.UserUserName == "admin" && userLogin.UserPass == "test23?*")
            {
                var tokenString = GenerateJwtToken();
                return Ok(new { Token = tokenString });
            }
            return Unauthorized();

        }
        private string GenerateJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("t3mpUs1apiW3b3dt@n0V=?"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "app.com",
                audience: "app.com",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
