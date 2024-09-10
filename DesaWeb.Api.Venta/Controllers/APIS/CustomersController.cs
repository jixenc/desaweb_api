using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesaWeb.Api.Venta.Controllers.APIS
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {

        [HttpPost("CustomerCreate")]
        public IActionResult CreateCustomer(Customer cusC)
        {

            return Ok();

        }

        [HttpGet("CustConsult/CustCode/{_pcodeC}")]

        public IActionResult CustomerConsulxCode(string _pcodeC)
        {

            return Ok();
        }

        [HttpPut("CustUpdate/{_pcodeU}")]
        public IActionResult CustomerUpdate(string _pcodeU, Customer cusU)
        {

            return Ok();
        }

        [HttpDelete("CustDown/{_pcodeD}")]
        public IActionResult CustomerDown(string _pcodeD)
        {

            return Ok();
        }
    }
}
