using DesaWeb.Api.Venta.Models;
using Microsoft.AspNetCore.Mvc;
using DesaWeb.Api.Venta.Services;
using Microsoft.AspNetCore.Authorization;

namespace DesaWeb.Api.Venta.Controllers.APIS
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService custService)
        {
            _customerService = custService;
        }


        [HttpPost("CustomerCreate")]
        public IActionResult CreateCustomer(Customer cusC)
        {
            if (cusC != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _customerService.SaveCustomer(cusC);
                        return Ok();
                    }
                    catch (Exception ex)
                    {

                        return StatusCode(500, new { Message = "Error al guardar el cliente", Details = ex.Message });
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            return BadRequest("La informacion del cliente es nula");
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
